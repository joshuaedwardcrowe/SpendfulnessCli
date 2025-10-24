using Cli.Spendfulness.Database.Accounts;
using Cli.Spendfulness.Database.Commitments;
using Cli.Spendfulness.Database.Milestones;
using Cli.Spendfulness.Database.Settings;
using Cli.Spendfulness.Database.SpendingSamples;
using Cli.Spendfulness.Database.Users;
using Microsoft.EntityFrameworkCore;

namespace Cli.Spendfulness.Database;

public class YnabCliDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<SettingType> SettingTypes { get; set; }
    public DbSet<Commitment> Commitments { get; set; }
    public DbSet<Milestone> Milestones { get; set; }
    
    
    public DbSet<SpendingSample> SpendingSamples { get; set; }
    public DbSet<SpendingSampleMatchCriteria> SpendingSampleMatchCriteria { get; set; }
    public DbSet<SpendingSampleMatchCriteriaPrice> SpendingSampleMatchCriteriaPrices { get; set; }
    public DbSet<CustomAccountType> CustomAccountTypes { get; set; }
    public DbSet<AccountAttributes> AccountAccountTypes { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var profileDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var cliDbPath = $"{profileDirectoryPath}//YnabCli.db";
        
        optionsBuilder.UseSqlite($"Data Source={cliDbPath}");
        // optionsBuilder.LogTo(Console.WriteLine);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<SettingType>()
            .HasData(new List<SettingType>
            {
                new()
                {
                    Id = 1,
                    Name = nameof(SettingTypeNames.YnabApiKey)
                }
            });
        
        modelBuilder
            .Entity<SpendingSampleMatchCriteria>()
            .Ignore(x => x.MostRecentPrice);

        modelBuilder
            .Entity<CustomAccountType>()
            .HasData(new List<CustomAccountType>
            {
                new()
                {
                    Id = 1,
                    Name = "Stocks and Shares ISA"
                }
            });
    }
}