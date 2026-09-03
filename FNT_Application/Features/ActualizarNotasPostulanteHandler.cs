using FNT_Application.DTOs;
using FNT_Application.Validators;
using FNT_CrossCutting;
using FNT_Domain;
using FNT_Domain.PostulanteAggregates;

namespace FNT_Application.Features;

public class ActualizarNotasPostulanteHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ActualizarNotasPostulanteValidator _validator;
    private readonly NotasPostulanteService _notasService;

    public ActualizarNotasPostulanteHandler(
        IUnitOfWork unitOfWork,
        ActualizarNotasPostulanteValidator validator,
        NotasPostulanteService notasService)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _notasService = notasService;
    }

    public async Task<Response<bool>> EjecutarAsync(
        ActualizarNotasPostulanteRequest request,
        CancellationToken cancellationToken)
    {
        var mensajeValidacion = _validator.Validar(request);
        if (mensajeValidacion is not null)
        {
            return Falla(mensajeValidacion);
        }

        var repository = _unitOfWork.PostulanteNotas;
        var postulantes = await repository.ObtenerPostulantesPorAlumnoAsync(request.IDAlumno, cancellationToken);
        if (postulantes.Count == 0)
        {
            return Falla("No existe el postulante");
        }

        if (!postulantes.Any(p => p.IDPerAcad == request.IDPerAcad))
        {
            return Falla("El postulante no figura en el periodo");
        }

        if (!postulantes.Any(p => p.IDPerAcad == request.IDPerAcad && p.Ingresante == "1"))
        {
            return Falla("El postulante no figura como ingresante");
        }

        var escuela = await repository.ObtenerEscuelaAsync(request.IDEscuela!, cancellationToken);
        if (escuela is null)
        {
            return Falla("No existe la escuela indicada");
        }

        if (_notasService.EsMedicina(request.IDEscuela!))
        {
            return Falla("Es Medicina");
        }

        var modalidad = request.IDEscuelaADM!;
        if (!_notasService.EsModalidadSoportada(modalidad))
        {
            return Falla("Modalidad de admisión no soportada");
        }

        var idFacultad = escuela.IDFacultad?.Trim();
        if (string.IsNullOrWhiteSpace(idFacultad))
        {
            return Falla("La escuela no tiene una facultad configurada");
        }

        var coincidencias = postulantes
            .Where(p => p.IDPerAcad == request.IDPerAcad
                     && p.Ingresante == "1"
                     && p.Asistio == "1")
            .ToList();

        if (coincidencias.Count == 0)
        {
            return Falla("No rindió evaluación momento 1");
        }

        var nota1 = (float)request.Nota1.GetValueOrDefault();
        var nota2 = (float)request.Nota2.GetValueOrDefault();
        var nota3 = (float)request.Nota3.GetValueOrDefault();
        var nota4 = (float)request.Nota4.GetValueOrDefault();
        var puntaje = _notasService.CalcularPuntaje(
            idFacultad,
            modalidad,
            nota1,
            nota2,
            nota3,
            nota4);

        _notasService.ActualizarNotas(
            coincidencias,
            modalidad,
            nota1,
            nota2,
            nota3,
            nota4,
            puntaje);

        await _unitOfWork.GuardarCambiosAsync(cancellationToken);

        return new Response<bool>
        {
            IsSuccess = true,
            Data = true,
            Message = "Notas actualizadas correctamente."
        };
    }

    private static Response<bool> Falla(string message)
    {
        return new Response<bool>
        {
            IsSuccess = false,
            Data = false,
            Message = message
        };
    }
}
