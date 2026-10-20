# Spec 004 — Estatísticas

> **Status:** Rascunho. O design e as tarefas serão escritos quando a spec 003 estiver concluída.
> **Depende de:** 001
> **Última atualização:** 2026-10-20

## Contexto

A taxa de conclusão responde à pergunta "estou cumprindo o que me propus?", por semana e por mês, para cada hábito e no geral.

## Fora de escopo

- Gráficos de evolução ao longo do tempo
- Comparação entre períodos
- Exportação de dados

## Requisitos

### Requisito 1 — Tela e períodos

**História:** Como usuário, quero ver minha taxa de conclusão por semana ou por mês, para avaliar minha consistência.

**Critérios de aceite**

1. O sistema DEVE oferecer a tela "Estatísticas", acessível pela navegação principal.
2. O sistema DEVE permitir escolher o tipo de período: semana (segunda a domingo) ou mês do calendário. O padrão é a semana atual.
3. O sistema DEVE permitir navegar para o período anterior e para o seguinte, sem ir além do período atual.
4. SE nenhum hábito existir no período escolhido, ENTÃO o sistema DEVE exibir uma mensagem de período sem dados.

### Requisito 2 — Dias avaliados

**História:** Como usuário, quero que um dia ainda em andamento não derrube minha taxa, para que o número seja justo.

**Critérios de aceite**

1. O sistema DEVE avaliar, para cada hábito, apenas os dias do período a partir da data de início do hábito e até ontem.
2. QUANDO hoje estiver no período e o hábito estiver marcado hoje, o sistema DEVE incluir hoje nos dias avaliados.
3. O sistema NÃO DEVE incluir dias futuros, nem hoje sem marcação.

### Requisito 3 — Taxa de hábitos diários e de dias da semana

**História:** Como usuário, quero saber que percentual dos dias previstos eu cumpri.

**Critérios de aceite**

1. O sistema DEVE calcular a taxa como dias previstos marcados ÷ dias previstos avaliados, em percentual inteiro (arredondado para o inteiro mais próximo).
2. O sistema NÃO DEVE contar marcações extras na taxa; DEVE exibi-las como contagem separada (ex.: "+2 extras").
3. SE não houver dia previsto avaliado no período, ENTÃO o sistema DEVE exibir "—" no lugar da taxa e desconsiderar o hábito na taxa geral.

### Requisito 4 — Taxa de hábitos de vezes por semana

**História:** Como usuário, quero saber em quantas semanas eu bati a meta.

**Critérios de aceite**

1. O sistema DEVE considerar uma semana cumprida quando o número de marcações nela for maior ou igual à meta semanal vigente no último dia avaliado da semana.
2. ENQUANTO a semana atual não tiver a meta atingida, o sistema DEVE tratá-la como "em andamento" e não incluí-la no cálculo.
3. Na visão semanal, o sistema DEVE exibir o progresso (ex.: "2 de 3") e o estado da semana: cumprida, em andamento ou não cumprida.
4. Na visão mensal, o sistema DEVE calcular a taxa como semanas cumpridas ÷ semanas avaliadas, atribuindo cada semana ao mês que contém a sua quinta-feira (regra da ISO 8601).
5. SE a semana em que o hábito foi criado não tiver a meta atingida, ENTÃO o sistema DEVE desconsiderá-la (ela não conta como não cumprida).

### Requisito 5 — Visão geral

**História:** Como usuário, quero um resumo de todos os hábitos, para enxergar o período de uma vez.

**Critérios de aceite**

1. O sistema DEVE exibir a taxa geral do período como média simples das taxas dos hábitos que têm taxa definida.
2. O sistema DEVE listar os hábitos com sua taxa e uma barra de progresso, ordenados da maior para a menor taxa.

## Exemplos (viram casos de teste)

Semana de referência: segunda 19/10/2026 a domingo 25/10/2026. Hábitos criados antes dessa semana, salvo indicação.

| # | Frequência | Marcações | Hoje | Resultado | Por quê |
|---|---|---|---|---|---|
| 1 | Diária | seg, ter, qui | qui 22, marcado | 75% | Avaliados seg a qui (hoje marcado): 3 de 4 |
| 2 | Seg, Qua, Sex | seg, ter (extra), qua | sex 23, sem marcação | 100%, "+1 extra" | Previstos avaliados: seg e qua; sexta em andamento |
| 3 | 3x por semana | seg, qua | qui 22 | "2 de 3", em andamento, "—" | A semana atual ainda não bateu a meta |
| 4 | 3x por semana | — | consultando outubro de 2026 em novembro | 3 de 5 semanas = 60% | Semanas com as quintas 1, 8, 15, 22 e 29/10; 3 cumpridas |

## Dúvidas em aberto

- [ ] Hábito que mudou de **tipo** de frequência dentro do período (ex.: diária → 3x por semana). Proposta: cada semana é avaliada pela frequência vigente no seu último dia avaliado; semanas "por dia" contribuem com dias e semanas "por meta" com 1 oportunidade; taxa = cumpridos ÷ oportunidades.
- [ ] A regra da quinta-feira (ISO) é intuitiva o bastante na visão mensal? Alternativa: atribuir cada semana ao mês do seu domingo.
- [ ] Validar o critério 4.5 (semana de criação só conta se a meta for atingida).
