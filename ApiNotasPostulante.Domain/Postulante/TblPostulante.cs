namespace ApiNotasPostulante.Domain.Postulante;

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
    public decimal? RV { get; set; }
    public decimal? RM { get; set; }
    public decimal? LE { get; set; }
    public decimal? CO { get; set; }
    public decimal? N2 { get; set; }
    public decimal? N5 { get; set; }
    public decimal? N6 { get; set; }
    public decimal? N7 { get; set; }
    public decimal? Puntaje { get; set; }
    public string? Moodle { get; set; }
    public string? DescripMoodle { get; set; }
    public string? msgPostulante { get; set; }
}
