using CadastroAnimeSerie.API.Test.DataBuilders;
using CadastroAnimeSerie.Modelo;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace CadastroAnimeSerie.API.Test;

public class Anime_GET : IClassFixture<CadastroAnimeSerieWebApplicationFactory>
{
    private readonly CadastroAnimeSerieWebApplicationFactory app;

    public Anime_GET(CadastroAnimeSerieWebApplicationFactory app)
    {
        this.app = app;

        GeraDadosParaTestes(100);

    }

    private void GeraDadosParaTestes(int quantidadeDeRegistros)
    {
        var animeDataBuilder = new AnimeDataBuilder();
        var listaDeAnimes = animeDataBuilder.Generate(quantidadeDeRegistros);
        app.Context.Database.ExecuteSqlRaw("DELETE FROM Animes");
        app.Context.Animes.AddRange(listaDeAnimes);
        app.Context.SaveChanges();
    }

    [Fact]
    public async Task Recupera_Lista_Anime()
    {
        //Arrange
        var quantidadeDeAnimes = app.Context.Animes.Count();

        using var client = app.CreateClient();
        //Act
        var response = await client.GetFromJsonAsync<ICollection<Anime>>("/Anime");

        //Assert
        Assert.NotNull(response);
        Assert.Equal(quantidadeDeAnimes, response.Count);
    }

    [Fact]
    public async Task Recupera_Anime_Por_Nome()
    {
        //Arrange
        var animeEncontrado = app.Context.Animes.FirstOrDefault();

        using var client = app.CreateClient();
        //Act
        var response = await client.GetFromJsonAsync<Anime>("/Anime/Nome/" + animeEncontrado!.Nome);

        //Assert
        Assert.NotNull(response);
        Assert.Equal(animeEncontrado.Nome, response.Nome);
    }

    [Fact]
    public async Task Recupera_Anime_Por_Id()
    {
        //Arrange
        var animeEncontrado = app.Context.Animes.FirstOrDefault();

        using var client = app.CreateClient();
        //Act
        var response = await client.GetFromJsonAsync<Anime>("/Anime/" + animeEncontrado.Id);

        //Assert
        Assert.NotNull(response);
        Assert.Equal(animeEncontrado.Nome, response.Nome);
    }

    [Fact]
    public async Task Recuperar_Animes_Na_Consulta_Paginada()
    {
        //Arrange
        int pagina = 1;
        int tamanhoPorPagina = 80;

        using var client = app.CreateClient();
        //Act
        var response = await client.GetFromJsonAsync<ICollection<Anime>>
            ($"/Anime/Paginado?pagina={pagina}&tamanhoPorPagina={tamanhoPorPagina}");
        //var reponseTodas = await client.GetFromJsonAsync<ICollection<Anime>>("/ofertas-viagem/todas");
        //Assert
        Assert.True(response != null);
        Assert.Equal(tamanhoPorPagina, response.Count());

    }

    [Fact]
    public async Task Recuperar_Animes_Na_Consulta_Paginada_Quantidade_Ultima_Pagina()
    {
        //Arrange
        int pagina = 2;
        int tamanhoPorPagina = 80;

        using var client = app.CreateClient();
        //Act
        var response = await client.GetFromJsonAsync<ICollection<Anime>>
            ($"/Anime/Paginado?pagina={pagina}&tamanhoPorPagina={tamanhoPorPagina}");

        //Assert
        Assert.True(response != null);
        Assert.Equal(20, response.Count());

    }

    [Fact]
    public async Task Recuperar_Animes_Na_Consulta_Paginada_Pagina_Com_Valor_Negativo()
    {
        //Arrange
        int pagina = -8;
        int tamanhoPorPagina = 80;

        using var client = app.CreateClient();
        //Act
        var response = await client.GetFromJsonAsync<ICollection<Anime>>
            ($"/Anime/Paginado?pagina={pagina}&tamanhoPorPagina={tamanhoPorPagina}");

        //Assert
        Assert.True(response != null);
        Assert.Equal(80, response.Count());

    }

    [Fact]
    public async Task Recuperar_Animes_Na_Consulta_Paginada_Pagina_Com_Valor_Zero()
    {
        //Arrange
        int pagina = 0;
        int tamanhoPorPagina = 80;

        using var client = app.CreateClient();
        //Act
        var response = await client.GetFromJsonAsync<ICollection<Anime>>
            ($"/Anime/Paginado?pagina={pagina}&tamanhoPorPagina={tamanhoPorPagina}");

        //Assert
        Assert.True(response != null);
        Assert.Equal(80, response.Count());

    }

    [Fact]
    public async Task Recuperar_Animes_Na_Consulta_Paginada_Tamanho_Por_Pagina_Com_Valor_Negativo()
    {
        //Arrange
        int pagina = 1;
        int tamanhoPorPagina = -50;

        using var client = app.CreateClient();
        //Act
        var response = await client.GetFromJsonAsync<ICollection<Anime>>
            ($"/Anime/Paginado?pagina={pagina}&tamanhoPorPagina={tamanhoPorPagina}");

        //Assert
        Assert.True(response != null);
        Assert.Equal(0, response.Count());

    }

    [Fact]
    public async Task Recuperar_Animes_Na_Consulta_Paginada_Tamanho_Por_Pagina_Com_Valor_Zero()
    {
        //Arrange
        int pagina = 1;
        int tamanhoPorPagina = 0;

        using var client = app.CreateClient();
        //Act
        var response = await client.GetFromJsonAsync<ICollection<Anime>>
            ($"/Anime/Paginado?pagina={pagina}&tamanhoPorPagina={tamanhoPorPagina}");

        //Assert
        Assert.True(response != null);
        Assert.Equal(0, response.Count());

    }
}
