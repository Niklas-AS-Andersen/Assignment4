using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System.Collections.Generic;
using Assignment4.Core;

namespace Assignment4.Entities
{
    public class KanbanContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }

        public KanbanContext(DbContextOptions<KanbanContext> options) : base(options) { }
        
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Task>(entity =>
            {
                entity.Property(e => e.State)
                .HasConversion(
                    v => v.ToString(),
                    v => (State)Enum.Parse(typeof(State), v));
                entity.HasMany(e => e.Tags).WithMany(e => e.Tasks);
                });
                
             modelBuilder.Entity<Tag>(entity =>
            {
                        entity.HasIndex(t => t.Name)
                        .IsUnique();
            });
            

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.HasIndex(e => e.Email)
                    .IsUnique();
                entity.HasMany(e => e.Tasks);
            });

            
        }
    }
}
