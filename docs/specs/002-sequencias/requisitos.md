# Spec 002 — Sequências

> **Status:** Rascunho. O design e as tarefas serão escritos quando a spec 001 estiver concluída.
> **Depende de:** 001
> **Última atualização:** 2026-10-20

## Contexto

A sequência ("streak") é o principal motivador de um app de hábitos: ver quantos dias seguidos você manteve o hábito. Como só é possível marcar o dia de hoje (D07), a sequência reflete fielmente o que aconteceu.

## Fora de escopo

- "Congelar" ou recuperar uma sequência perdida
- Aviso de sequência em risco
- Conquistas ou medalhas por sequência

## Requisitos

### Requisito 1 — Sequência atual

**História:** Como usuário, quero ver há quantos dias seguidos mantenho cada hábito, para me manter motivado.

**Critérios de aceite**

1. O sistema DEVE calcular a sequência atual percorrendo os dias de hoje para trás, sem ultrapassar a data de início do hábito.
2. Para avaliar cada dia, o sistema DEVE usar a frequência vigente naquele dia (spec 001, critério 3.3).
3. QUANDO o dia avaliado for previsto e estiver marcado, o sistema DEVE somar 1 à sequência e seguir para o dia anterior.
4. QUANDO o dia avaliado for previsto e não estiver marcado, o sistema DEVE encerrar a contagem.
5. QUANDO o dia avaliado não for previsto, o sistema DEVE ignorá-lo: ele não soma e não quebra a sequência, mesmo que tenha marcação extra.
6. ENQUANTO hoje não estiver marcado, o sistema NÃO DEVE tratar hoje como quebra: a contagem começa em ontem.
7. Em hábitos de vezes por semana, o sistema DEVE considerar todos os dias como previstos para a sequência, ou seja, contar dias seguidos marcados, independentemente da meta semanal (D10).

### Requisito 2 — Maior sequência

**História:** Como usuário, quero ver meu recorde em cada hábito, para ter uma meta a superar.

**Critérios de aceite**

1. O sistema DEVE calcular a maior sequência aplicando as regras do requisito 1 sobre todo o histórico do hábito.
2. A maior sequência DEVE ser sempre maior ou igual à sequência atual.
3. QUANDO o hábito ainda não tiver marcações, o sistema DEVE exibir sequência atual 0 e maior sequência 0.

### Requisito 3 — Exibição

**História:** Como usuário, quero ver as sequências onde já olho todo dia, sem precisar procurar.

**Critérios de aceite**

1. O sistema DEVE exibir a sequência atual de cada hábito na tela "Hoje".
2. O sistema DEVE oferecer uma tela de detalhe do hábito (rota `/habitos/:id`, acessada pela lista de hábitos e pela tela "Hoje") com nome, descrição, frequência vigente, sequência atual, maior sequência e as ações de editar e excluir.
3. QUANDO o usuário marcar ou desmarcar um hábito hoje, o sistema DEVE atualizar a sequência exibida.
4. O sistema DEVE exibir a sequência como número acompanhado de uma unidade (ver dúvida em aberto).

## Exemplos (viram casos de teste)

Semana de referência: segunda 19/10/2026 a domingo 25/10/2026. Em todos os exemplos, o hábito foi criado na segunda 19/10. ✓ = marcado.

| # | Frequência | Marcações | Hoje | Atual | Por quê |
|---|---|---|---|---|---|
| 1 | Diária | seg ✓, ter ✓, qua ✓ | qui 22, sem marcação | 3 | Hoje pendente não quebra |
| 2 | Diária | seg ✓, qua ✓, qui ✓ | qui 22 | 2 | A terça sem marcação quebra |
| 3 | Seg, Qua, Sex | seg ✓, qua ✓ | qui 22, sem marcação | 2 | Terça e quinta não são previstas |
| 4 | Seg, Qua, Sex | seg ✓, ter ✓ (extra) | qua 21, sem marcação | 1 | O extra é ignorado; a quarta está pendente |
| 5 | 3x por semana | seg ✓, qua ✓ | qui 22, sem marcação | 1 | A terça sem marcação quebra (D10) |
| 6 | Diária até ter 20; Seg, Qua, Sex a partir de qua 21 | seg ✓, ter ✓, qua ✓ | qui 22, sem marcação | 3 | Cada dia usa a frequência vigente nele |

## Dúvidas em aberto

- [ ] Unidade da sequência: "5 dias" pode confundir em hábitos de dias da semana, onde contam só os dias previstos. Alternativas: "5 seguidas" ou "5 dias" com tooltip explicando. Decidir no design.
