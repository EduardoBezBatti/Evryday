# Registro de decisões

Cada decisão relevante fica registrada aqui, com o motivo. Para mudar uma decisão, adicione uma nova linha que a substitui (ex.: "substitui D10") em vez de apagar a antiga.

Origem: **E** = escolhida na entrevista inicial (2026-10-20) · **P** = proposta pelo assistente, *aguardando sua validação*.

| # | Decisão | Motivo / consequência | Origem |
|---|---|---|---|
| D01 | App web: ASP.NET Core (API) + Angular | Aprender a stack e ter um projeto de portfólio com front e back separados | E |
| D02 | .NET 10 (LTS) | Versão com suporte longo mais recente | E |
| D03 | SQLite + EF Core | Nada para instalar; o EF permite trocar por PostgreSQL depois | E |
| D04 | Sem login no MVP | Foco no núcleo do produto. Login vira a spec 005, com migration que associa os hábitos existentes ao primeiro usuário | E |
| D05 | Hábitos apenas sim/não | Simplicidade; hábitos com quantidade ficam para o futuro | E |
| D06 | Três frequências: diária, dias da semana e X vezes por semana | Cobre as rotinas mais comuns | E |
| D07 | Só é possível marcar ou desmarcar **hoje** | App "honesto". Consequência: o calendário é somente leitura e esquecimentos não podem ser corrigidos | E |
| D08 | Hábito de dias da semana aceita marcação **extra** em dia não previsto | Conta nas estatísticas (como contagem separada) e não afeta a sequência | E |
| D09 | Mudança de frequência vale **de hoje em diante** (agenda versionada) | Preserva o passado, mas exige guardar o histórico de agendas. É a regra mais complexa do domínio | E |
| D10 | Sequência de "X vezes por semana" conta **dias seguidos marcados** | Escolha consciente: um hábito de 3x por semana terá sequências curtas, porque todo dia não marcado quebra a sequência. Reavaliar depois de usar o app | E |
| D11 | Exclusão definitiva, com confirmação | Simples; não há arquivamento no MVP | E |
| D12 | Back-end em camadas: Dominio, Infraestrutura e Api | Domínio testável isoladamente, sem a burocracia da Clean Architecture completa | E |
| D13 | Angular Material | Biblioteca oficial, acessível e bem documentada | E |
| D14 | Tudo em português (código, UI, specs e commits) | Preferência do projeto | E |
| D15 | GitHub + GitHub Actions | Portfólio e retorno automático dos testes | E |
| D16 | Testes unitários + integração | Cada critério de aceite vira teste | E |
| D17 | Semana de segunda a domingo | Padrão ISO 8601; afeta a meta semanal e as estatísticas | E |
| D18 | "Hoje" definido pelo servidor, com fuso configurável (padrão `America/Sao_Paulo`) | Uma única fonte de verdade para a regra D07. O cliente envia a data que acredita ser hoje só para detectar a virada do dia | E |
| D19 | Nome de hábito único, sem diferenciar maiúsculas de minúsculas | Evita hábitos duplicados por engano | E |
| D20 | Cor escolhida de uma paleta fixa de 8 cores | Visual consistente e validação simples | E |
| D21 | Meta de "vezes por semana" entre 1 e 6 | 7 vezes por semana equivale a diária | E |
| D22 | "Extra" não é gravado; é derivado da frequência vigente | Se a frequência mudar no mesmo dia, a classificação acompanha automaticamente | E |
