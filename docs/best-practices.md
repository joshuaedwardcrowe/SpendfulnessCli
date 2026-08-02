1. Names can be simpiified in respect to their surrounding context. e.g. If you're got `_workflowContextRuns = []` in a `CliWorkflowContext`; it can just be simplified to `_runs`
2. => functions should start on a new line. No exceptions.
3. Repository methods that retrieve can both expect something to exist and not. If it must exist, it's a .Find. If it can not exist, it's a Get.