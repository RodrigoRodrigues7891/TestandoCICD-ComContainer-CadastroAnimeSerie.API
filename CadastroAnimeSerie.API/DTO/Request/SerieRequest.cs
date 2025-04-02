namespace CadastroAnimeSerie.API.DTO.Request;

public record SerieRequest(string Nome, string Sinopse, int? QuantidadeDeEpisodios, int? AnoDoLancamento, string? Diretor);