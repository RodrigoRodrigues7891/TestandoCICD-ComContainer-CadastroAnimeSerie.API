namespace CadastroAnimeSerie.API.DTO.Response;

public record SerieResponse(int Id, string Nome, string Sinopse, int? AnoDoLancamento, int? QuantidadeDeEpisodios, string? Diretor);