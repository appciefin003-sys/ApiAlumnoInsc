using FNT_Domain.PostulanteAggregates;
using Microsoft.EntityFrameworkCore;

namespace FNT_Persistence.Context;

public class UcciDbContext : DbContext
{
    public UcciDbContext(DbContextOptions<UcciDbContext> options) : base(options)
    {
    }

    public DbSet<TblPostulante> TblPostulante => Set<TblPostulante>();
    public DbSet<TblEscuela> TblEscuela => Set<TblEscuela>();
    public DbSet<TblConfigSyncNotasUC> TblConfigSyncNotasUC => Set<TblConfigSyncNotasUC>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblPostulante>(entity =>
        {
            entity.ToTable("tblPostulante", "dbo", tb =>
            {
                tb.HasTrigger("TRG_IPostulante2BannerPOS");
                tb.HasTrigger("TRG_UNotasPostulante");
                tb.HasTrigger("TRG_Up_Aula_tblPostulante");
                tb.HasTrigger("TRG_PersonaImpedido");
                tb.HasTrigger("TRG_UpdateAsistioPostulanteMED");
                tb.HasTrigger("TRG_UPostulanteCRM");
                tb.HasTrigger("TRG_UIngresantePostu");
                tb.HasTrigger("TRG_UFechaExmCRM");
                tb.HasTrigger("TRG_UEstadoPostulante");
                tb.HasTrigger("TRG_UModalidadCarrera");
                tb.HasTrigger("TRG_ICrearPeriSucePromo");
                tb.HasTrigger("TRG_IUExcepcionesConvalidantes");
                tb.HasTrigger("TRG_DtblPostulante");
                tb.HasTrigger("TRG_ItblPostulante");
                tb.HasTrigger("TRG_UtblPostulante");
            });
            entity.HasKey(e => new { e.IDAlumno, e.IDDependencia, e.IDPerAcad, e.IDExamen, e.IDEscuelaADM });

            entity.Property(e => e.IDAlumno).HasMaxLength(10);
            entity.Property(e => e.IDDependencia).HasMaxLength(4);
            entity.Property(e => e.IDPerAcad).HasMaxLength(6);
            entity.Property(e => e.IDEscuelaADM).HasMaxLength(3);
            entity.Property(e => e.Ingresante).HasMaxLength(1);
            entity.Property(e => e.Asistio).HasMaxLength(1);
            entity.Property(e => e.IDEscuela1).HasMaxLength(3);
            entity.Property(e => e.IDEscuela2).HasMaxLength(3);
            entity.Property(e => e.IngresoOpcion).HasMaxLength(1);
            entity.Property(e => e.Moodle).HasMaxLength(1);
            entity.Property(e => e.msgPostulante).HasMaxLength(800);
            entity.Property(e => e.RV).HasPrecision(18, 2);
            entity.Property(e => e.RM).HasPrecision(18, 2);
            entity.Property(e => e.LE).HasPrecision(18, 2);
            entity.Property(e => e.CO).HasPrecision(18, 2);
            entity.Property(e => e.N2).HasPrecision(18, 2);
            entity.Property(e => e.N5).HasPrecision(18, 2);
            entity.Property(e => e.N6).HasPrecision(18, 2);
            entity.Property(e => e.N7).HasPrecision(18, 2);
            entity.Property(e => e.Puntaje).HasPrecision(18, 2);
        });

        modelBuilder.Entity<TblEscuela>(entity =>
        {
            entity.ToTable("tblEscuela", "dbo");
            entity.HasKey(e => new { e.IDEscuela, e.IDDependencia });

            entity.Property(e => e.IDEscuela).HasMaxLength(3);
            entity.Property(e => e.IDDependencia).HasMaxLength(4);
            entity.Property(e => e.IDFacultad).HasMaxLength(10);
            entity.Property(e => e.IDTipoEsc).HasMaxLength(3);
        });

        modelBuilder.Entity<TblConfigSyncNotasUC>(entity =>
        {
            entity.ToTable("tblConfigSyncNotas_UC", "dbo");
            entity.HasKey(e => e.Variable);

            entity.Property(e => e.Variable).HasMaxLength(100);
            entity.Property(e => e.Valor).HasMaxLength(500);
            entity.Property(e => e.Detalle).HasMaxLength(500);
        });
    }
}
