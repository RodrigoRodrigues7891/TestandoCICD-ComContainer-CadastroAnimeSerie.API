using CadastroAnimeSerie.API.DTO.Request;
using CadastroAnimeSerie.API.Test.DataBuilders;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Net;

namespace CadastroAnimeSerie.API.Test;

public class Anime_POST : IClassFixture<CadastroAnimeSerieWebApplicationFactory>
{
    public CadastroAnimeSerieWebApplicationFactory app;

    public Anime_POST(CadastroAnimeSerieWebApplicationFactory app)
    {
        this.app = app;

    }

    [Fact]
    public async Task Cadastra_Anime()
    {
        //Arrange
        app.Context.Database.ExecuteSqlRaw("DELETE FROM Animes");
        app.Context.SaveChanges();
        var animeFaker = new AnimeDataBuilder().Generate();

        AnimeRequest animeRequest = new AnimeRequest(animeFaker.Nome, animeFaker.Sinopse, animeFaker.QuantidadeDeEpisodios, animeFaker.AnoDoLancamento, animeFaker.Diretor);

        using var client = app.CreateClient();
        //Act
        var response = await client.PostAsJsonAsync("/Anime", animeRequest);

        //Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    }
}
