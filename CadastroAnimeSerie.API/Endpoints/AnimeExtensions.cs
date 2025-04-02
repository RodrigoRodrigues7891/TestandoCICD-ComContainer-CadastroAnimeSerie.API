using CadastroAnimeSerie.API.DTO.Request;
using CadastroAnimeSerie.API.DTO.Response;
using CadastroAnimeSerie.Dados.Banco;
using CadastroAnimeSerie.Modelo;
using Microsoft.AspNetCore.Mvc;

namespace CadastroAnimeSerie.API.Endpoints;

public static class AnimeExtensions
{
    public static void AddEndPointsAnime(this WebApplication app)
    {
        var groupBuilder = app.MapGroup("anime")
            .WithTags("Anime");
        #region Endpoint Anime
        groupBuilder.MapGet("", ([FromServices] DAL<Anime> dalAnime) =>
        {
            var listaDeAnimes = dalAnime.Listar();

            if (listaDeAnimes is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(EntityListToResponseList(listaDeAnimes));
        }).WithSummary("Retorna uma lista todos os animes cadastrados.");

        groupBuilder.MapGet("Nome/{nome}", ([FromServices] DAL<Anime> dalAnime, string nome) =>
        {
            var animeRecuperado = dalAnime.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));

            if (animeRecuperado is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(EntityToResponse(animeRecuperado));
        }).WithSummary("Retorna um anime cadastrado pelo nome informado.");

        groupBuilder.MapGet("{id}", ([FromServices] DAL<Anime> dalAnime, int id) =>
        {
            var animeRecuperado = dalAnime.RecuperarPor(a => a.Id == id);

            if (animeRecuperado is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(EntityToResponse(animeRecuperado));
        }).WithSummary("Retorna um anime por Id.");

        groupBuilder.MapGet("Paginado",
            async ([FromServices] DAL<Anime> dalAnime,
            [FromQuery] int pagina = 1, [FromQuery] int tamanhoPorPagina = 25) =>
            {
                var anime = await dalAnime.ListarPaginado(pagina, tamanhoPorPagina);
                if (anime is null) return Results.NotFound();
                return Results.Ok(EntityListToResponseList(anime));
            }).WithSummary("Retorna anime em lista paginada.");

        groupBuilder.MapPost("", async ([FromServices] DAL<Anime> dalAnime, [FromBody] AnimeRequest request) =>
        {
            var anime = new Anime(request.Nome, request.Sinopse, request.AnoDoLancamento, request.QuantidadeDeEpisodios, request.Diretor);

            await dalAnime.Adicionar(anime);
            return Results.Ok(EntityToResponse(anime));
        }).WithSummary("Insere um anime. *Nome e Sinópse válida");

        groupBuilder.MapPut("", async ([FromServices] DAL<Anime> dalAnime, AnimeRequestEdit requestEdit) =>
        {
            var animeAAtualizar = dalAnime.RecuperarPor(a => a.Id == requestEdit.Id);
            if (animeAAtualizar is null)
            {
                return Results.NotFound();
            }

            animeAAtualizar.Nome = requestEdit.Nome;
            animeAAtualizar.Sinopse = requestEdit.Sinopse;
            animeAAtualizar.Diretor = requestEdit.Diretor;

            await dalAnime.Atualizar(animeAAtualizar);
            return Results.Ok();
        }).WithSummary("Altera um anime a partir de um Id. *Nome e Sinópse válida");

        groupBuilder.MapDelete("{id}", async ([FromServices] DAL<Anime> dalAnime, int id) =>
        {
            var animeParaDeletar = dalAnime.RecuperarPor(a => a.Id == id);
            if (animeParaDeletar is null)
            {
                Results.NotFound();
            }

            await dalAnime.Deletar(animeParaDeletar);
            return Results.NoContent();
        }).WithSummary("Deleta um anime a partir de um Id.");
        #endregion
    }
    private static ICollection<AnimeResponse> EntityListToResponseList(IEnumerable<Anime> listaDeAnimes)
    {
        return listaDeAnimes.Select(a => EntityToResponse(a)).ToList();
    }

    private static AnimeResponse EntityToResponse(Anime anime)
    {
        return new AnimeResponse(anime.Id, anime.Nome, anime.Sinopse, anime.AnoDoLancamento, anime.QuantidadeDeEpisodios, anime.Diretor);
    }
}
