using CadastroAnimeSerie.API.DTO.Request;
using CadastroAnimeSerie.API.Test.DataBuilders;
using CadastroAnimeSerie.Modelo;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Net;

namespace CadastroAnimeSerie.API.Test;

public class Anime_PUT : IClassFixture<CadastroAnimeSerieWebApplicationFactory>
{
    public CadastroAnimeSerieWebApplicationFactory app;

    public Anime_PUT(CadastroAnimeSerieWebApplicationFactory app)
    {
        this.app = app;


        var animeFaker = new AnimeDataBuilder().Generate();
        var anime = new Anime(animeFaker.Nome, animeFaker.Sinopse, animeFaker.QuantidadeDeEpisodios, animeFaker.AnoDoLancamento, animeFaker.Diretor);
        app.Context.Database.ExecuteSqlRaw("DELETE FROM Animes");
        app.Context.Animes.Add(anime);
        app.Context.SaveChanges();
    }

    [Fact]
    public async Task Atualiza_Anime()
    {
        //Arrange
        var animeParaAlterar = app.Context.Animes.FirstOrDefault();

        animeParaAlterar.Nome = animeParaAlterar.Nome + " Alterado";
        animeParaAlterar.Sinopse = animeParaAlterar.Sinopse + " Alterado";
        animeParaAlterar.Diretor = animeParaAlterar.Diretor + " Alterado";

        AnimeRequestEdit animeRequestEdit = new AnimeRequestEdit(animeParaAlterar.Id, animeParaAlterar.Nome, animeParaAlterar.Sinopse, animeParaAlterar.QuantidadeDeEpisodios, animeParaAlterar.AnoDoLancamento, animeParaAlterar.Diretor);
        using var client = app.CreateClient();
        //Act
        var response = await client.PutAsJsonAsync("/Anime", animeRequestEdit);

        //Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    }
}
