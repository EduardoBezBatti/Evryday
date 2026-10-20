# Spec 001 — Hábitos e marcação diária

> **Status:** Aprovada
> **Depende de:** —
> **Última atualização:** 2026-10-20

## Contexto

Esta é a base do produto: cadastrar hábitos com sua frequência e marcar, na tela "Hoje", o que foi feito no dia. Sequências, calendário e estatísticas (specs 002 a 004) são construídos em cima do que esta spec registra.

## Fora de escopo

- Sequências, calendário e estatísticas (specs 002, 003 e 004)
- Marcar ou desmarcar dias passados (D07)
- Arquivar e reordenar hábitos
- Login e múltiplos usuários

## Requisitos

### Requisito 1 — Cadastrar hábito

**História:** Como usuário, quero cadastrar um hábito com nome, cor e frequência, para acompanhá-lo no dia a dia.

**Critérios de aceite**

1. QUANDO o usuário enviar o cadastro com dados válidos, o sistema DEVE criar o hábito com data de início igual a hoje e exibi-lo na lista de hábitos.
2. O sistema DEVE exigir nome com 1 a 60 caracteres, desconsiderando espaços no início e no fim, e DEVE salvar o nome sem esses espaços.
3. SE já existir um hábito com o mesmo nome, sem diferenciar maiúsculas de minúsculas, ENTÃO o sistema DEVE recusar o cadastro com a mensagem "Já existe um hábito com esse nome".
4. O sistema DEVE aceitar uma descrição opcional de até 200 caracteres.
5. O sistema DEVE exigir uma cor escolhida de uma paleta fixa de 8 cores.
6. O sistema DEVE exigir um tipo de frequência: diária, dias da semana ou vezes por semana.
7. QUANDO o tipo de frequência for "dias da semana", o sistema DEVE exigir ao menos um dia selecionado.
8. QUANDO o tipo de frequência for "vezes por semana", o sistema DEVE exigir uma meta inteira de 1 a 6.
9. SE algum dado for inválido, ENTÃO o sistema DEVE manter o formulário preenchido e exibir a mensagem de erro junto ao campo correspondente.

### Requisito 2 — Listar hábitos

**História:** Como usuário, quero ver todos os meus hábitos num só lugar, para gerenciá-los.

**Critérios de aceite**

1. O sistema DEVE exibir na tela "Hábitos" todos os hábitos em ordem de criação, com nome, cor e um resumo da frequência vigente (ex.: "Todos os dias", "Seg, Qua, Sex", "3 vezes por semana").
2. ENQUANTO não houver hábitos cadastrados, o sistema DEVE exibir uma mensagem de estado vazio com um atalho para cadastrar o primeiro hábito.

### Requisito 3 — Editar hábito

**História:** Como usuário, quero ajustar um hábito sem perder o que já registrei, para adaptá-lo à minha rotina.

**Critérios de aceite**

1. QUANDO o usuário salvar alterações de nome, descrição ou cor, o sistema DEVE atualizar o hábito sem alterar suas marcações.
2. O sistema DEVE aplicar na edição as mesmas validações do cadastro (critérios 1.2 a 1.8), desconsiderando o próprio hábito na verificação de nome repetido.
3. QUANDO o usuário alterar a frequência, o sistema DEVE aplicar a nova frequência a partir de hoje, inclusive, e manter a frequência anterior para os dias passados.
4. SE a frequência for alterada mais de uma vez no mesmo dia, ENTÃO o sistema DEVE considerar apenas a última alteração.
5. QUANDO a frequência mudar e o hábito já estiver marcado hoje, o sistema DEVE manter a marcação e classificá-la de acordo com a nova frequência (prevista ou extra).
6. O formulário de edição DEVE abrir preenchido com os dados atuais e a frequência vigente hoje.

### Requisito 4 — Excluir hábito

**História:** Como usuário, quero excluir um hábito que não faz mais sentido, para manter minha lista limpa.

**Critérios de aceite**

1. QUANDO o usuário pedir a exclusão, o sistema DEVE pedir confirmação avisando que o hábito e todo o seu histórico serão apagados permanentemente.
2. QUANDO o usuário confirmar, o sistema DEVE apagar o hábito, suas frequências e todas as suas marcações.
3. QUANDO o usuário cancelar, o sistema NÃO DEVE alterar nada.

### Requisito 5 — Tela "Hoje"

**História:** Como usuário, quero abrir o app e ver o que tenho para fazer hoje, para marcar rapidamente o que já fiz.

**Critérios de aceite**

