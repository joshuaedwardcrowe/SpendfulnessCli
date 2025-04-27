# YnabCli
A CLI tool for YNAB.

## Getting Started
To get started, you're going to need to:

### Establish the Database
```text
/database create
```

### Create a User
```text
/user create --user-name Joshua
```

This user iwll be automatically set as active. If you have created multiple users, you can switch them:
```text
/user switch --user-name Simon
```

### Add YNAB API Key
This key is neccessary to pull data from your YNAB set up.

```text
/settings create --name YnabApiKey --value AJsokoaAoj_ajsiiAIejenias
```

YNAB CLI will automatically use the default, or first, budget it finds.