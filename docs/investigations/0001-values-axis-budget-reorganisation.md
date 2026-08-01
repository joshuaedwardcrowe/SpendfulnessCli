# 0001. Can SpendfulnessCli perform a values-axis budget re-organisation?

- **Status:** In Review
- **Spike:** #200
- **Time-box:** 2 days
- **Date:** 2026-08-01

## Verdict

**New complexity.** A values-axis re-organisation is *merge-shaped* — many
existing categories collapse into fewer, value-named ones — and a merge
decomposes into four operations of which the YNAB API can perform one and
a half:

| # | Step | Possible? |
|---|---|---|
| 1 | Create the target category / group | ✅ `createCategory`, `createCategoryGroup` |
| 2 | Re-point transactions from source to target | ⚠️ bulk update works, **but split transactions cannot be re-categorised at all** |
| 3 | Move money already assigned to the source | ❌ `money_movements` is read-only |
| 4 | Retire the emptied source category | ❌ no `DELETE`; `hidden` is not writable |

This is not a coverage gap that closing more YnabSharp tickets will fix.
Steps 3 and 4 are ceilings in the YNAB API itself, and the split-transaction
restriction in step 2 is explicit in the spec.

The capability is therefore not "perform the migration." It is **perform the
automatable portion, then hand the user a checklist for the rest.** That is
still worth building — it is the difference between an afternoon of careful
clicking and a week of it — but it changes what the milestone is promising,
so it needs to be scoped that way rather than discovered halfway through.

## Recommendation

Build it as two things, sequenced:

1. **A mapping-driven re-point command.** Takes a hand-authored old → new
   mapping, creates any missing target categories and groups, and bulk
   re-points every non-split transaction. Depends on YnabSharp
   [#147](https://github.com/joshuaedwardcrowe/YnabSharp/issues/147)
   (create), [#100](https://github.com/joshuaedwardcrowe/YnabSharp/issues/100)
   and [#101](https://github.com/joshuaedwardcrowe/YnabSharp/issues/101)
   (group create/update). None are built yet — this is blocked on them.
2. **A retirement checklist output.** For everything the API can't do:
   each source category that still holds assigned money, and each one
   that now needs hiding by hand. Generated, not written by the user.

Split transactions need a third decision before either lands — see open
questions. Do not slice this into sub-issues until that's answered; it
changes whether step 1 is complete or partial.

## What was established

**The migration is merge-shaped, not rename-shaped.** A rename would be
`updateCategory` and nothing else. Consolidation is what makes it hard, and
it holds regardless of which target taxonomy is finally chosen — the shape
follows from the merge ratio, not from any particular set of names.

**Split transactions cannot be re-categorised.** The spec is explicit:
"If an existing transaction is a split, the `category_id` cannot be
changed." This matters more here than it would elsewhere, because this repo
already treats splittable transactions as a first-class concern (#165, #164).

**Three permanent API ceilings.** No `DELETE` for categories or groups (the
only two `delete` operations in the whole spec are on Transactions and
Scheduled Transactions); `hidden` is required on `CategoryBase` but absent
from `SaveCategory`, so it is readable and never writable; all four
`money_movements` operations are `get*`.

> *Permanent home:* YnabSharp's `docs/ynab-api-coverage.md`, under "What the
> API cannot do" — added in
> [YnabSharp#148](https://github.com/joshuaedwardcrowe/YnabSharp/pull/148).
> This file is not the only copy.

**The mapping cannot be inferred.** Nothing in the data says `🧺 Cupboard`
belongs under `Sanctuary (Am I Physically Safe)` rather than
`Community (I Am Loved)`. It is a values judgement, so it has to be authored
by hand and fed to the tool as input.
[my-financial-map](https://github.com/joshuaedwardcrowe/my-financial-map)
is the prose draft of it.

**Previewing is possible, but the scratch plan is made by hand.** YNAB
allows more than one plan, and the CLI can target any of them by id. But
there is no create-plan operation in the API (`getPlans`, `getPlanById`,
`getPlanSettingsById` only), so the throwaway copy has to be created in the
YNAB web app first. Cheap, and worth doing — just not automatable end to end.

## Evidence

Against the vendored spec, `docs/ynab-openapi-spec.yaml` at version
**1.86.0** in YnabSharp (last checked 2026-07-26):

```
# category operations — eight, not the seven the coverage doc listed
grep -n "operationId:" docs/ynab-openapi-spec.yaml | grep -i categor

# every delete verb in the spec — two, both on transactions
grep -n "^\s*delete:" docs/ynab-openapi-spec.yaml

# money movements — all four are get*
grep -n "operationId:" docs/ynab-openapi-spec.yaml | grep -i money

# hidden: present on the read schemas, absent from SaveCategory
grep -n "hidden" docs/ynab-openapi-spec.yaml
sed -n '/^    SaveCategory:/,/^    SaveMonthCategory:/p' docs/ynab-openapi-spec.yaml
```

The split-transaction restriction is in the `category_id` description on
`SaveTransactionWithOptionalFields`.

The under-reported counts this turned up were corrected in
[YnabSharp#148](https://github.com/joshuaedwardcrowe/YnabSharp/pull/148),
and the missing create endpoint raised as
[YnabSharp#147](https://github.com/joshuaedwardcrowe/YnabSharp/issues/147).

## Open questions

- **What happens to split transactions?** Leave them, report them, or
  delete-and-recreate them (which is possible — `deleteTransaction` and
  `createTransaction` both exist — but changes transaction ids and any
  import linkage)? This is the one that blocks breakdown.
- **Is the target taxonomy total or partial?** Banking, `Get a Car`,
  CroweCaptured, Home Maintenance and most of Celebrating currently have no
  value assigned in my-financial-map. If a residue is legitimate, the tool
  needs an explicit "unmapped" outcome rather than treating it as an error.
  Raised separately.
- **How much assigned money actually needs moving?** Step 3 is manual, so
  its cost is proportional to how many source categories hold a non-zero
  balance at migration time. Migrating just after a month rolls over may
  make it nearly free. Unmeasured.

## Out of scope

The taxonomy itself — which values exist and what belongs under them — is a
personal decision, not a technical one, and was not evaluated here.

`SPENDFULNESS.md`'s §3 Category-Group-to-Behaviour matrix looks overtaken by
this change of axis, but sense-checking it against YNAB's own material is
its own piece of work and is tracked separately.

Whether the live plan should ever be written to directly, versus always via
a previewed scratch plan, is a design decision for the delivery ticket.
