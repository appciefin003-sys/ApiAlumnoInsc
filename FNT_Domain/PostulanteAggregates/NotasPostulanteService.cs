namespace FNT_Domain.PostulanteAggregates;

public class NotasPostulanteService
{
    private const string FacultadCienciasSalud = "CS";
    private const string EscuelaMedicina = "502";
    private const string ModalidadADM = "ADM";
    private const string ModalidadADV = "ADV";
    private const string ModalidadADG = "ADG";

    public bool EsMedicina(string idEscuela)
    {
        return idEscuela == EscuelaMedicina;
    }

    public bool EsModalidadSoportada(string modalidad)
    {
        return modalidad is ModalidadADM or ModalidadADV or ModalidadADG;
    }

    public decimal CalcularPuntaje(
        string idFacultad,
        string modalidad,
        decimal nota1,
        decimal nota2,
        decimal nota3,
        decimal nota4)
    {
        var esModalidadADM = modalidad == ModalidadADM;
        if (idFacultad != FacultadCienciasSalud)
        {
            return esModalidadADM ? nota3 : nota1;
        }

        var puntaje = esModalidadADM
            ? nota3 * 0.35m + nota4 * 0.35m + nota2 * 0.15m + nota1 * 0.15m
            : nota1 * 0.35m + nota2 * 0.35m + nota3 * 0.15m + nota4 * 0.15m;

        return Math.Round(puntaje, 2, MidpointRounding.AwayFromZero);
    }

    public void ActualizarNotas(
        IEnumerable<TblPostulante> postulantes,
        string modalidad,
        decimal nota1,
        decimal nota2,
        decimal nota3,
        decimal nota4,
        decimal puntaje)
    {
        foreach (var postulante in postulantes)
        {
            if (modalidad == ModalidadADM)
            {
                postulante.RV = nota1;
                postulante.RM = nota2;
                postulante.LE = nota3;
                postulante.CO = nota4;
            }
            else
            {
                postulante.N2 = nota1;
                postulante.N5 = nota2;
                postulante.N6 = nota3;
                postulante.N7 = nota4;
            }

            postulante.Puntaje = puntaje;
        }
    }
}
