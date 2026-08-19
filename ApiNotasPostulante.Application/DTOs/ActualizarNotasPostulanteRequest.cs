using System.Text.Json.Serialization;

namespace ApiNotasPostulante.Application.DTOs;

public class ActualizarNotasPostulanteRequest
{
    [JsonPropertyName("IDAlumno")]
    public string IDAlumno { get; set; } = null!;

    [JsonPropertyName("IDPerAcad")]
    public string IDPerAcad { get; set; } = null!;

    [JsonPropertyName("Ingresante")]
    public string? Ingresante { get; set; }

    [JsonPropertyName("Asistio")]
    public string? Asistio { get; set; }

    [JsonPropertyName("IDEscuela")]
    public string? IDEscuela { get; set; }

    [JsonPropertyName("IDEscuelaADM")]
    public string? IDEscuelaADM { get; set; }

    [JsonPropertyName("Nota1")]
    public decimal? Nota1 { get; set; }

    [JsonPropertyName("Nota2")]
    public decimal? Nota2 { get; set; }

    [JsonPropertyName("Nota3")]
    public decimal? Nota3 { get; set; }

    [JsonPropertyName("Nota4")]
    public decimal? Nota4 { get; set; }
}
