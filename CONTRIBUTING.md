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
- **Keep PRs small: max 20 files, 10-15 preferred.** If a change is going
  to blow past that, plan the split into multiple PRs upfront, not after
  the fact.
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

## Testing

- **Build test doubles reusably from the start, not as a private nested
  class you promote later.** Put the double in the relevant test
  project's `TestHelpers/` (or equivalent) folder the first time, not
  the second.
- **Serialize real DTOs instead of hand-writing JSON/CSV string
  literals** for canned response bodies — construct the actual type and
  serialize it, so the fixture can't drift from the real shape.
- **Name test doubles `Test*`**, not `Stub*`/`Fake*`/`Mock*`.

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
`User Value` issues get broken into sub-issues through backlog
refinement (see [Projects](#projects) below), a few at a time — not all
at once upfront.

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

Work bigger than a single issue goes through a pipeline biased toward
re-planning over predicting — estimates are inputs to prioritization,
not commitments to defend:

1. **WAG** — a fast, rough gut-feel estimate (in months), logged on the
   [Ideas board](https://github.com/users/joshuaedwardcrowe/projects/10)'s
   `WAG (months)` field, purely to judge whether an idea is worth
   pursuing at all. Non-binding — expected to be wrong.
2. **SWAG** — the same estimate, re-checked against everything else
   competing for the slot, logged in the same board's `SWAG (months)`
   field. "Prioritizing" means sorting/grouping that board by
   `Priority` (`High`/`Medium`/`Low`) or `SWAG` — there's no separate
   roadmap artifact to keep in sync. Still non-binding: a relative
   sizing input, not a plan.
3. **New GitHub Project** — once an idea is greenlit, it graduates off
   the Ideas board into its own project (e.g.
   [Spendfulness](https://github.com/users/joshuaedwardcrowe/projects/9),
   [YNAB Analysis & Automation](https://github.com/users/joshuaedwardcrowe/projects/8)).
4. **Inception spike** — plans the *next* milestone in real detail;
   everything beyond that is a rough forecast, re-planned properly once
   you actually get there (rolling-wave planning, not a full plan for
   the whole estimate up front). Refresh the Ideas board's `Validated
   Estimate (months)` field as it's learned, not just once.
5. **Backlog refinement, just-in-time** — rather than one big spike
   producing the full chronological order for an entire milestone, only
   the next handful of tickets need to be fully ordered and estimated
   at any moment. The rest of the milestone stays a loosely-ordered
   backlog, refined incrementally as work proceeds. A milestone-scale
   re-planning pass is still useful when picking up a milestone cold —
   treat its output as a starting point, not a fixed contract.

   A **spike** (a specific, scoped investigation — "should we support
   X," "what does Y actually look like") resolves to one of two
   outcomes: **new complexity found**, or **no new complexity**. On no
   new complexity, close the spike and open a fresh, cleanly-titled
   delivery-stage ticket for the actual build — don't retitle or reuse
   the spike issue in place. That new ticket gets sized in a normal
   backlog-refinement pass, not as part of the spike itself.
6. **Fixed-length iterations + end-of-iteration review** — work in
   short, regular iterations rather than open-ended milestone spans.
   At the end of each one: check what actually got done vs. planned,
   re-prioritize the backlog based on what was learned, and feed the
   iteration's actual pace back into WAG/SWAG calibration. This
   inspect-and-adapt step is what keeps the rest of the pipeline
   honest — without it, WAG/SWAG/the inception spike are just a plan
   nobody revisits.
7. **Tickets with Estimates** — the leaf/actionable tickets pulled into
   an iteration get the `Estimate` field (Fibonacci story points, not
   time) on the project board — the parent story tracks the outcome,
   not the effort to reach it.

This repo follows [SoloCAIRN](https://github.com/joshuaedwardcrowe/SoloCAIRN)
for a ticket's Build-stage lifecycle, with one extension specific to
this repo, not something SoloCAIRN itself prescribes: **the GitHub
Issue itself is the story artifact** — no separate markdown file or
dedicated location. It's already written down, reviewable via
comments, and tracked through GitHub's own history.

Current project boards:

- [YNAB Analysis & Automation](https://github.com/users/joshuaedwardcrowe/projects/8) — analyse and CRUD commands
- [Spendfulness](https://github.com/users/joshuaedwardcrowe/projects/9) — the spendfulness-measurement theme
- [Ideas](https://github.com/users/joshuaedwardcrowe/projects/10) — the WAG/SWAG staging board described above
- Creating a Proof of Concept, Adding Settings, Tech Debt Monitoring — narrower/unexplored scopes

## Milestones

Milestones group issues by feature area within this repo (e.g.
`Measuring Spendfulness`, `Modifying YNAB Data`) — narrower and
repo-scoped, unlike a Project board which can span themes or
work-types. Only the immediate milestone is planned in real detail (via
the **inception spike**, step 4 above); later milestones stay a rough
forecast, refined properly when picked up.

## Questions

Open an issue if something in this document is unclear or actively
getting in the way — this document is subject to the same process as
everything else.
