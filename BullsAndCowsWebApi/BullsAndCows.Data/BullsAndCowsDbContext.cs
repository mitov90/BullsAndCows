using BullsAndCows.Data.Migrations;
using BullsAndCows.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BullsAndCows.Data
{
    //public class BullsAndCowsDbContext
    //{
    //}
    public class BullsAndCowsDbContext : IdentityDbContext<User>
    {
        public BullsAndCowsDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
           // Database.SetInitializer(new DropCreateDatabaseAlways<BullsAndCowsDbContext>()); 
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BullsAndCowsDbContext, Configuration>());
        }
        
        public static BullsAndCowsDbContext Create()
        {
            return new BullsAndCowsDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                        .HasRequired(c => c.RedPlayer)
                        .WithMany()
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Game>()
                        .HasOptional(s => s.BluePlayer)
                        .WithMany()
                        .WillCascadeOnDelete(false);
            base.OnModelCreating(modelBuilder);
        }

        public IDbSet<Game> Games { get; set; }

        public IDbSet<Player> Players { get; set; }

        public IDbSet<Notification> Notifications { get; set; }

        public IDbSet<Guess> Guesses { get; set; }
    }
}