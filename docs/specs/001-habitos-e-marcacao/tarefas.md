# Spec 001 — Tarefas

> **Status:** Aprovada
> **Design:** [design.md](design.md)

Como usar: faça **uma tarefa por vez**, na ordem. Cada tarefa termina com o projeto compilando e os testes verdes. Ao concluir, marque `[x]` e faça commit citando o código, por exemplo `feat(dominio): cria Frequencia (001/T1.1)`.

## Fase 0 — Fundação

- [x] **T0.1** Repositório
  - `git init`, `.gitignore` (`dotnet new gitignore` + `node_modules/` e `*.db`) e commit inicial com `docs/`, `README.md` e `CLAUDE.md`
  - Criar o repositório no GitHub e fazer o primeiro push
- [x] **T0.2** Solução .NET
  - `backend/Habitos.slnx` com `Habitos.Dominio` e `Habitos.Infraestrutura` (classlib), `Habitos.Api` (web) e `Habitos.Dominio.Testes` e `Habitos.Api.Testes` (xUnit)
  - Referências respeitando as regras de dependência de `docs/tecnologia.md`
  - `Directory.Build.props` com `Nullable`, `ImplicitUsings` e `TreatWarningsAsErrors`
  - Pronto quando: `dotnet build backend` e `dotnet test backend` passam
- [x] **T0.3** App Angular
  - `ng new` em `frontend/` (com roteamento, SCSS, sem SSR) e `ng add @angular/material`
  - Registrar o locale `pt-BR`; `proxy.conf.json` com `/api` → URL da API
  - Pronto quando: `npm test` e `npm run build` passam
- [ ] **T0.4** CI no GitHub Actions
  - `.github/workflows/ci.yml` com os jobs `backend` e `frontend` (ver `docs/tecnologia.md`)
  - Pronto quando: pipeline verde no GitHub
- [ ] **T0.5** Esqueleto ponta a ponta
  - `GET /api/saude` na API e uma página provisória no Angular exibindo o resultado via proxy
  - Preencher a seção "Como rodar" do `README.md`
  - Pronto quando: com API e front-end rodando, o navegador mostra "API ok"

## Fase 1 — Domínio (comece pelos testes)

- [ ] **T1.1** Frequência
  - `TipoFrequencia`, `DiasDaSemana` (flags) e o record `Frequencia` com `Diaria()`, `NosDias()`, `VezesNaSemana()` e `PreveDia()`
  - `ExcecaoDeValidacao` (campo + mensagem)
  - Testes: ao menos um dia; meta de 1 a 6; `PreveDia` nos três tipos
  - _Requisitos: 1.6, 1.7, 1.8, 5.2_
- [ ] **T1.2** Semana
  - `Semana.InicioDe(data)` retornando a segunda-feira
  - Testes: segunda, quarta e domingo da mesma semana retornam a mesma segunda
  - _Requisitos: 7.2_
- [ ] **T1.3** Criação e dados do hábito
  - `Habito.Criar()`, `AtualizarDados()` e `PaletaDeCores`
  - Testes: nome com espaços nas pontas, com 0 e com 61 caracteres; descrição com 201; cor fora da paleta; data de início = hoje; primeira agenda vigente desde hoje
  - _Requisitos: 1.1, 1.2, 1.4, 1.5, 3.1_
- [ ] **T1.4** Agenda versionada
  - `AgendaHabito`, `AlterarFrequencia()` e `FrequenciaEm()`
  - Testes: a mudança vale de hoje em diante; dias passados mantêm a frequência anterior; duas mudanças no mesmo dia; voltar à frequência anterior no mesmo dia remove a agenda de hoje
  - _Requisitos: 3.3, 3.4_
- [ ] **T1.5** Marcações
  - `Marcacao`, `Marcar()`, `Desmarcar()`, `EstaMarcadoEm()`, `EhExtra()`, `MarcacoesNaSemana()` e `ExcecaoDeRegraDeNegocio`
  - Testes: só hoje; marcar duas vezes; desmarcar sem marcação; extra em dia não previsto; extra vira prevista após mudança de frequência; contagem da semana de segunda a domingo
  - _Requisitos: 3.5, 5.5, 6.1, 6.2, 6.3, 6.4, 6.5_

## Fase 2 — Persistência e relógio

