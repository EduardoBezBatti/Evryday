export interface HabitoRequest {
  nome: string;
  descricao?: string;
  cor: string;
  dataInicio: string;
}

export interface HabitoUpdateRequest {
  nome: string;
  descricao?: string;
  cor: string;
  frequencia: FrequenciaRequest;
}

export interface FrequenciaRequest {
  tipo: number;
  dias: number;
  vezesPorSemana?: number;
}

export interface Habito {
  id: string;
  nome: string;
  descricao?: string;
  cor: string;
  dataInicio: string;
  resumoFrequencia: string;
  sequenciaAtual: number;
  maiorSequencia: number;
}

export interface HabitoHoje {
  id: string;
  nome: string;
  cor: string;
  marcado: boolean;
  ehExtra: boolean;
  sequenciaAtual: number;
}

export interface HojeResponse {
  paraHoje: HabitoHoje[];
  extras: HabitoHoje[];
  progressoSemanal: number;
  metaAtingida: boolean;
}
