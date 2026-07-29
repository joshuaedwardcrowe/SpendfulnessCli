# Contributing to SpendfulnessCli

SpendfulnessCli is a personal tool, not a published library — the
process below is deliberately lighter than [KitCli](https://github.com/KitCli/KitCli)'s
or [YnabSharp](https://github.com/joshuaedwardcrowe/YnabSharp)'s, but
keeps the same core habit: non-obvious decisions get a paper trail
instead of relying on memory.

## Before you write code

- **Bugs and small fixes** — just open a PR. No issue required.
- **New commands, or anything that changes how a command reports or
  aggregates data** — open an issue first. Get the shape agreed before
  investing in the implementation.
- **Architectural decisions** — see [ADRs](#adrs) below.

## Branching & PRs

- Branch off `main`, one branch per issue (e.g.
  `163-splittable-transactions-personalisation`). No long-running
  branches.
- One logical change per PR.
- **PR titles use [Conventional Commits](https://www.conventionalcommits.org/):**
  `<type>(scope): <description>` — `type` is one of `feat` `fix` `docs`
  `chore` `refactor` `test` `ci`; `scope` (optional) matches a command
  area (`chat`, `export`, `organisation`, `personalisation`,
  `reporting`, `reusable`). `description` is lowercase, imperative, no
  trailing period. For a breaking change, add `!` right before the
  colon. Example: `feat(reporting): add loan-to-value command`.
- There's no enforced CI or required review on this repo (single
  maintainer, no branch protection) — build and test locally before
  merging anyway.
- Docs-only changes (like this file) can be committed straight to
  `main`.

## ADRs

An ADR ([`ADR/`](ADR/)) captures a decision — its premise, the problem,
and the solution — not how something works today (that's
[`CONCEPTS.md`](CONCEPTS.md)). Number sequentially
(`ADRxx-Title.md`). [`FUTURE-ADR.md`](FUTURE-ADR.md) tracks decisions
that are known to need one but haven't been written yet.

**Write one when you're** introducing a new cross-cutting pattern,
changing a project boundary, or making a breaking change to a command's
shape. **Skip it for** bug fixes and internal refactors.

## Concepts

[`CONCEPTS.md`](CONCEPTS.md) explains how the system's pieces fit
together today — Http/Clients/Wrappers, Commands, Compilation,
Aggregates, etc. Keep it current: if a change makes it inaccurate,
update it in the same PR.

## Issues

Labels: `User Value` (direct value to the CLI's user) · `Developer
Value` (internal/plumbing) · `New Command` · `Tech Debt` · `Tech Debt -
Preferential` · `Bug` · `Enhancement` · `Documentation` · `Integration`
· `Spike`.

`User Value` is a value classification, not an Agile user-story
format — don't force issues into "As a user, I want..." shape. Larger
`User Value` issues get broken into sub-issues, but only once a
milestone spike has established delivery order (see
[Projects](#projects) below) — not upfront.

**Issue titles** follow a two-stage convention:

- **Idea-stage** (unvalidated, pre-WAG) — plain-language problem
  statements, e.g. "No way to identify X" / "No command to do Y". This
  is deliberate: an idea is a pitch for an unmet need, not yet a scoped
  unit of work.
- **Delivery-stage sub-issues** (carved out by a planning spike, ready
  to build) — Conventional Commits style, matching PR titles:
  `type(scope): description`, e.g. `feat(reporting): add loan-to-value
  command`. By this point the work is scoped, so the title should read
  like the commit that will close it.

## Projects

Work bigger than a single issue goes through a staged pipeline before
it's ever decomposed into tickets:

1. **WAG** — a fast, rough gut-feel estimate (in months), logged on the
   [Ideas board](https://github.com/users/joshuaedwardcrowe/projects/10)'s
   `WAG (months)` field, purely to judge whether an idea is worth
   pursuing at all.
2. **SWAG** — the same estimate, re-checked against everything else
   competing for the slot, logged in the same board's `SWAG (months)`
   field. "Prioritizing" means sorting/grouping that board by
   `Priority` (`High`/`Medium`/`Low`) or `SWAG` — there's no separate
   roadmap artifact to keep in sync.
3. **New GitHub Project** — once an idea is greenlit, it graduates off
   the Ideas board into its own project (e.g.
   [Spendfulness](https://github.com/users/joshuaedwardcrowe/projects/9),
   [YNAB Analysis & Automation](https://github.com/users/joshuaedwardcrowe/projects/8)).
4. **Inception spike** — validates the WAG/SWAG estimate for real and
   defines milestones spanning that period, logged on the Ideas board's
   `Validated Estimate (months)` field. This spike's output is
   milestones, not tickets.
5. **Pick up a milestone**, then run a **milestone spike** — this is
   the one that plans the actual chronological delivery order and
   produces the ordered ticket breakdown for that specific milestone.
   Don't skip straight from the inception spike to tickets: decomposing
   before delivery order is agreed produces a breakdown that looks
   plausible but can't be proven correct — e.g. proposing a report
   command before the data it depends on is parsed.
6. **Tickets with Estimates** — the milestone spike's tickets get the
   `Estimate` field (Fibonacci story points, not time) on the project
   board, on the leaf/actionable tickets — the parent story tracks the
   outcome, not the effort to reach it.

Current project boards:

- [YNAB Analysis & Automation](https://github.com/users/joshuaedwardcrowe/projects/8) — analyse and CRUD commands
- [Spendfulness](https://github.com/users/joshuaedwardcrowe/projects/9) — the spendfulness-measurement theme
- [Ideas](https://github.com/users/joshuaedwardcrowe/projects/10) — the WAG/SWAG staging board described above
- Creating a Proof of Concept, Adding Settings, Tech Debt Monitoring — narrower/unexplored scopes

## Milestones

Milestones group issues by feature area within this repo (e.g.
`Measuring Spendfulness`, `Modifying YNAB Data`) — narrower and
repo-scoped, unlike a Project board which can span themes or
work-types. A milestone's definition comes out of a project's
**inception spike** (step 4 above), not chosen upfront.

## Questions

Open an issue if something in this document is unclear or actively
getting in the way — this document is subject to the same process as
everything else.
