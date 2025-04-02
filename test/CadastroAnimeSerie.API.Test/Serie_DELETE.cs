using CadastroAnimeSerie.API.Test.DataBuilders;
using CadastroAnimeSerie.Modelo;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CadastroAnimeSerie.API.Test;

public class Serie_DELETE : IClassFixture<CadastroAnimeSerieWebApplicationFactory>
{
    public CadastroAnimeSerieWebApplicationFactory app;

    public Serie_DELETE(CadastroAnimeSerieWebApplicationFactory app)
    {
        this.app = app;

        var serieFaker = new AnimeDataBuilder().Generate();
        var serie = new Serie(serieFaker.Nome, serieFaker.Sinopse, serieFaker.QuantidadeDeEpisodios, serieFaker.AnoDoLancamento, serieFaker.Diretor);
        app.Context.Database.ExecuteSqlRaw("DELETE FROM Series");
        app.Context.Series.Add(serie);
        app.Context.SaveChanges();
    }

    [Fact]
    public async Task Deleta_Serie_PorId()
    {
        //Arrange
        var serieParaDeletar = app.Context.Series.FirstOrDefault();

        using var client = app.CreateClient();
        //Act
        var response = await client.DeleteAsync("/Serie/" + serieParaDeletar.Id);

        //Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}