﻿using FreeLancing.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FreeLancing.Models.VMs;
using FreeLancing.Models.ApiModels;

namespace FreeLancing.Data
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }
        public DbSet<CustomTag> CustomTags { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<GlobalMessage> GlobalMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Bid>()
                .HasOne(a => a.Job)
                    .WithMany(b => b.JobBids)
                        .HasForeignKey(c => c.JobId)
                            .OnDelete(DeleteBehavior.Cascade);
            base.OnModelCreating(builder);
        }
    }
}
