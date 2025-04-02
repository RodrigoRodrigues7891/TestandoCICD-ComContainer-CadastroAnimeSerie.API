namespace CadastroAnimeSerie.API.DTO.Request;

public record AnimeRequest(string Nome, string Sinopse, int? QuantidadeDeEpisodios, int? AnoDoLancamento, string? Diretor);