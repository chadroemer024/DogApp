using DogApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DogApp.Data
{
    public class DataContext : DbContext
    {
       public DataContext(DbContextOptions<DataContext> options): base(options)
        {

        }

        public DbSet<Dog> Dogs { get; set; }
        public DbSet<Person> People { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dog>()
                .HasOne<Person>(d => d.CurrentOwner)
                .WithMany(o => o.Dogs)
                .HasForeignKey(d => d.CurrentOwnerId);

        }
    }
}
