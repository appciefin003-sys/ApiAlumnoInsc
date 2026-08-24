using ApiNotasPostulante.Domain.Postulante;
using ApiNotasPostulante.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ApiNotasPostulante.Persistence.Repositories;

public class ActualizarNotasPostulanteRepository : IActualizarNotasPostulanteRepository
{
    private readonly UcciDbContext _context;

    public ActualizarNotasPostulanteRepository(UcciDbContext context)
    {
        _context = context;
    }

    public async Task<List<TblPostulante>> ObtenerPostulantesPorAlumnoAsync(string idAlumno, CancellationToken cancellationToken)
    {
        return await _context.TblPostulante
            .Where(p => p.IDAlumno == idAlumno)
            .ToListAsync(cancellationToken);
    }

    public async Task<TblEscuela?> ObtenerEscuelaAsync(string idEscuela, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(idEscuela))
        {
            return null;
        }

        return await _context.TblEscuela
            .Where(e => e.IDEscuela == idEscuela)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Dictionary<string, string>> ObtenerConfiguracionActivaAsync(CancellationToken cancellationToken)
    {
        return await _context.TblConfigSyncNotasUC
            .Where(c => c.Flg == true)
            .ToDictionaryAsync(c => c.Variable, c => c.Valor ?? string.Empty, cancellationToken);
    }

}
