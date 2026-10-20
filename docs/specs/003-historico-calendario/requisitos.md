# Spec 003 — Histórico em calendário

> **Status:** Rascunho. O design e as tarefas serão escritos quando a spec 002 estiver concluída.
> **Depende de:** 001, 002 (tela de detalhe do hábito)
> **Última atualização:** 2026-10-20

## Contexto

Ver o histórico de um hábito num calendário estilo "heatmap do GitHub" revela padrões (ex.: "sempre falho às sextas") que números sozinhos escondem.

## Fora de escopo

- Editar marcações pelo calendário (D07: só hoje pode ser marcado)
- Calendário combinando todos os hábitos
- Navegar para antes das últimas 53 semanas
- Indicar no calendário as datas em que a frequência mudou

## Requisitos

### Requisito 1 — Calendário do hábito

**História:** Como usuário, quero ver o histórico de um hábito num calendário, para identificar padrões e falhas.

**Critérios de aceite**

1. O sistema DEVE exibir na tela de detalhe do hábito um calendário com as últimas 53 semanas, terminando na semana atual. Cada coluna é uma semana e cada linha é um dia, de segunda a domingo.
2. O sistema DEVE classificar cada dia em exatamente um estado, usando a frequência vigente naquele dia e aplicando as regras nesta ordem:

   | Ordem | Condição | Estado |
   |---|---|---|
   | 1 | Antes da data de início ou depois de hoje | Fora do período |
   | 2 | Marcado e previsto | Cumprido |
   | 3 | Marcado e não previsto | Extra |
   | 4 | Hoje, não marcado e previsto | Pendente |
   | 5 | Passado, não marcado, previsto, em hábito diário ou de dias da semana | Perdido |
   | 6 | Qualquer outro caso (dia não previsto, ou dia de hábito de vezes por semana sem marcação) | Livre |

3. Cada estado DEVE ter aparência distinta e aparecer numa legenda; o estado "Cumprido" DEVE usar a cor do hábito.
4. QUANDO o usuário passar o mouse ou tocar num dia, o sistema DEVE exibir a data por extenso e o estado.
5. Cada dia DEVE ter rótulo acessível com a data e o estado (ex.: "quarta-feira, 21 de outubro: cumprido").
6. O sistema DEVE exibir o nome abreviado de cada mês acima da coluna em que o mês começa.
7. ENQUANTO a tela for estreita demais para as 53 semanas, o calendário DEVE rolar na horizontal e abrir mostrando a semana atual.
8. O calendário DEVE ser somente leitura.

### Requisito 2 — Resumo do período

**História:** Como usuário, quero um número que resuma o calendário, para ter noção rápida do meu histórico.

**Critérios de aceite**

1. O sistema DEVE exibir, acima do calendário, o total de marcações no período mostrado (ex.: "142 marcações nas últimas 53 semanas").

## Exemplos (viram casos de teste)

Semana de referência: segunda 19/10/2026 a domingo 25/10/2026. Hábitos criados na segunda 19/10.

| # | Frequência | Marcações | Hoje | Estados de seg a dom |
|---|---|---|---|---|
| 1 | Seg, Qua, Sex | seg, ter | qui 22 | Cumprido, Extra, Perdido, Livre, Fora, Fora, Fora |
| 2 | 3x por semana | seg | qua 21 | Cumprido, Livre, Pendente, Fora, Fora, Fora, Fora |
| 3 | Diária | seg, qua | qui 22, marcado | Cumprido, Perdido, Cumprido, Cumprido, Fora, Fora, Fora |

## Dúvidas em aberto

- [ ] Cores exatas de cada estado e contraste mínimo: definir no design, junto com o tema do Angular Material.
