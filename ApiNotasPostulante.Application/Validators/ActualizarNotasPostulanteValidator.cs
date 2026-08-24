using ApiNotasPostulante.Application.DTOs;

namespace ApiNotasPostulante.Application.Validators;

public class ActualizarNotasPostulanteValidator
{
    public string? Validar(ActualizarNotasPostulanteRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.IDAlumno))
        {
            return "IDAlumno requerido";
        }

        if (request.IDAlumno.Length > 10)
        {
            return "IDAlumno no puede exceder 10 caracteres";
        }

        if (string.IsNullOrWhiteSpace(request.IDPerAcad))
        {
            return "IDPerAcad requerido";
        }

        if (request.IDPerAcad.Length > 6)
        {
            return "IDPerAcad no puede exceder 6 caracteres";
        }

        if (string.IsNullOrWhiteSpace(request.Ingresante))
        {
            return "Ingresante requerido";
        }

        if (request.Ingresante != "1")
        {
            return "Ingresante debe ser 1";
        }

        if (string.IsNullOrWhiteSpace(request.Asistio))
        {
            return "Asistio requerido";
        }

        if (request.Asistio != "1")
        {
            return "No rindió evaluación momento 1";
        }

        if (string.IsNullOrWhiteSpace(request.IDEscuela))
        {
            return "IDEscuela requerido";
        }

        if (request.IDEscuela.Length > 3)
        {
            return "IDEscuela no puede exceder 3 caracteres";
        }

        if (string.IsNullOrWhiteSpace(request.IDEscuelaADM))
        {
            return "IDEscuelaADM requerido";
        }

        if (request.IDEscuelaADM.Length > 3)
        {
            return "IDEscuelaADM no puede exceder 3 caracteres";
        }

        if (!request.Nota1.HasValue || !request.Nota2.HasValue || !request.Nota3.HasValue || !request.Nota4.HasValue)
        {
            return "Notas incompletas";
        }

        return null;
    }
}
