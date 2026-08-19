namespace ApiNotasPostulante.Domain.Postulante;

public interface IActualizarNotasPostulanteRepository
{
    Task<List<TblPostulante>> ObtenerPostulantesPorAlumnoAsync(string idAlumno, CancellationToken cancellationToken);

    Task<TblEscuela?> ObtenerEscuelaAsync(string idEscuela, CancellationToken cancellationToken);

    Task<Dictionary<string, string>> ObtenerConfiguracionActivaAsync(CancellationToken cancellationToken);

    Task<int> GuardarCambiosAsync(CancellationToken cancellationToken);
}
