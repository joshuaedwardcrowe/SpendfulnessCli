# Behavioral Budgeting Framework: Mapping Spendfulness in YNAB

**Spendfulness** is YNAB’s core philosophy of aligning every financial decision directly with personal values and intentional living, moving personal finance away from restrictive, guilt-based tracking. 

Instead of asking, *"Do I have enough money for a haircut?"* spendfulness asks, *"Does spending this money on a haircut right now support how I want to live my life and show up to this interview?"*

This document outlines a multi-dimensional framework for tracking financial records in YNAB. It shifts personal finance from basic bookkeeping into behavioral psychology by cross-examining spending patterns across four distinct situational axes and integrating YNAB's core spendfulness philosophy.

---

## 1. The Four Axes of Financial Behavior

Analyze every transaction using these four core situational dimensions:

* **Axis 1: Financial Target (What)** — The specific granular item or service purchased (e.g., a coffee, a haircut).
* **Axis 2: Encouraged Behaviour (How)** — The operational trigger, structurally separating long-term routines from one-off events.
* **Axis 3: Situational Context (Situation)** — The environment, setting, or framing where the transaction occurred (e.g., being at a museum, on a business trip).
* **Axis 4: Underlying Motivation (Why)** — The deep life shift or emotional driver behind the outflow (e.g., job hunting, family bonding).

---

## 2. Integrating YNAB Spendfulness

Spendfulness acts as an active filtering mechanism. It forces you to cross-examine these axes *before* tapping your card, rather than just tracking the damage afterward.

### Aligning Target and Purpose (Axes 1 & 4)
* **The Concept:** Traditional systems only track the physical item. Spendfulness connects the item directly to your broader life goals.
* **YNAB Application:** Use the **Reflect** tab to audit past spending targets. Ensure historical outflows actually served your values rather than thoughtless impulses.

### Eliminating Cash-Register "Mental Math" (Axes 2 & 3)
* **The Concept:** Temptation peaks when you are embedded in a specific situational context. 
* **YNAB Application:** Check category balances *before* purchasing. YNAB transforms your budget into a dynamic **conscious choice map**, showing the immediate trade-offs of spending within that specific situation.

