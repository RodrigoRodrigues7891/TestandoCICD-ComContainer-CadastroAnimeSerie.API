namespace CadastroAnimeSerie.API.DTO.Request;

public record SerieRequestEdit(int Id, string Nome, string Sinopse, int? QuantidadeDeEpisodios, int? AnoDoLancamento, string? Diretor);