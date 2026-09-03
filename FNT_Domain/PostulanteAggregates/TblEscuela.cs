namespace FNT_Domain.PostulanteAggregates;

public class TblEscuela
{
    public string IDEscuela { get; set; } = null!;
    public string IDDependencia { get; set; } = null!;
    public string? IDFacultad { get; set; }
    public string? IDTipoEsc { get; set; }
    public string? Nombre { get; set; }
    public string? Activo { get; set; }
}