- [ ] **T2.1** `HabitosDbContext` e mapeamentos
  - Tabelas, `NomeNormalizado` com índice único, índices únicos de agenda e de marcação, cascata e complex type `Frequencia`
  - _Requisitos: 1.3, 4.2, 6.3_
- [ ] **T2.2** Migration inicial
  - Criar a migration e aplicá-la na inicialização em Development; `habitos.db` fora do Git
- [ ] **T2.3** Relógio
  - `IRelogio` + `Relogio` (TimeProvider + fuso de `Aplicacao:FusoHorario`), registrado na injeção de dependência
  - Testes com `FakeTimeProvider`: 2026-10-21T02:30Z → hoje = 2026-10-20
  - _Requisitos: 7.1_

## Fase 3 — API

- [ ] **T3.1** Base dos testes de integração
  - `WebApplicationFactory` com SQLite em memória e `FakeTimeProvider` no lugar dos serviços reais
- [ ] **T3.2** Erros e serialização
  - `IExceptionHandler` mapeando `ExcecaoDeValidacao` → 400 e `ExcecaoDeRegraDeNegocio` → 422 (Problem Details); enums como texto em camelCase
  - _Requisitos: RNF 3, 1.9_
- [ ] **T3.3** Cadastro e consulta
  - `POST /api/habitos`, `GET /api/habitos` e `GET /api/habitos/{id}`, com verificação de nome repetido
  - Testes: 201 + `Location`; 400 por campo; nome repetido ("Água" × "ÁGUA"); ordem de criação; 404
  - _Requisitos: 1.1–1.8, 2.1, 3.6_
- [ ] **T3.4** Edição
  - `PUT /api/habitos/{id}`
  - Testes: dados atualizados sem tocar as marcações; mudança de frequência preserva o passado; a verificação de nome repetido ignora o próprio hábito
  - _Requisitos: 3.1–3.5_
- [ ] **T3.5** Exclusão
  - `DELETE /api/habitos/{id}`
  - Testes: hábito, agendas e marcações removidos; 404
  - _Requisitos: 4.2_
- [ ] **T3.6** Endpoint da tela "Hoje"
  - `GET /api/hoje`
  - Testes: diário e 3x por semana em `paraHoje`; dias da semana fora de hoje em `extras`; progresso e `metaAtingida`
  - _Requisitos: 5.1, 5.2, 5.3, 5.5, 5.6, 5.7_
- [ ] **T3.7** Marcações
  - `PUT` e `DELETE /api/habitos/{id}/marcacoes/{data}`
  - Testes: 204; repetição não duplica; data ≠ hoje → 422 com `hoje`; 404
  - _Requisitos: 6.1–6.6, 7.1_

## Fase 4 — Front-end

- [ ] **T4.1** Núcleo
  - Modelos TypeScript dos contratos, `HabitosApiService` e interceptor de erros com snackbar
  - _Requisitos: RNF 3, 6.7_
- [ ] **T4.2** Layout e rotas
  - Shell com barra de navegação (Hoje, Hábitos), rotas do design, tema Material e layout responsivo
  - _Requisitos: 5.1, RNF 1, RNF 2_
- [ ] **T4.3** Pipe `resumoFrequencia`
  - Testes: diária; dias fora de ordem (sáb, seg → "Seg, Sáb"); "1 vez" × "3 vezes"
  - _Requisitos: 2.1_
- [ ] **T4.4** Tela "Hábitos"
  - Lista, estado vazio e exclusão com `DialogoConfirmacao`
  - _Requisitos: 2.1, 2.2, 4.1, 4.2, 4.3_
- [ ] **T4.5** Formulário de hábito
  - Cadastro e edição, seletor de cor, seletor de frequência, validações no cliente e erros 400 por campo
  - _Requisitos: 1.1–1.9, 3.1–3.6_
- [ ] **T4.6** Tela "Hoje"
  - Data por extenso, seções "Para hoje" e "Extras", progresso semanal, marcar/desmarcar e recarga após 422
  - _Requisitos: 5.1–5.8, 6.1, 6.2, 6.6, 6.7_

## Fase 5 — Fechamento

- [ ] **T5.1** Rastreabilidade: conferir a tabela de testes do design; todo critério tem teste ou passo manual
- [ ] **T5.2** Roteiro manual: fluxo completo no desktop e em 360px (DevTools), com navegação só por teclado
- [ ] **T5.3** Atualizar o `README.md` e mudar o status desta spec para `Concluída`
