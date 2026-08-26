using FNT_Domain.PostulanteAggregates.Interface;

namespace FNT_Domain;

public interface IUnitOfWork
{
    IActualizarNotasPostulanteRepository PostulanteNotas { get; }

    Task<int> GuardarCambiosAsync(CancellationToken cancellationToken);
}
