namespace FNT_Domain.PostulanteAggregates;

public class TblConfigSyncNotasUC
{
    public string Variable { get; set; } = null!;
    public string? Valor { get; set; }
    public string? Detalle { get; set; }
    public DateTime? FechaModificacion { get; set; }
    public bool? Flg { get; set; }
}
