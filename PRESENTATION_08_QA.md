## 8. Q&A
**Duration: 2 minutes**

### Common Questions

**Q: Why not just use System.CommandLine?**
- **A:** System.CommandLine is great for single-command CLIs. This framework is for interactive, continuous-input CLIs with command pipelines and reusable patterns.

**Q: Is this production-ready?**
- **A:** Yes! It's running in production for SpendfulnessCli (financial management tool). Comprehensive tests, ADR-documented decisions, handles edge cases.

**Q: How do I get started?**
- **A:** 
  1. Clone the repo: `git clone https://github.com/KitCli/KitCli.Spendfulness`
  2. Explore the `Cli.*` projects (the framework)
  3. Look at `SpendfulnessCli.*` for examples
  4. Build your first command!

**Q: What about testing?**
- **A:** Easy! Commands are records. Handlers are classes. Mock dependencies, test handlers. Framework provides `ICliCommandOutcomeIo` for integration tests.

**Q: Can I extend the argument type system?**
- **A:** Yes! Implement `IConsoleInstructionArgumentBuilder` for your type and register it. Framework automatically uses it.

---

