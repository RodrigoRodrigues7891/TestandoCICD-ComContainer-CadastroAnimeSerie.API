using CadastroAnimeSerie.API.DTO.Request;
using CadastroAnimeSerie.API.Test.DataBuilders;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Net;

namespace CadastroAnimeSerie.API.Test;

public class Serie_POST : IClassFixture<CadastroAnimeSerieWebApplicationFactory>
{
    public CadastroAnimeSerieWebApplicationFactory app;

    public Serie_POST(CadastroAnimeSerieWebApplicationFactory app)
    {
        this.app = app;

    }

    [Fact]
    public async Task Cadastra_Serie()
    {
        //Arrange
        app.Context.Database.ExecuteSqlRaw("DELETE FROM Series");
        app.Context.SaveChanges();
        var serieFaker = new SerieDataBuilder().Generate();

        SerieRequest serieRequest = new SerieRequest(serieFaker.Nome, serieFaker.Sinopse, serieFaker.QuantidadeDeEpisodios, serieFaker.AnoDoLancamento, serieFaker.Diretor);

        using var client = app.CreateClient();
        //Act
        var response = await client.PostAsJsonAsync("/Serie", serieRequest);

        //Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    }
}
