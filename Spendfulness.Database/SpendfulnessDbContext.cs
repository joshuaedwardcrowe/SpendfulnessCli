using Microsoft.EntityFrameworkCore;
using Spendfulness.Database.Accounts;
using Spendfulness.Database.Commitments;
using Spendfulness.Database.Milestones;
using Spendfulness.Database.Settings;
using Spendfulness.Database.SpendingSamples;
using Spendfulness.Database.Users;

namespace Spendfulness.Database;

// TODO: Try to make this internal. Entity framework is a DEPENDENCY.
public class SpendfulnessDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<SettingType> SettingTypes { get; set; }
    public DbSet<Commitment> Commitments { get; set; }
    public DbSet<Milestone> Milestones { get; set; }
    
    
    // TODO: I'm still not entirely sure about this. 
    public DbSet<SpendingSample> SpendingSamples { get; set; }
    public DbSet<SpendingSampleMatchCriteria> SpendingSampleMatchCriteria { get; set; }
    public DbSet<SpendingSampleMatchCriteriaPrice> SpendingSampleMatchCriteriaPrices { get; set; }
    public DbSet<CustomAccountType> CustomAccountTypes { get; set; }
    public DbSet<CustomAccountAttributes> CustomAccountAttributes { get; set; }
    
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