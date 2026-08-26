using FNT_Application.DTOs;
using FNT_Application.Features;
using FNT_Application.Validators;
using FNT_Domain;
using FNT_Domain.PostulanteAggregates;
using FNT_Domain.PostulanteAggregates.Interface;
using Xunit;

namespace FNT_NotasPostulante_Tests;

public class ActualizarNotasPostulanteHandlerTests
{
    [Fact]
    public async Task RechazaLaSolicitudCuandoFaltaUnaNota()
    {
        var repository = CrearRepository();
        var request = CrearRequest();
        request.Nota4 = null;

        var response = await CrearHandler(repository).EjecutarAsync(request, CancellationToken.None);

        Assert.False(response.IsSuccess);
        Assert.Equal("Notas incompletas", response.Message);
        Assert.Equal(0, repository.CantidadGuardados);
    }

    [Fact]
    public async Task RechazaLaSolicitudCuandoIDAlumnoEstaVacio()
    {
        var repository = CrearRepository();
        var request = CrearRequest();
        request.IDAlumno = " ";

        var response = await CrearHandler(repository).EjecutarAsync(request, CancellationToken.None);

        Assert.False(response.IsSuccess);
        Assert.Equal("IDAlumno requerido", response.Message);
    }

    [Fact]
    public async Task RechazaIngresanteConEspacios()
    {
        var repository = CrearRepository();
        var request = CrearRequest();
        request.Ingresante = " 1 ";

        var response = await CrearHandler(repository).EjecutarAsync(request, CancellationToken.None);

        Assert.False(response.IsSuccess);
        Assert.Equal("Ingresante debe ser 1", response.Message);
    }

    [Fact]
    public async Task RechazaAsistioConEspacios()
    {
        var repository = CrearRepository();
        var request = CrearRequest();
        request.Asistio = " 1 ";

        var response = await CrearHandler(repository).EjecutarAsync(request, CancellationToken.None);

        Assert.False(response.IsSuccess);
        Assert.Equal("No rindió evaluación momento 1", response.Message);
    }

    [Fact]
    public async Task RechazaEscuelaInexistente()
    {
        var repository = CrearRepository();
        repository.Escuelas.Clear();

        var response = await CrearHandler(repository).EjecutarAsync(CrearRequest(), CancellationToken.None);

        Assert.False(response.IsSuccess);
        Assert.Equal("No existe la escuela indicada", response.Message);
        Assert.Equal(0, repository.CantidadGuardados);
    }

    [Fact]
    public async Task RechazaMedicinaCuandoLaEscuelaExiste()
    {
        var repository = CrearRepository();
        repository.Escuelas.Add(new TblEscuela
        {
            IDEscuela = "502",
            IDDependencia = "D001",
            IDFacultad = "CS"
        });
        var request = CrearRequest();
        request.IDEscuela = "502";

        var response = await CrearHandler(repository).EjecutarAsync(request, CancellationToken.None);

        Assert.False(response.IsSuccess);
        Assert.Equal("Es Medicina", response.Message);
        Assert.Equal(0, repository.CantidadGuardados);
    }

    [Fact]
    public async Task InformaEscuelaInexistenteAntesDeEvaluarMedicina()
    {
        var repository = CrearRepository();
        var request = CrearRequest();
        request.IDEscuela = "502";

        var response = await CrearHandler(repository).EjecutarAsync(request, CancellationToken.None);

        Assert.False(response.IsSuccess);
        Assert.Equal("No existe la escuela indicada", response.Message);
    }

    [Fact]
    public async Task CalculaPuntajeAdmParaCienciasDeLaSalud()
    {
        var repository = CrearRepository("CS");

        var response = await CrearHandler(repository).EjecutarAsync(CrearRequest(), CancellationToken.None);

        Assert.True(response.IsSuccess);
        Assert.Equal(13.80m, repository.Postulantes[0].Puntaje);
        Assert.Equal(10m, repository.Postulantes[0].RV);
        Assert.Equal(12m, repository.Postulantes[0].RM);
        Assert.Equal(14m, repository.Postulantes[0].LE);
        Assert.Equal(16m, repository.Postulantes[0].CO);
    }

    [Fact]
    public async Task CalculaPuntajeAdvParaCienciasDeLaSalud()
    {
        var repository = CrearRepository("CS");
        var request = CrearRequest();
        request.IDEscuelaADM = "ADV";

        var response = await CrearHandler(repository).EjecutarAsync(request, CancellationToken.None);

        Assert.True(response.IsSuccess);
        Assert.Equal(12.20m, repository.Postulantes[0].Puntaje);
        Assert.Equal(10m, repository.Postulantes[0].N2);
        Assert.Equal(12m, repository.Postulantes[0].N5);
        Assert.Equal(14m, repository.Postulantes[0].N6);
        Assert.Equal(16m, repository.Postulantes[0].N7);
    }

