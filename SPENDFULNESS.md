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
* **YNAB Application:** Use Rule Three (**Roll with the Punches**). Dynamically shift funds from lower-priority categories to cover unexpected high-value needs intentionally.

---

## 3. Data Logging Taxonomy (YNAB Structural Mapping)

By redefining the axes around **Situation** rather than physical coordinates, you can map this framework directly into YNAB’s native architecture. This completely eliminates data loss from automated bank syncs.

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

### Raw Memo Field Format
Because your Category and Category Group handle the Situation and Behavior automatically, your memo field stays incredibly clean and easy to type:
```text
#familytime #aligned | Coffee at Train Museum
```

---

## 4. Analysing the Output

1. **Export Register Data:** Download your YNAB transaction history as a CSV file.
2. **Parse Layout:** Group your data by YNAB's native `Category Group` (Axis 2) and `Category` (Axis 3).
3. **Isolate Misalignment:** Filter your parsed text for the `#misaligned` tag to reveal which specific situations or environments consistently trick you into low-value target spending.
