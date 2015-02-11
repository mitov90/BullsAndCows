using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace BullsAndCows.Models
{
    public class Game
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DateCreated { get; set; }

        public GameState GameState { get; set; }

        public string RedPlayerNumber { get; set; }

        public string BluePlayerNumber { get; set; }

        public int RedPlayerId { get; set; }
        
        [ForeignKey("RedPlayerId")]
        public virtual Player RedPlayer { get; set; }

        public int? BluePlayerId { get; set; }

        [ForeignKey("BluePlayerId")]
        public virtual Player BluePlayer { get; set; }

        private ICollection<Guess> guesses;

        public Game()
        {
            this.guesses = new HashSet<Guess>();
        }

        public virtual ICollection<Guess> Guesses
        {
            get
            {
                return this.guesses;
            }
            set
            {
                this.guesses = value;
            }
        }
    }
}