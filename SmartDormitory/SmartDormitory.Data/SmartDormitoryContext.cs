﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartDormitory.Data.Models;
using SmartDormitory.Data.Models.Contracts;
using System;
using System.Linq;

namespace SmartDormitory.App.Data
{
    public class SmartDormitoryContext : IdentityDbContext<User>
    {
        public SmartDormitoryContext()
        {

        }
        public SmartDormitoryContext(DbContextOptions<SmartDormitoryContext> options)
            : base(options)
        {
        }

        public DbSet<Sensor> Sensors { get; set; }

        public DbSet<IcbSensor> IcbSensors { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasMany(u => u.Sensors)
                .WithOne(s => s.Owner)
                .HasForeignKey(u => u.OwnerId);

            builder.Entity<IcbSensor>()
                .HasMany(x => x.Sensors)
                .WithOne(s => s.IcbSensor)
                .HasForeignKey(s => s.IcbSensorId);

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges();
        }

        private void ApplyAuditInfoRules()
        {
            var newlyCreatedEntities = this.ChangeTracker
                .Entries()
                .Where(e => e.Entity is IAuditable && ((e.State == EntityState.Added) || (e.State == EntityState.Modified)));

            foreach (var entry in newlyCreatedEntities)
            {
                var entity = (IAuditable)entry.Entity;

                if (entry.State == EntityState.Added && entity.CreatedOn == null)
                {
                    entity.CreatedOn = DateTime.Now;
                }
                else
                {
                    entity.ModifiedOn = DateTime.Now;
                }
            }
        }
    }
}
