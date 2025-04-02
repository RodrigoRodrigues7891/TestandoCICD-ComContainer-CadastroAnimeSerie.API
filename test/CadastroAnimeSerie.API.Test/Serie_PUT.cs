using CadastroAnimeSerie.API.DTO.Request;
using CadastroAnimeSerie.API.Test.DataBuilders;
using CadastroAnimeSerie.Modelo;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Net;

namespace CadastroAnimeSerie.API.Test;

public class Serie_PUT : IClassFixture<CadastroAnimeSerieWebApplicationFactory>
{
    public CadastroAnimeSerieWebApplicationFactory app;

    public Serie_PUT(CadastroAnimeSerieWebApplicationFactory app)
    {
        this.app = app;


        var serieFaker = new SerieDataBuilder().Generate();
        var serie = new Serie(serieFaker.Nome, serieFaker.Sinopse, serieFaker.QuantidadeDeEpisodios, serieFaker.AnoDoLancamento, serieFaker.Diretor);
        app.Context.Database.ExecuteSqlRaw("DELETE FROM Series");
        app.Context.Series.Add(serie);
        app.Context.SaveChanges();
    }

    [Fact]
    public async Task Atualiza_Serie()
    {
        //Arrange
        var serieParaAlterar = app.Context.Series.FirstOrDefault();

        serieParaAlterar.Nome = serieParaAlterar.Nome + " Alterado";
        serieParaAlterar.Sinopse = serieParaAlterar.Sinopse + " Alterado";
        serieParaAlterar.Diretor = serieParaAlterar.Diretor + " Alterado";

        SerieRequestEdit serieRequestEdit = new SerieRequestEdit(serieParaAlterar.Id, serieParaAlterar.Nome, serieParaAlterar.Sinopse, serieParaAlterar.QuantidadeDeEpisodios, serieParaAlterar.AnoDoLancamento, serieParaAlterar.Diretor);
        using var client = app.CreateClient();
        //Act
        var response = await client.PutAsJsonAsync("/Serie", serieRequestEdit);

        //Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    }
}
