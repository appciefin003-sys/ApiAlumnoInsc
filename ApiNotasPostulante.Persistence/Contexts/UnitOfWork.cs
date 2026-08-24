using ApiNotasPostulante.Domain.Postulante;

namespace ApiNotasPostulante.Persistence.Contexts;

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
