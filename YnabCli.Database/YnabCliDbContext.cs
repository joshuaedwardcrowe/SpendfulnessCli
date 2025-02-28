using Microsoft.EntityFrameworkCore;
using YnabCli.Database.Accounts;
using YnabCli.Database.Commitments;
using YnabCli.Database.Milestones;
using YnabCli.Database.Settings;
using YnabCli.Database.Users;

namespace YnabCli.Database;

public class YnabCliDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<SettingType> SettingTypes { get; set; }
    public DbSet<Commitment> Commitments { get; set; }
    public DbSet<Milestone> Milestones { get; set; }
    public DbSet<CustomAccountType> CustomAccountTypes { get; set; }
    public DbSet<AccountCustomAccountType> AccountAccountTypes { get; set; }
    
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