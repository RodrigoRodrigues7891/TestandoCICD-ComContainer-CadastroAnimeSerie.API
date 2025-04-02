using CadastroAnimeSerie.API.DTO.Request;
using CadastroAnimeSerie.API.DTO.Response;
using CadastroAnimeSerie.Dados.Banco;
using CadastroAnimeSerie.Modelo;
using Microsoft.AspNetCore.Mvc;

namespace CadastroAnimeSerie.API.Endpoints;

public static class SerieExtensions
{
    public static void AddEndPointsSerie(this WebApplication app)
    {
        var groupBuilder = app.MapGroup("serie")
            .WithTags("Serie");
        #region Endpoint Anime
        groupBuilder.MapGet("", ([FromServices] DAL<Serie> dalSerie) =>
        {
            var listaDeSeries = dalSerie.Listar();

            if (listaDeSeries is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(EntityListToResponseList(listaDeSeries));
        }).WithSummary("Retorna uma lista todos os series cadastrados.");

        groupBuilder.MapGet("Nome/{nome}", ([FromServices] DAL<Serie> dalSerie, string nome) =>
        {
            var serieRecuperada = dalSerie.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));

            if (serieRecuperada is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(EntityToResponse(serieRecuperada));
        }).WithSummary("Retorna uma serie cadastrado pelo nome informado.");

        groupBuilder.MapGet("{id}", ([FromServices] DAL<Serie> dalSerie, int id) =>
        {
            var serieRecuperada = dalSerie.RecuperarPor(a => a.Id == id);

            if (serieRecuperada is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(EntityToResponse(serieRecuperada));
        }).WithSummary("Retorna uma serie por Id.");

        groupBuilder.MapGet("Paginado",
            async ([FromServices] DAL<Serie> dalSerie,
            [FromQuery] int pagina = 1, [FromQuery] int tamanhoPorPagina = 25) =>
            {
                var serie = await dalSerie.ListarPaginado(pagina, tamanhoPorPagina);
                if (serie is null) return Results.NotFound();
                return Results.Ok(EntityListToResponseList(serie));
            }).WithSummary("Retorna series em lista paginada.");

        groupBuilder.MapPost("", async ([FromServices] DAL<Serie> dalSerie, [FromBody] SerieRequest request) =>
        {
            var serie = new Serie(request.Nome, request.Sinopse, request.AnoDoLancamento, request.QuantidadeDeEpisodios, request.Diretor);

            await dalSerie.Adicionar(serie);
            return Results.Ok(EntityToResponse(serie));
        }).WithSummary("Insere uma serie. *Nome e Sinópse válida");

        groupBuilder.MapPut("", async ([FromServices] DAL<Serie> dalSerie, SerieRequestEdit requestEdit) =>
        {
            var serieAAtualizar = dalSerie.RecuperarPor(a => a.Id == requestEdit.Id);
            if (serieAAtualizar is null)
            {
                return Results.NotFound();
            }

            serieAAtualizar.Nome = requestEdit.Nome;
            serieAAtualizar.Sinopse = requestEdit.Sinopse;
            serieAAtualizar.Diretor = requestEdit.Diretor;

            await dalSerie.Atualizar(serieAAtualizar);
            return Results.Ok();
        }).WithSummary("Altera uma serie a partir de um Id. *Nome e Sinópse válida");

        groupBuilder.MapDelete("{id}", async ([FromServices] DAL<Serie> dalSerie, int id) =>
        {
            var serieParaDeletar = dalSerie.RecuperarPor(a => a.Id == id);
            if (serieParaDeletar is null)
            {
                Results.NotFound();
            }

            await dalSerie.Deletar(serieParaDeletar);
            return Results.NoContent();
        }).WithSummary("Deleta uma serie a partir de um Id.");
        #endregion
    }
    private static ICollection<SerieResponse> EntityListToResponseList(IEnumerable<Serie> listaDeSeries)
    {
        return listaDeSeries.Select(a => EntityToResponse(a)).ToList();
    }

    private static SerieResponse EntityToResponse(Serie serie)
    {
        return new SerieResponse(serie.Id, serie.Nome, serie.Sinopse, serie.AnoDoLancamento, serie.QuantidadeDeEpisodios, serie.Diretor);
    }
}
