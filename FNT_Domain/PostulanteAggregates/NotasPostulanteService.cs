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

    public float CalcularPuntaje(
        string idFacultad,
        string modalidad,
        float nota1,
        float nota2,
        float nota3,
        float nota4)
    {
        var esModalidadADM = modalidad == ModalidadADM;
        if (idFacultad != FacultadCienciasSalud)
        {
            return esModalidadADM ? nota3 : nota1;
        }

        var puntaje = esModalidadADM
            ? nota3 * 0.35f + nota4 * 0.35f + nota2 * 0.15f + nota1 * 0.15f
            : nota1 * 0.35f + nota2 * 0.35f + nota3 * 0.15f + nota4 * 0.15f;

        return MathF.Round(puntaje, 2, MidpointRounding.AwayFromZero);
    }

    public void ActualizarNotas(
        IEnumerable<TblPostulante> postulantes,
        string modalidad,
        float nota1,
        float nota2,
        float nota3,
        float nota4,
        float puntaje)
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
