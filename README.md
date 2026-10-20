# Gerenciador de Hábitos

Aplicação web para cadastrar hábitos, marcar o que foi feito **hoje** e acompanhar sequências, histórico e taxa de conclusão.

Projeto pessoal desenvolvido com **Spec Driven Development**: cada funcionalidade é especificada, revisada e só depois implementada. As specs ficam em [`docs/specs`](docs/specs).

## Stack

.NET 10 · ASP.NET Core Minimal API · EF Core + SQLite · Angular + Angular Material · xUnit · GitHub Actions

## Specs

| Spec | Funcionalidade | Requisitos | Design | Tarefas | Implementação |
|---|---|---|---|---|---|
| [001](docs/specs/001-habitos-e-marcacao) | Hábitos e marcação diária | Aprovada | Aprovada | Aprovada | Em andamento |
| [002](docs/specs/002-sequencias) | Sequências | Rascunho | — | — | — |
| [003](docs/specs/003-historico-calendario) | Histórico em calendário | Rascunho | — | — | — |
| [004](docs/specs/004-estatisticas) | Estatísticas | Rascunho | — | — | — |

## Documentação

- [Produto](docs/produto.md): visão, escopo e glossário
- [Tecnologia](docs/tecnologia.md): arquitetura, convenções e testes
- [Decisões](docs/decisoes.md): o que foi decidido e por quê
- [Processo SDD](docs/processo-sdd.md): como trabalhamos neste projeto

## Como rodar

> Esta seção será preenchida ao concluir a Fase 0 da spec 001 (tarefa T0.5).

## Estrutura

```
backend/    API .NET (Dominio, Infraestrutura, Api + testes)
frontend/   App Angular
docs/       Produto, tecnologia, decisões e specs
```
