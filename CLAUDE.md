# CLAUDE.md

Instruções para assistentes de IA neste repositório. Este projeto usa **Spec Driven Development**: a spec é a fonte da verdade.

## Antes de qualquer tarefa, leia

1. `docs/produto.md`: escopo, fora de escopo e **glossário** (use exatamente esses termos)
2. `docs/tecnologia.md`: arquitetura, convenções e testes
3. A spec em andamento: `docs/specs/NNN-*/requisitos.md` → `design.md` → `tarefas.md`

## Regras do fluxo

- **Sem spec aprovada, sem código.** Só implemente tarefas de specs cujo `requisitos.md` e `design.md` estejam com status `Aprovada`. Se estiverem em revisão, ajude a revisar.
- **Uma tarefa por vez**, na ordem de `tarefas.md`. Ao terminar: testes passando, marque `[x]`, resuma o que fez e **pare** para revisão. Não comece a próxima tarefa sem pedido.
- **Spec e código nunca divergem.** Se a spec estiver ambígua, errada ou incompleta, pare e proponha a alteração no documento antes de mudar o código.
- **Sem escopo extra.** Nada que não esteja nos requisitos (ver "Fora de escopo" em `produto.md` e na própria spec).
- Cada critério de aceite citado na tarefa precisa de teste. Prefira escrever o teste primeiro.
- Decisões técnicas novas e relevantes vão para `docs/decisoes.md`.

## Ao criar uma nova spec

1. Copie `docs/specs/_modelo/` para `docs/specs/NNN-nome-curto/`.
2. Escreva `requisitos.md` (formato EARS, ver `docs/processo-sdd.md`) e peça aprovação.
3. Depois `design.md` e peça aprovação. Depois `tarefas.md`.
4. Atualize a tabela de specs no `README.md`.

## Convenções essenciais

- Tudo em português (código, UI, commits). Identificadores sem acento: `Habito`, `Marcacao`, `PreveDia`.
- Regras de negócio só em `Habitos.Dominio`. Endpoints e front-end não decidem regras.
- Nunca use `DateTime.Now`/`DateTime.UtcNow`: "hoje" vem de `IRelogio` e entra no domínio como parâmetro.
- Erros HTTP em Problem Details: 400 validação, 404 não encontrado, 422 regra de negócio.
- Commits: Conventional Commits em português citando a tarefa, ex.: `feat(habitos): cadastra hábito (001/T3.3)`.

## Comandos

```bash
dotnet build backend
dotnet test backend
dotnet run --project backend/src/Habitos.Api
cd frontend && npm start        # http://localhost:4200 (proxy /api → API)
cd frontend && npm test
cd frontend && npm run build
```
