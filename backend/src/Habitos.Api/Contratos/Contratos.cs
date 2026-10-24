namespace Habitos.Api.Contratos;

public record HabitoRequest(string Nome, string Descricao, string Cor, DateOnly DataInicio);
public record HabitoResponse(Guid Id, string Nome, string Descricao, string Cor, DateOnly DataInicio, string ResumoFrequencia, int SequenciaAtual, int MaiorSequencia);
public record HabitoUpdateRequest(string Nome, string Descricao, string Cor, FrequenciaRequest Frequencia);
public record FrequenciaRequest(int Tipo, int Dias, int? VezesPorSemana);
public record MarcacaoRequest(DateOnly Data);
public record HojeResponse(List<HabitoHojeDto> ParaHoje, List<HabitoHojeDto> Extras, double ProgressoSemanal, bool MetaAtingida);
public record HabitoHojeDto(Guid Id, string Nome, string Cor, bool Marcado, bool EhExtra, int SequenciaAtual);
