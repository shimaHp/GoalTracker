﻿
using GoalTracker.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GoalTracker.Infrastructure.Persistence;

public class GoalTrackerDbContext(DbContextOptions<GoalTrackerDbContext> options) : IdentityDbContext<User>(options)
{
    

        public DbSet<Goal> Goals { get; set; }
    internal DbSet<WorkItem> WorkItems { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Goal Configuration
        modelBuilder.Entity<Goal>(entity =>
        {
            // Title is required, has max length, and must be unique
            entity.Property(g => g.Title)
                  .IsRequired()
                  .HasMaxLength(200);

            // Add unique constraint on Title
            entity.HasIndex(g => g.Title)
                  .IsUnique();

            // Description is optional and has max length
            entity.Property(g => g.Description)
                  .HasMaxLength(1000);

            // CreatedDate defaults to current time
            entity.Property(g => g.CreatedDate)
                  .HasDefaultValueSql("GETDATE()");

            // Index for faster querying
            entity.HasIndex(g => g.CreatedDate);
        });

        // WorkItem Configuration
        modelBuilder.Entity<WorkItem>(entity =>
        {
            // Title is required and has max length
            entity.Property(w => w.Title)
                  .IsRequired()
                  .HasMaxLength(200);

            // Description is optional and has max length
            entity.Property(w => w.Description)
                  .HasMaxLength(1000);

            // CreatedDate defaults to current time
            entity.Property(w => w.CreatedDate)
                  .HasDefaultValueSql("GETDATE()");

            // Creator relationship
            entity.HasOne(w => w.Creator)
                  .WithMany()
                  .HasForeignKey(w => w.CreatorId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Assignee relationship
            entity.HasOne(w => w.Assignee)
                  .WithMany()
                  .HasForeignKey(w => w.AssigneeId)
                  .OnDelete(DeleteBehavior.Restrict);

            // LastUpdatedBy relationship
            entity.HasOne(w => w.LastUpdatedBy)
                  .WithMany()
                  .HasForeignKey(w => w.LastUpdatedById)
                  .OnDelete(DeleteBehavior.Restrict);




            // Configure relationship with Goal
            entity.HasOne(w => w.Goal)
                  .WithMany(g => g.WorkItems)
                  .HasForeignKey(w => w.GoalId)
                  .OnDelete(DeleteBehavior.Cascade); // Deletes WorkItems if Goal is deleted

            // Index for faster querying
            entity.HasIndex(w => w.CreatedDate);
            entity.HasIndex(w => w.GoalId);
        });

        // Enum to string conversion for Status and Priority
        modelBuilder.Entity<Goal>()
            .Property(g => g.Status)
            .HasConversion<int>();

        modelBuilder.Entity<Goal>()
            .Property(g => g.Priority)
            .HasConversion<int>();

        modelBuilder.Entity<WorkItem>()
            .Property(w => w.Status)
            .HasConversion<int>();

    }
}

