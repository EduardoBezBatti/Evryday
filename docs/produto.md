# Produto

## Visão

Um app web simples e honesto para acompanhar hábitos: você marca o que fez **hoje** e o app mostra sua sequência, seu histórico e sua taxa de conclusão.

## Problema

Manter hábitos é difícil sem enxergar o próprio progresso. Muitos apps são carregados de recursos ou deixam "corrigir" o passado, o que tira o valor da sequência.

## Público

Uso pessoal (um único usuário) no MVP. Contas e login ficam para uma spec futura.

## Objetivos do projeto

1. Aprender e praticar Spec Driven Development do início ao fim.
2. Aprender na prática a stack escolhida (Angular, ASP.NET Core, EF Core).
3. Ter um projeto de portfólio: código limpo, testado, com CI e documentação.

## Princípios de produto

- **Honestidade:** só é possível marcar o dia de hoje. O histórico não pode ser reescrito.
- **Simplicidade:** um hábito é feito ou não feito (sim/não).
- **Flexibilidade na rotina:** frequência diária, em dias da semana ou X vezes por semana. A frequência pode mudar sem reescrever o passado.

## Escopo do MVP

| Spec | Funcionalidade | Resumo |
|---|---|---|
| 001 | Hábitos e marcação diária | Cadastrar, editar e excluir hábitos; tela "Hoje"; marcar e desmarcar o dia |
| 002 | Sequências | Sequência atual e maior sequência de cada hábito; tela de detalhe |
| 003 | Histórico em calendário | Calendário estilo heatmap das últimas 53 semanas, por hábito |
| 004 | Estatísticas | Taxa de conclusão por semana e por mês |

## Fora do escopo do MVP

- Login, contas e múltiplos usuários (planejado: spec 005)
- Hábitos com quantidade ou tempo (ex.: 8 copos, 30 minutos)
- Marcar ou desmarcar dias passados
- Arquivar hábitos (a exclusão é definitiva)
- Lembretes e notificações
- Reordenar hábitos manualmente
- Gráficos de evolução, exportação de dados, temas
- App mobile nativo (a versão web é responsiva)
- Deploy em produção (planejado: spec 006)

## Depois do MVP (ideias, sem compromisso)

- 005 — Login e contas (ASP.NET Core Identity)
- 006 — Deploy (container + nuvem)
- Hábitos quantitativos · Arquivar hábitos · Lembretes · PWA instalável · Gráficos de evolução

## Glossário

Use estes termos de forma consistente em specs, código e interface.

| Termo | Definição | No código |
|---|---|---|
| Hábito | Algo que o usuário quer fazer com regularidade | `Habito` |
| Frequência | Regra que diz quando o hábito é esperado: diária, dias da semana ou vezes por semana | `Frequencia`, `TipoFrequencia` |
| Agenda | Uma frequência com a data a partir da qual ela vale. Um hábito tem uma ou mais agendas ao longo do tempo | `AgendaHabito` |
| Frequência vigente | A frequência da agenda válida numa data | `FrequenciaEm(data)` |
| Data de início | Dia em que o hábito foi cadastrado; dias anteriores não contam | `DataInicio` |
| Dia previsto | Dia em que a frequência vigente espera o hábito. Diária e vezes por semana: todos os dias. Dias da semana: só os dias escolhidos | `PreveDia(data)` |
| Marcação | Registro de que o hábito foi feito num dia (no máximo uma por dia) | `Marcacao` |
| Marcação extra | Marcação feita num dia não previsto (só acontece em hábitos de dias da semana) | `EhExtra(data)` |
| Meta semanal | Quantidade de marcações esperadas por semana em hábitos de vezes por semana | `VezesPorSemana` |
| Semana | De segunda a domingo | `Semana` |
| Hoje | Data atual no fuso horário configurado no servidor | `IRelogio.Hoje()` |
| Sequência atual | Quantos dias previstos seguidos o hábito foi cumprido até hoje | `Sequencia.Atual` |
| Maior sequência | A maior sequência já alcançada | `Sequencia.Maior` |
| Taxa de conclusão | Percentual cumprido do que era esperado num período | `TaxaDeConclusao` |
