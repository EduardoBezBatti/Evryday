# Como usamos Spec Driven Development neste projeto

Guia rápido para consultar sempre que for começar uma funcionalidade nova.

## A ideia em uma frase

A especificação é a fonte da verdade: primeiro decidimos **o quê** e **por quê**, depois **como**, e só então escrevemos código, uma tarefa de cada vez.

## Os documentos

| Documento | Responde | Muda com que frequência |
|---|---|---|
| `docs/produto.md` | O que é o produto, para quem, o que está fora | Raramente |
| `docs/tecnologia.md` | Stack, arquitetura, convenções, testes | Raramente |
| `docs/decisoes.md` | Por que escolhemos X e não Y | A cada decisão relevante |
| `docs/specs/NNN-*/requisitos.md` | O que a funcionalidade faz (histórias + critérios de aceite) | Até ser aprovado |
| `docs/specs/NNN-*/design.md` | Como será construída (dados, API, telas, testes) | Até ser aprovado |
| `docs/specs/NNN-*/tarefas.md` | Passos pequenos e ordenados de implementação | Checkboxes durante a implementação |

## O ciclo de cada spec

1. **Requisitos**: copie `docs/specs/_modelo/` para `docs/specs/NNN-nome/` e escreva `requisitos.md`. Revise com o checklist abaixo e mude o status para `Aprovada`.
2. **Design**: escreva `design.md` com base nos requisitos aprovados. Todo requisito precisa aparecer em algum lugar do design.
3. **Tarefas**: quebre o design em tarefas de no máximo ~2 horas. Cada tarefa cita os requisitos que atende.
4. **Implementação**: uma tarefa por vez → código + testes → marca `[x]` → commit.
5. **Fechamento**: todo critério de aceite tem teste (ou verificação manual registrada), CI verde, status `Concluída`.

> **Regra de ouro:** se, durante a implementação, você descobrir que a spec está errada ou incompleta, **pare e corrija a spec primeiro**, depois o código. Spec e código nunca podem divergir.

Por que só a spec 001 tem design e tarefas prontos? Porque o design das próximas fica melhor quando escrito **depois** de implementar a anterior, já com o que você aprendeu. Os requisitos de 002 a 004 já existem para não perder as decisões da entrevista.

## Status de uma spec

`Rascunho` → `Em revisão` → `Aprovada` → `Em implementação` → `Concluída`

## Escrevendo critérios de aceite (EARS)

EARS (*Easy Approach to Requirements Syntax*) é um jeito de escrever requisitos com poucos padrões de frase. Isso evita ambiguidade e deixa cada critério fácil de transformar em teste.

| Padrão | Quando usar | Exemplo |
|---|---|---|
| O sistema **DEVE** … | Regra que vale sempre | O sistema DEVE exigir nome com 1 a 60 caracteres. |
| **QUANDO** <evento>, o sistema DEVE … | Reação a uma ação ou acontecimento | QUANDO o usuário marcar um hábito, o sistema DEVE registrar a marcação na data de hoje. |
| **ENQUANTO** <estado>, o sistema DEVE … | Comportamento durante um estado | ENQUANTO não houver hábitos, o sistema DEVE exibir o estado vazio. |
| **SE** <situação indesejada>, **ENTÃO** o sistema DEVE … | Erros e casos inválidos | SE a data for diferente de hoje, ENTÃO o sistema DEVE recusar a marcação. |
| **ONDE** <recurso opcional>, o sistema DEVE … | Comportamento que só existe se um recurso estiver presente | ONDE o login estiver habilitado, o sistema DEVE exibir apenas os hábitos do usuário. |

Dicas:

- Um critério = um comportamento verificável. Se tem "e" demais, quebre em dois.
- Evite palavras vagas como "rápido", "fácil", "intuitivo" e "etc.". Troque por algo mensurável.
- Exemplos concretos (tabelas de entrada → saída esperada) valem ouro, porque viram casos de teste diretamente.

## Checklist de revisão de requisitos

- [ ] Cada história tem papel, ação e benefício?
- [ ] Cada critério é testável (dá para escrever um teste que passa ou falha)?
- [ ] Os casos de erro e de borda estão cobertos (`SE … ENTÃO`)?
- [ ] O que está **fora de escopo** foi escrito explicitamente?
- [ ] Os termos batem com o glossário de `produto.md`?
- [ ] Não há decisão técnica escondida nos requisitos (isso vai no design)?
- [ ] As dúvidas em aberto foram resolvidas ou conscientemente adiadas?

## Checklist de revisão de design

- [ ] Todo requisito está coberto (veja a tabela de testes do design)?
- [ ] O modelo de dados suporta todos os casos de borda?
- [ ] Os contratos da API (requisição, resposta, erros) estão definidos?
- [ ] A estratégia de testes diz qual teste cobre cada critério?
- [ ] As alternativas descartadas estão registradas com o motivo?

## Trabalhando com o Claude (ou outra IA)

O `CLAUDE.md` na raiz já explica o fluxo para a IA. Alguns pedidos úteis:

- *"Leia a spec 001 e aponte ambiguidades ou casos de borda que faltam nos requisitos."*
- *"Com base nos requisitos aprovados da spec 002, escreva o design.md seguindo o modelo."*
- *"Gere o tarefas.md da spec 002 a partir do design."*
- *"Implemente a tarefa T1.1 da spec 001, com testes, e pare para eu revisar."*
- *"A tarefa T3.4 revelou que o critério 3.5 está incompleto. Proponha a alteração na spec antes de mexer no código."*

Peça **uma tarefa por vez** e revise o diff antes de seguir. É aí que o aprendizado acontece.
