using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsAndCows.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public DateTime DateCreated { get; set; }

        public NotificationType Type { get; set; }

        public NotificationState State { get; set; }
        
        public int PlayerId { get; set; }
        
        public virtual Player Player { get; set; }

        public int GameId { get; set; }

        public virtual Game Game { get; set; }
    }
}