    [Fact]
    public async Task UsaRedondeoComercialParaCienciasDeLaSalud()
    {
        var repository = CrearRepository("CS");
        var request = CrearRequest();
        request.Nota1 = 10.01m;
        request.Nota2 = 10m;
        request.Nota3 = 10.01m;
        request.Nota4 = 10m;

        var response = await CrearHandler(repository).EjecutarAsync(request, CancellationToken.None);

        Assert.True(response.IsSuccess);
        Assert.Equal(10.01m, repository.Postulantes[0].Puntaje);
    }

    [Theory]
    [InlineData("ADM", 14)]
    [InlineData("ADV", 10)]
    [InlineData("ADG", 10)]
    public async Task UsaNotaBaseFueraDeCienciasDeLaSalud(string modalidad, int puntajeEsperado)
    {
        var repository = CrearRepository("FI");
        var request = CrearRequest();
        request.IDEscuelaADM = modalidad;

        var response = await CrearHandler(repository).EjecutarAsync(request, CancellationToken.None);

        Assert.True(response.IsSuccess);
        Assert.Equal((decimal)puntajeEsperado, repository.Postulantes[0].Puntaje);
    }

    [Fact]
    public async Task ActualizaTodasLasFilasCoincidentes()
    {
        var repository = CrearRepository("CS");
        repository.Postulantes.Add(CrearPostulante(idDependencia: "D002"));

        var response = await CrearHandler(repository).EjecutarAsync(CrearRequest(), CancellationToken.None);

        Assert.True(response.IsSuccess);
        Assert.Equal(2, repository.Postulantes.Count(p => p.Puntaje == 13.80m));
        Assert.Equal(1, repository.CantidadGuardados);
    }

    private static ActualizarNotasPostulanteHandler CrearHandler(FakeRepository repository)
    {
        return new ActualizarNotasPostulanteHandler(
            new FakeUnitOfWork(repository),
            new ActualizarNotasPostulanteValidator(),
            new NotasPostulanteService());
    }

    private static FakeRepository CrearRepository(string idFacultad = "FI")
    {
        var repository = new FakeRepository();
        repository.Postulantes.Add(CrearPostulante());
        repository.Escuelas.Add(new TblEscuela
        {
            IDEscuela = "501",
            IDDependencia = "D001",
            IDFacultad = idFacultad
        });
        return repository;
    }

    private static TblPostulante CrearPostulante(string idDependencia = "D001")
    {
        return new TblPostulante
        {
            IDAlumno = "A001",
            IDDependencia = idDependencia,
            IDPerAcad = "202601",
            IDExamen = new DateTime(2026, 1, 1),
            IDEscuelaADM = "ADM",
            Ingresante = "1",
            Asistio = "1"
        };
    }

    private static ActualizarNotasPostulanteRequest CrearRequest()
    {
        return new ActualizarNotasPostulanteRequest
        {
            IDAlumno = "A001",
            IDPerAcad = "202601",
            Ingresante = "1",
            Asistio = "1",
            IDEscuela = "501",
            IDEscuelaADM = "ADM",
            Nota1 = 10m,
            Nota2 = 12m,
            Nota3 = 14m,
            Nota4 = 16m
        };
    }

    private sealed class FakeRepository : IActualizarNotasPostulanteRepository
    {
        public List<TblPostulante> Postulantes { get; } = [];
        public List<TblEscuela> Escuelas { get; } = [];
        public int CantidadGuardados { get; private set; }

        public Task<List<TblPostulante>> ObtenerPostulantesPorAlumnoAsync(
            string idAlumno,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(Postulantes.Where(p => p.IDAlumno == idAlumno).ToList());
        }

        public Task<TblEscuela?> ObtenerEscuelaAsync(
            string idEscuela,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(Escuelas.FirstOrDefault(e => e.IDEscuela == idEscuela));
        }

        public Task<Dictionary<string, string>> ObtenerConfiguracionActivaAsync(
            CancellationToken cancellationToken)
        {
            return Task.FromResult<Dictionary<string, string>>([]);
        }

        public void RegistrarGuardado()
        {
            CantidadGuardados++;
        }
    }

    private sealed class FakeUnitOfWork : IUnitOfWork
    {
        private readonly FakeRepository _repository;

        public FakeUnitOfWork(FakeRepository repository)
        {
            _repository = repository;
            PostulanteNotas = repository;
        }

        public IActualizarNotasPostulanteRepository PostulanteNotas { get; }

        public Task<int> GuardarCambiosAsync(CancellationToken cancellationToken)
        {
            _repository.RegistrarGuardado();
            return Task.FromResult(1);
        }
    }
}
