using FNT_Domain;
using FNT_Domain.PostulanteAggregates.Interface;

namespace FNT_Persistence.Context;

public class UnitOfWork : IUnitOfWork
{
    private readonly UcciDbContext _context;
    private bool _disposed;

    public UnitOfWork(
        UcciDbContext context,
        IActualizarNotasPostulanteRepository postulanteNotasRepository)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        PostulanteNotas = postulanteNotasRepository;
    }

    public IActualizarNotasPostulanteRepository PostulanteNotas { get; }

    public Task<int> GuardarCambiosAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _context.Dispose();
        }

        _disposed = true;
    }
}
