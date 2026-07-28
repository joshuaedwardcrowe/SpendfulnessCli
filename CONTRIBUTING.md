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
planning spike has established delivery order (see
[Projects](#projects) below) — not upfront.

## Projects

Work bigger than a single issue is tracked on a GitHub Projects (v2)
board rather than just a milestone — a board can carry an `Estimate`
field (story points, not time) and group issues by theme or work-type
across what a milestone alone can't. Current boards:

- [YNAB Analysis & Automation](https://github.com/users/joshuaedwardcrowe/projects/8) — analyse and CRUD commands
- [Spendfulness](https://github.com/users/joshuaedwardcrowe/projects/9) — the spendfulness-measurement theme
- Creating a Proof of Concept, Adding Settings, Tech Debt Monitoring — narrower/unexplored scopes

**Starting a new project:** open it with a planning spike first, not a
pre-built backlog. Decomposing a story-shaped issue into sub-issues
before the delivery order is actually agreed produces a breakdown that
looks plausible but can't be proven correct — e.g. proposing a report
command before the data it depends on is parsed. The spike's job is to
establish that order collaboratively; only create sub-issues once it
concludes, and in the order it settles on.

**Estimates** go on the leaf/actionable sub-issues, not the parent
story issue — the parent tracks the outcome, not the effort to reach
it.

## Milestones

Milestones group issues by feature area within this repo (e.g.
`Measuring Spendfulness`, `Modifying YNAB Data`) — narrower and
repo-scoped, unlike a Project board which can span themes or work-types.

## Questions

Open an issue if something in this document is unclear or actively
getting in the way — this document is subject to the same process as
everything else.
