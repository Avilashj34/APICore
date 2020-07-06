using Microsoft.EntityFrameworkCore;
using APICoreExample.Model;

namespace APICoreExample.Model
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<WorkInformation> WorkInformation { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Customer>(entity => {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
                entity.Property(c => c.FirstName).HasMaxLength(10).IsRequired();
                entity.Property(c => c.Age).HasMaxLength(2).IsRequired();
            });
            builder.Entity<WorkInformation>(entity => {
                entity.HasKey(w => w.Id);
                entity.Property(w => w.Id).ValueGeneratedOnAdd();
                entity.Property(w => w.CompanyName).HasMaxLength(20).IsRequired();
                entity.Property(w => w.Experience).HasMaxLength(2).IsRequired();
                entity.Property(w => w.Position).HasMaxLength(15).IsRequired();
            });
            //Configuring the One to Many relation 
            //builder.Entity<WorkInformation>().HasOne(w => w.Customer).WithMany(c => c.WorkInformation)
              //  .HasForeignKey(w => w.CustomerId).OnDelete(DeleteBehavior.Cascade).IsRequired();
            base.OnModelCreating(builder);
        }
    }
}
