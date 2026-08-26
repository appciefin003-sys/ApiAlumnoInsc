using FNT_Domain;
using FNT_Domain.PostulanteAggregates.Interface;

namespace FNT_Persistence.Context;

public class UnitOfWork : IUnitOfWork
{
    private readonly UcciDbContext _context;

    public UnitOfWork(
        UcciDbContext context,
        IActualizarNotasPostulanteRepository postulanteNotasRepository)
    {
        _context = context;
        PostulanteNotas = postulanteNotasRepository;
    }

    public IActualizarNotasPostulanteRepository PostulanteNotas { get; }

    public Task<int> GuardarCambiosAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
