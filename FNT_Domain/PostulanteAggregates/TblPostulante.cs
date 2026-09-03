namespace FNT_Domain.PostulanteAggregates;

public class TblPostulante
{
    public string IDAlumno { get; set; } = null!;
    public string IDDependencia { get; set; } = null!;
    public string IDPerAcad { get; set; } = null!;
    public DateTime IDExamen { get; set; }
    public string IDEscuelaADM { get; set; } = null!;
    public string? Ingresante { get; set; }
    public string? Asistio { get; set; }
    public string? IDEscuela1 { get; set; }
    public string? IDEscuela2 { get; set; }
    public string? IngresoOpcion { get; set; }
    public float? RV { get; set; }
    public float? RM { get; set; }
    public float? LE { get; set; }
    public float? CO { get; set; }
    public float? N2 { get; set; }
    public float? N5 { get; set; }
    public float? N6 { get; set; }
    public float? N7 { get; set; }
    public float? Puntaje { get; set; }
    public string? Moodle { get; set; }
    public string? DescripMoodle { get; set; }
    public string? msgPostulante { get; set; }
}
