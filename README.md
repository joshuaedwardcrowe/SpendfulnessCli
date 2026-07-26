# SpendfulnessCli

A personal CLI for reporting on and reasoning about YNAB spending —
built on [KitCli](https://github.com/KitCli/KitCli), talking to YNAB
via [YnabSharp](https://github.com/joshuaedwardcrowe/YnabSharp).

## Getting started

### 1. Create the database

```text
/database create
```

### 2. Create a user

```text
/user create --user-name Joshua
```

This user is automatically set as active. If you've created more than
one, switch between them:

```text
/user switch --user-name Simon
```

### 3. Add your YNAB API key

Settings are tied to whichever user is currently active, so this must
come *after* step 2 — the key you add here only applies to that user.
Get a personal access token from YNAB's own account settings, then:

```text
/settings create --name YnabApiKey --value <your-ynab-personal-access-token>
```

SpendfulnessCli uses the default (or first) budget it finds for the
active user.

## What you can do

A few of the reporting commands, once set up:

```text
/spare-money
/spare-money --add 50 --minus-savings true
/yearly-spending
/recurring-transactions
```

Every command's invocation name is derived automatically from its type
name (`SpareMoneyCliCommand` → `spare-money`, or the shorthand `sm`) —
it isn't declared or documented per-command, so if you're looking for
the full list, it's the command classes themselves: see
`SpendfulnessCli.Commands.Reporting`, `.Personalisation`, and
`.Organisation`.

## What's not built yet

Some real gaps, tracked rather than silently missing:

- No command writes anything back to YNAB — everything here is
  read-only reporting today ([#185](https://github.com/joshuaedwardcrowe/SpendfulnessCli/issues/185)).
- `/chat` exists but isn't actually connected to your YNAB data yet
  ([#181](https://github.com/joshuaedwardcrowe/SpendfulnessCli/issues/181)).
- `Splitwise`/`Splitter.Cli` are empty, unimplemented scaffolds
  ([#186](https://github.com/joshuaedwardcrowe/SpendfulnessCli/issues/186)).