1. O sistema DEVE abrir na tela "Hoje", exibindo a data atual por extenso (ex.: "terça-feira, 20 de outubro").
2. O sistema DEVE listar na seção "Para hoje" os hábitos cuja frequência vigente prevê o dia de hoje: todos os diários, os de dias da semana que incluem hoje e todos os de vezes por semana.
3. O sistema DEVE listar numa seção separada, "Extras", os hábitos de dias da semana que não preveem hoje.
4. ENQUANTO a seção "Extras" estiver vazia, o sistema NÃO DEVE exibi-la.
5. Para hábitos de vezes por semana, o sistema DEVE exibir o progresso da semana atual (ex.: "2 de 3 nesta semana"), contando as marcações de segunda a domingo.
6. QUANDO a meta semanal for atingida, o sistema DEVE indicar "Meta da semana atingida" e continuar permitindo marcar o hábito nos dias seguintes.
7. O sistema DEVE indicar visualmente quais hábitos já foram marcados hoje.
8. ENQUANTO não houver hábitos cadastrados, o sistema DEVE exibir uma mensagem de estado vazio com um atalho para cadastrar o primeiro hábito.

### Requisito 6 — Marcar e desmarcar hoje

**História:** Como usuário, quero marcar e desmarcar o que fiz hoje, para registrar meu progresso e corrigir um clique errado.

**Critérios de aceite**

1. QUANDO o usuário marcar um hábito na tela "Hoje", o sistema DEVE registrar uma marcação na data de hoje.
2. QUANDO o usuário desmarcar um hábito marcado hoje, o sistema DEVE remover a marcação de hoje.
3. O sistema DEVE manter no máximo uma marcação por hábito por dia: marcar um hábito já marcado NÃO DEVE criar outra marcação, e desmarcar um hábito não marcado NÃO DEVE gerar erro.
4. SE for pedida a marcação ou desmarcação de uma data diferente de hoje, ENTÃO o sistema DEVE recusar com a mensagem "Só é possível marcar o dia de hoje".
5. QUANDO um hábito de dias da semana for marcado num dia não previsto, o sistema DEVE aceitar a marcação como extra.
6. SE o dia virar enquanto a tela "Hoje" estiver aberta e o usuário tentar marcar ou desmarcar, ENTÃO o sistema DEVE recusar a operação, avisar que o dia mudou e recarregar a tela com os hábitos do novo dia.
7. SE a operação falhar, ENTÃO o sistema DEVE exibir uma mensagem de erro e manter o estado anterior do hábito na tela.

### Requisito 7 — Regras de calendário

**História:** Como usuário, quero que "hoje" e "semana" signifiquem sempre a mesma coisa, para que o app se comporte de forma previsível.

**Critérios de aceite**

1. O sistema DEVE determinar "hoje" pelo relógio do servidor, no fuso horário configurado (padrão: America/Sao_Paulo).
2. O sistema DEVE considerar que a semana vai de segunda a domingo.

## Requisitos não funcionais

1. A interface DEVE estar em português do Brasil e funcionar em telas a partir de 360px de largura.
2. Todos os elementos interativos DEVEM ser acessíveis por teclado e ter rótulo para leitores de tela.
3. Os erros da API DEVEM seguir o formato Problem Details (RFC 9457).
4. Os critérios dos requisitos 1, 3, 4, 6 e 7 DEVEM ser cobertos por testes automatizados.

## Casos de borda

| Situação | Comportamento esperado |
|---|---|
| Hábito criado hoje, de dias da semana que não incluem hoje | Aparece em "Extras" hoje (5.3) |
| Hábito de vezes por semana criado no meio da semana | Meta cheia na semana; o progresso conta só as marcações existentes (5.5) |
| Frequência muda de "Seg, Qua" para "diária" numa terça já marcada | A marcação de hoje deixa de ser extra e passa a ser prevista (3.5) |
| Frequência alterada duas vezes no mesmo dia | Vale só a última (3.4) |
| Duplo clique rápido em "marcar" | Uma única marcação (6.3) |
| Tela aberta antes da meia-noite, clique depois | Recusa, aviso e recarga (6.6) |
| Cadastro de "  Ler " e depois de "ler" | O primeiro é salvo como "Ler"; o segundo é recusado por nome repetido (1.2, 1.3) |
| Exclusão de hábito marcado hoje | Some da tela "Hoje" e da lista (4.2) |

## Decisões propostas pelo assistente (revise)

Estas regras não foram perguntadas na entrevista. Se discordar de alguma, altere aqui e em `docs/decisoes.md` antes de aprovar.

- Nome único, sem diferenciar maiúsculas de minúsculas (D19)
- Paleta fixa de 8 cores (D20)
- Meta semanal de 1 a 6 (D21)
- Data de início = dia do cadastro; lista em ordem de criação
- Semana de segunda a domingo (D17) e fuso America/Sao_Paulo (D18)
- Seção "Extras" separada na tela "Hoje", para permitir a marcação extra (D08)
- Meta semanal cheia mesmo na semana em que o hábito foi criado

## Dúvidas em aberto

- [ ] Nenhuma no momento. Anote aqui o que surgir na revisão.
