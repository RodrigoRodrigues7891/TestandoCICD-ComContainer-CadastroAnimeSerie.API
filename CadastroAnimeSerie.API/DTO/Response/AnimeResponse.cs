namespace CadastroAnimeSerie.API.DTO.Response;

public record AnimeResponse(int Id, string Nome, string Sinopse, int? AnoDoLancamento, int? QuantidadeDeEpisodios, string? Diretor);