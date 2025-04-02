using CadastroAnimeSerie.API.Test.DataBuilders;
using CadastroAnimeSerie.Modelo;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace CadastroAnimeSerie.API.Test;

public class Serie_GET : IClassFixture<CadastroAnimeSerieWebApplicationFactory>
{
    private readonly CadastroAnimeSerieWebApplicationFactory app;

    public Serie_GET(CadastroAnimeSerieWebApplicationFactory app)
    {
        this.app = app;

        GeraDadosParaTestes(100);

    }

    private void GeraDadosParaTestes(int quantidadeDeRegistros)
    {
        var serieDataBuilder = new SerieDataBuilder();
        var listaDeSeries = serieDataBuilder.Generate(quantidadeDeRegistros);
        app.Context.Database.ExecuteSqlRaw("DELETE FROM Series");
        app.Context.Series.AddRange(listaDeSeries);
        app.Context.SaveChanges();
    }

    [Fact]
    public async Task Recupera_Lista_Serie()
    {
        //Arrange
        var quantidadeDeSeries = app.Context.Series.Count();

        using var client = app.CreateClient();
        //Act
        var response = await client.GetFromJsonAsync<ICollection<Serie>>("/Serie");

        //Assert
        Assert.NotNull(response);
        Assert.Equal(quantidadeDeSeries, response.Count);
    }

    [Fact]
    public async Task Recupera_Serie_Por_Nome()
    {
        //Arrange
        var serieEncontrada = app.Context.Series.FirstOrDefault();

        using var client = app.CreateClient();
        //Act
        var response = await client.GetFromJsonAsync<Serie>("/Serie/Nome/" + serieEncontrada!.Nome);

        //Assert
        Assert.NotNull(response);
        Assert.Equal(serieEncontrada.Nome, response.Nome);
    }

    [Fact]
    public async Task Recupera_Serie_Por_Id()
    {
        //Arrange
        var serieEncontrada = app.Context.Series.FirstOrDefault();

        using var client = app.CreateClient();
        //Act
        var response = await client.GetFromJsonAsync<Serie>("/Serie/" + serieEncontrada.Id);

        //Assert
        Assert.NotNull(response);
        Assert.Equal(serieEncontrada.Nome, response.Nome);
    }

    [Fact]
    public async Task Recuperar_Series_Na_Consulta_Paginada()
    {
        //Arrange
        int pagina = 1;
        int tamanhoPorPagina = 80;

        using var client = app.CreateClient();
        //Act
        var response = await client.GetFromJsonAsync<ICollection<Serie>>
            ($"/Serie/Paginado?pagina={pagina}&tamanhoPorPagina={tamanhoPorPagina}");
        //var reponseTodas = await client.GetFromJsonAsync<ICollection<Serie>>("/ofertas-viagem/todas");
        //Assert
        Assert.True(response != null);
        Assert.Equal(tamanhoPorPagina, response.Count());

    }

    [Fact]
    public async Task Recuperar_Series_Na_Consulta_Paginada_Quantidade_Ultima_Pagina()
    {
        //Arrange
        int pagina = 2;
        int tamanhoPorPagina = 80;

        using var client = app.CreateClient();
        //Act
        var response = await client.GetFromJsonAsync<ICollection<Serie>>
            ($"/Serie/Paginado?pagina={pagina}&tamanhoPorPagina={tamanhoPorPagina}");

        //Assert
        Assert.True(response != null);
        Assert.Equal(20, response.Count());

    }

    [Fact]
    public async Task Recuperar_Series_Na_Consulta_Paginada_Pagina_Com_Valor_Negativo()
    {
        //Arrange
        int pagina = -8;
        int tamanhoPorPagina = 80;

        using var client = app.CreateClient();
        //Act
        var response = await client.GetFromJsonAsync<ICollection<Serie>>
            ($"/Serie/Paginado?pagina={pagina}&tamanhoPorPagina={tamanhoPorPagina}");

        //Assert
        Assert.True(response != null);
        Assert.Equal(80, response.Count());

    }

    [Fact]
    public async Task Recuperar_Series_Na_Consulta_Paginada_Pagina_Com_Valor_Zero()
    {
        //Arrange
        int pagina = 0;
        int tamanhoPorPagina = 80;

        using var client = app.CreateClient();
        //Act
        var response = await client.GetFromJsonAsync<ICollection<Serie>>
            ($"/Serie/Paginado?pagina={pagina}&tamanhoPorPagina={tamanhoPorPagina}");

        //Assert
        Assert.True(response != null);
        Assert.Equal(80, response.Count());

    }

    [Fact]
    public async Task Recuperar_Series_Na_Consulta_Paginada_Tamanho_Por_Pagina_Com_Valor_Negativo()
    {
        //Arrange
        int pagina = 1;
        int tamanhoPorPagina = -50;

        using var client = app.CreateClient();
        //Act
        var response = await client.GetFromJsonAsync<ICollection<Serie>>
            ($"/Serie/Paginado?pagina={pagina}&tamanhoPorPagina={tamanhoPorPagina}");

        //Assert
        Assert.True(response != null);
        Assert.Equal(0, response.Count());

    }

    [Fact]
    public async Task Recuperar_Series_Na_Consulta_Paginada_Tamanho_Por_Pagina_Com_Valor_Zero()
    {
        //Arrange
        int pagina = 1;
        int tamanhoPorPagina = 0;

        using var client = app.CreateClient();
        //Act
        var response = await client.GetFromJsonAsync<ICollection<Serie>>
            ($"/Serie/Paginado?pagina={pagina}&tamanhoPorPagina={tamanhoPorPagina}");

        //Assert
        Assert.True(response != null);
        Assert.Equal(0, response.Count());

    }
}
