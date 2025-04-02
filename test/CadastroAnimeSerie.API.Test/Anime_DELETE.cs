using CadastroAnimeSerie.API.Test.DataBuilders;
using CadastroAnimeSerie.Modelo;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CadastroAnimeSerie.API.Test;

public class Anime_DELETE : IClassFixture<CadastroAnimeSerieWebApplicationFactory>
{
    public CadastroAnimeSerieWebApplicationFactory app;

    public Anime_DELETE(CadastroAnimeSerieWebApplicationFactory app)
    {
        this.app = app;

        var animeFaker = new AnimeDataBuilder().Generate();
        var anime = new Anime(animeFaker.Nome, animeFaker.Sinopse, animeFaker.QuantidadeDeEpisodios, animeFaker.AnoDoLancamento, animeFaker.Diretor);
        app.Context.Database.ExecuteSqlRaw("DELETE FROM Animes");
        app.Context.Animes.Add(anime);
        app.Context.SaveChanges();
    }

    [Fact]
    public async Task Deleta_Anime_PorId()
    {
        //Arrange
        var animeParaDeletar = app.Context.Animes.FirstOrDefault();

        using var client = app.CreateClient();
        //Act
        var response = await client.DeleteAsync("/Anime/" + animeParaDeletar.Id);

        //Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}