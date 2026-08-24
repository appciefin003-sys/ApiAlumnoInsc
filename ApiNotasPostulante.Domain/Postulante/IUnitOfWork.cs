namespace ApiNotasPostulante.Domain.Postulante;

public interface IUnitOfWork
{
    IActualizarNotasPostulanteRepository PostulanteNotas { get; }

    Task<int> GuardarCambiosAsync(CancellationToken cancellationToken);
}
