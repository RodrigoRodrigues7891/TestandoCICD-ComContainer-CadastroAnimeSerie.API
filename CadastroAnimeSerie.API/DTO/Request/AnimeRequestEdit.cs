namespace CadastroAnimeSerie.API.DTO.Request;

public record AnimeRequestEdit(int Id, string Nome, string Sinopse, int? QuantidadeDeEpisodios, int? AnoDoLancamento, string? Diretor);