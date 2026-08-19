using ApiNotasPostulante.Application.DTOs;
using ApiNotasPostulante.CrossCutting;
using ApiNotasPostulante.Domain.Postulante;

namespace ApiNotasPostulante.Application.Features;

public class ActualizarNotasPostulanteHandler
{
    private const int LongitudIDAlumno = 10;
    private const int LongitudIDPerAcad = 6;
    private const int LongitudIDEscuela = 3;
    private const int LongitudIDEscuelaADM = 3;
    private const string IDFacultadCienciasSalud = "CS";
    private const string IdEscuelaMedicina = "502";
    private const string ModalidadAdmision = "ADM";

    private readonly IActualizarNotasPostulanteRepository _repository;

    public ActualizarNotasPostulanteHandler(IActualizarNotasPostulanteRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<bool>> EjecutarAsync(ActualizarNotasPostulanteRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.IDAlumno))
            {
                return Falla("IDAlumno requerido");
            }

            if (request.IDAlumno.Length > LongitudIDAlumno)
            {
                return Falla($"IDAlumno no puede exceder {LongitudIDAlumno} caracteres");
            }

            if (string.IsNullOrWhiteSpace(request.IDPerAcad))
            {
                return Falla("IDPerAcad requerido");
            }

            if (request.IDPerAcad.Length > LongitudIDPerAcad)
            {
                return Falla($"IDPerAcad no puede exceder {LongitudIDPerAcad} caracteres");
            }

            if (string.IsNullOrWhiteSpace(request.Ingresante))
            {
                return Falla("Ingresante requerido");
            }

            if (request.Ingresante != "1")
            {
                return Falla("Ingresante debe ser 1");
            }

            if (string.IsNullOrWhiteSpace(request.Asistio))
            {
                return Falla("Asistio requerido");
            }

            if (request.Asistio != "1")
            {
                return Falla("No rindió evaluación momento 1");
            }

            if (string.IsNullOrWhiteSpace(request.IDEscuela))
            {
                return Falla("IDEscuela requerido");
            }

            if (request.IDEscuela.Length > LongitudIDEscuela)
            {
                return Falla($"IDEscuela no puede exceder {LongitudIDEscuela} caracteres");
            }

            if (string.IsNullOrWhiteSpace(request.IDEscuelaADM))
            {
                return Falla("IDEscuelaADM requerido");
            }

            if (request.IDEscuelaADM.Length > LongitudIDEscuelaADM)
            {
                return Falla($"IDEscuelaADM no puede exceder {LongitudIDEscuelaADM} caracteres");
            }

            if (!request.Nota1.HasValue || !request.Nota2.HasValue || !request.Nota3.HasValue || !request.Nota4.HasValue)
            {
                return Falla("Notas incompletas");
            }

            var postulantes = await _repository.ObtenerPostulantesPorAlumnoAsync(request.IDAlumno, cancellationToken);
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

            var modalidad = request.IDEscuelaADM;
            var escuela = await _repository.ObtenerEscuelaAsync(request.IDEscuela, cancellationToken);
            if (escuela is null)
            {
                return Falla("No existe la escuela indicada");
            }

            if (request.IDEscuela == IdEscuelaMedicina)
            {
                return Falla("Es Medicina");
            }

            if (modalidad != ModalidadAdmision && modalidad != "ADV" && modalidad != "ADG")
            {
                return Falla("Modalidad de admisión no soportada");
            }

            var esModalidadADM = modalidad == ModalidadAdmision;
            var idFacultad = escuela.IDFacultad?.Trim();
            if (string.IsNullOrWhiteSpace(idFacultad))
            {
                return Falla("La escuela no tiene una facultad configurada");
            }

            var nota1 = request.Nota1.Value;
            var nota2 = request.Nota2.Value;
            var nota3 = request.Nota3.Value;
            var nota4 = request.Nota4.Value;
            var esFacultadCS = idFacultad == IDFacultadCienciasSalud;
            var puntaje = esFacultadCS
                ? Math.Round(
                    esModalidadADM
                        ? nota3 * 0.35m + nota4 * 0.35m + nota2 * 0.15m + nota1 * 0.15m
                        : nota1 * 0.35m + nota2 * 0.35m + nota3 * 0.15m + nota4 * 0.15m,
                    2,
                    MidpointRounding.AwayFromZero)
                : esModalidadADM ? nota3 : nota1;

            var coincidencias = postulantes
                .Where(p => p.IDPerAcad == request.IDPerAcad
                         && p.Ingresante == "1"
                         && p.Asistio == "1")
                .ToList();

            if (coincidencias.Count == 0)
            {
                return Falla("No rindió evaluación momento 1");
            }

            foreach (var postulante in coincidencias)
            {
                if (esModalidadADM)
                {
                    postulante.RV = nota1;
                    postulante.RM = nota2;
                    postulante.LE = nota3;
                    postulante.CO = nota4;
                }
                else
                {
                    postulante.N2 = nota1;
                    postulante.N5 = nota2;
                    postulante.N6 = nota3;
                    postulante.N7 = nota4;
                }

                postulante.Puntaje = puntaje;
            }

            await _repository.GuardarCambiosAsync(cancellationToken);

            return new Response<bool>
            {
                IsSuccess = true,
                Data = true,
                Message = "Notas actualizadas correctamente."
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception)
        {
            return Falla("Ocurrió un error al actualizar las notas");
        }
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

    private sealed class ConfiguracionNotas
    {
        public List<string> PeriodosActivos { get; init; } = new();
        public int DiasHaciaAtras { get; init; }
        public List<string> EscuelasExcluidas { get; init; } = new();
        public string Dependencia { get; init; } = string.Empty;
        public string FlagIngresante { get; init; } = string.Empty;
        public string Renuncia { get; init; } = string.Empty;
        public List<string> TiposPrograma { get; init; } = new();
        public string IDFacultadCS { get; init; } = "CS";
    }
}
