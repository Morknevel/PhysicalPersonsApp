using Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal.Data;

public class PersonDbContext : DbContext
{
    public PersonDbContext()
    {
        
    }
    public PersonDbContext(DbContextOptions<PersonDbContext> options) : base(options)
    {
        
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=PhysicalPersons;Integrated Security=True;TrustServerCertificate=True;\n");
        }
    }
    public  DbSet<Person> Persons { get; set; }
    public DbSet<PhoneNumber> PhoneNumbers { get; set; }
    public DbSet<City> Cities { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Relationship>()
            .HasKey(r => new { r.PersonId, r.RelatedId });

        modelBuilder.Entity<Relationship>()
            .HasOne(r => r.Person)
            .WithMany(p => p.RelatedFrom)
            .HasForeignKey(r => r.PersonId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Relationship>()
            .HasOne(r => r.RelatedPerson)
            .WithMany(p => p.RelatedTo)
            .HasForeignKey(r => r.RelatedId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Relationship>()
            .Property(r => r.RelationType)
            .HasConversion<string>() 
            .IsRequired();
    }

}