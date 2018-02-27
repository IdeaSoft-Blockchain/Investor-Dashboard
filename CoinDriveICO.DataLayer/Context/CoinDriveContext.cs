using CoinDriveICO.DataLayer.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CoinDriveICO.DataLayer.Context
{
    public class CoinDriveContext : IdentityDbContext<AppUser,AppRole, int, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
    {
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<InnerTransaction> InnerTransactions { get; set; }
        public DbSet<AffiliatePayoff> AffiliatePayoffs { get; set; }
        public CoinDriveContext(DbContextOptions<CoinDriveContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Transaction>().Property(x => x.Amount).HasColumnType("decimal(38,20)");
            builder.Entity<InnerTransaction>().Property(x => x.FromValue).HasColumnType("decimal(38,20)");
            builder.Entity<InnerTransaction>().Property(x => x.ToValue).HasColumnType("decimal(38,20)");
            builder.Entity<InnerTransaction>().Property(x => x.TypeToTokenConversationRate).HasColumnType("decimal(38,20)");
            builder.Entity<InnerTransaction>().Property(x => x.TokenMultiplier).HasColumnType("decimal(38,20)");
            builder.Entity<AppUser>().Property(x => x.Balance).HasColumnType("decimal(38,20)");
            builder.Entity<AffiliatePayoff>().Property(x => x.AffiliatePayoffMultiplier).HasColumnType("decimal(38,20)");
            builder.Entity<AffiliatePayoff>().Property(x => x.TransactionValue).HasColumnType("decimal(38,20)");
            builder.Entity<AffiliatePayoff>().Property(x => x.PayoffValue).HasColumnType("decimal(38,20)");
            builder.Entity<AppUser>().HasIndex(x => x.Id).IsUnique(true);
            builder.Entity<AffiliatePayoff>().HasOne(x => x.InnerTransaction)
                .WithMany(x => x.AssociatedAffiliatePayoffs).HasForeignKey(x => x.InnerTransactionId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<AffiliatePayoff>().HasOne(x => x.AffiliateUser)
                .WithMany(x => x.AssociatedDestinationAffiliatePayoffs).HasForeignKey(x => x.AffiliateUserId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<AffiliatePayoff>().HasOne(x => x.PayingUser)
                .WithMany(x => x.AssociatedSourceAffiliatePayoffs).HasForeignKey(x => x.PayingUserId).OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(builder);
        }
    }

    public class CoinDriveContextFactory : IDesignTimeDbContextFactory<CoinDriveContext>
    {
        public CoinDriveContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<CoinDriveContext>();
            options.UseSqlServer("Data Source=95.211.94.106;Initial Catalog=CoinDriveICO;User ID=SA;Password=Viennus7");
            var context = new CoinDriveContext(options.Options);
            return context;
        }
    }
}