using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BullsAndCows.Models
{
    public class Guess
    {
        public int Id { get; set; }

        public string GuessNumber { get; set; }

        public DateTime DateCreated { get; set; }

        public int Cows { get; set; }

        public int Bulls { get; set; }

        public int PlayerId { get; set; }

        public virtual Player Player { get; set; }

        public int GameId { get; set; }

        public virtual Game Game { get; set; }
    }
}