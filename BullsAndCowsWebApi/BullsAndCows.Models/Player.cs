namespace BullsAndCows.Models
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class Player 
    {
        private ICollection<Game> games;

        private ICollection<Notification> notifications;
        public int Id { get; set; }

        public string Name { get; set; }

        //[Required ]
        //public DateTime RegisterDate { get; set; }

        public int WonGames { get; set; }

        public int LostGames { get; set; }

        public int Rank { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Game> Games
        {
            get
            {
                return this.games;
            }

            set
            {
                this.games = value;
            }
        }
         public virtual ICollection<Notification> Notifications
        {
            get
            {
                return this.notifications;
            }

            set
            {
                this.notifications = value;
            }
        }

        public Player()
        {
            this.games = new HashSet<Game>();
            this.notifications = new HashSet<Notification>();
        }
        //public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string authenticationType)
        //{
        //    // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        //    var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
        //    // Add custom user claims here
        //    return userIdentity;
        //}
    }
}