### Rolling with Behavioral Flexibility (Axis 2)
* **The Concept:** Sudden routine shifts or life events require immediate capital, which traditional budgets punish with guilt.
* **YNAB Application:** Ask [YNAB's fifth key question](https://www.ynab.com/guide/foundations-the-ynab-method) — *"What changes do I need to make, if any?"* Dynamically shift funds from lower-priority categories to cover unexpected high-value needs intentionally.

---

## 3. Data Logging Taxonomy (YNAB Structural Mapping)

By redefining the axes around **Situation** rather than physical coordinates, you can map this framework directly into YNAB’s native architecture. This completely eliminates data loss from automated bank syncs.

> **This mapping is an interpretation, not a YNAB requirement.** YNAB deliberately prescribes no structure — its own guidance is to turn spending priorities into categories, "whatever matters to YOU." Nothing below is imposed by the product, and nothing below is the only workable arrangement. What the axes *do* require is that Behaviour, Situation, Target and Why each land somewhere recoverable. Which YNAB field carries which axis is a choice. See [Choosing what the Category Group carries](#choosing-what-the-category-group-carries).

### Structural Mapping Example: The Museum Coffee

```text
Events (Category Group) 
└── Museums (Category)
    └── Transaction: "Coffee at Train Museum" (Memo)
```

### Framework Matrix

| Dimension | YNAB Structural Field | Architectural Role | Example Mapping |
| :--- | :--- | :--- | :--- |
| **Axis 2: Behaviour** | **Category Group** | High-level behavioral bucket | `Events` or `Routines` |
| **Axis 3: Situation** | **Category** | The specific environment/setting | `Museums` or `Commuting` |
| **Axis 1: Target** | **Memo Text** | The actual item purchased | `Coffee` or `Pastry` |
| **Axis 4: Why** | **Memo Tag** | The deep motivation driver | `#familytime` or `#networking` |
| **Spendful?** | **Memo Tag** | Evaluation of value alignment | `#aligned` or `#misaligned` |

### Choosing what the Category Group carries

The matrix above puts **Behaviour** (Axis 2) in the Category Group and **Why** (Axis 4) in a memo tag. That is one arrangement. The alternative — and the one this author's own budget moved to — is to put **Why** in the Category Group instead:

| | Behaviour-led (matrix above) | Value-led |
| :--- | :--- | :--- |
| **Category Group** | `Events`, `Routines` | `Community (I Am Loved)`, `Sanctuary (Am I Physically Safe)` |
| **Category** | `Museums`, `Commuting` | the situation or target, as before |
| **Memo tag** | `#familytime` carries the Why | the Why is already structural |

Neither is more correct. They trade against each other:

* **Behaviour-led** keeps the structure stable — routines and events don't change much — and keeps values in a tag, where they're easy to revise but easy to skip. Misalignment analysis then depends on tagging discipline at the point of sale.
* **Value-led** makes the Why structural, so every transaction inherits a value whether or not you remember to tag it, and the budget itself reads as a statement of priorities. The cost is that the structure now changes when your values do, which is a re-organisation rather than an edit — see [investigation 0001](docs/investigations/0001-values-axis-budget-reorganisation.md).

The `#aligned` / `#misaligned` tag stays useful under either. It records a judgement about a *specific* transaction, which no amount of structure can infer: putting a purchase in the right value group says what it was *for*, not whether it was worth it.

### Naming Categories: Value, Not Mechanism

A category name is only doing its job if it answers *what am I valuing here*, not *what kind of object or bill is this*. This is a subtler trap than picking the wrong axis, because a mechanism-named category can sit in exactly the right value group for years without anyone noticing it's hollow.

**The test:** would a values-based decision about this transaction be hidden by its current name? A bill named `Council Tax` says nothing about why it matters when you're deciding whether to move house or fight a valuation. Named for what it actually protects — the legal right to remain in your home without court action — the stakes are visible every time you look at the budget.

Three shapes this mistake takes:

* **Bills named after the bill.** `Mortgage`, `Home Insurance`, `Council Tax` name the instrument, not the value it buys. The fix asks what actually happens because of the payment: a mortgage buys equity in the home, not "a loan"; home insurance buys a replacement structure if disaster strikes, not "a policy."
* **Categories named after a device or product type.** A hair trimmer isn't "Portable Technology" — it's grooming. The device is the vehicle; the value is the styling choice it enables. Naming by device type hides that a razor and a laptop share nothing as *values*, even though both are "tech."
* **Categories named after a product shelf, not a need.** A generic toiletries category looks like one value because it's one aisle in a shop. It isn't. Toothpaste serves dental health. Body wash serves general hygiene. A styling product serves self-expression, because there's a choice being made that plain hygiene doesn't have. Splitting a shelf category across several value categories isn't inconsistency — it's the fix.

**A sharper test for hardware specifically:** before naming a piece of technology after the axis its *content* lives in, check whether that content already has its own category. A smart speaker used mainly for music doesn't automatically belong under an entertainment value just because music is one — if the streaming subscription is *already* a separate line item, the speaker itself is only buying "the home is equipped for it," which is a shelter-shaped value, not a second helping of the entertainment value the subscription already accounts for. Naming the same value twice under two categories is as much a failure as naming it under the wrong one.

This author's own re-pass through the Sanctuary axis, worked category by category:

| Old (mechanism) | New (value) | Why |
| :--- | :--- | :--- |
| Council Tax | Standing | The legal right to remain in the home, not the tax itself |
| Mortgage | Ownership | Buys equity in the sanctuary, not "a loan" |
| Water | Drinking | The base survival need, not "a utility" |
| Home Insurance | Replacement | What actually happens if the structure is lost |
| Electric | Power | Deliberately left broad — no single downstream use (fridge, lighting, charging, heating ignition) is *the* reason, so naming one would narrow dishonestly |
| Security | Security | Already names the concrete thing — there was no mechanism to strip away |
| Home Automation | *dissolved* | Was hiding two purchases: a smart speaker (music hardware, feeding a value the streaming subscription already owns) and smart bulbs (a lighting fixture). Neither is "automation" as a value. |
| Fixtures (absorbing the above hardware) | Setup | The word that actually stretches across furniture, lighting, and built-in audio — "Furnishings" failed (excludes bulbs), "Equipment" failed (excludes furniture) |
| Toiletries | *dissolved into Healthy's sub-categories* | Each product serves a distinct, specific health value — there was never one "toiletries" value to name |

Life Insurance is still open at time of writing: "Payout" names the mechanism (money moves) without the reason (the family keeps the home if the earner dies). Candidates on the table are "Continuity" and "Provision." Not settled — a reminder that this exercise doesn't always resolve in one pass.

### Raw Memo Field Format
Because your Category and Category Group handle the Situation and Behavior automatically, your memo field stays incredibly clean and easy to type:
```text
#familytime #aligned | Coffee at Train Museum
```

---

## 4. Analysing the Output

1. **Export Register Data:** Download your YNAB transaction history as a CSV file.
2. **Parse Layout:** Group your data by YNAB's native `Category Group` and `Category`. Which axis each one carries depends on the arrangement you chose above — Behaviour and Situation under the behaviour-led mapping, Why and Situation under the value-led one.
3. **Isolate Misalignment:** Filter your parsed text for the `#misaligned` tag to reveal which specific situations or environments consistently trick you into low-value target spending.
