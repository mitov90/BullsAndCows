namespace BullsAndCows.WebApi.Models
{
    using System;
    using System.Linq.Expressions;

    using BullsAndCows.Models;

    public class GuessModel
    {
        public static Expression<Func<Guess, GuessModel>> FromGuess
        {
            get
            {
                return
                    guess =>
                    new GuessModel
                        {
                            Id = guess.Id, 
                            UserId = guess.Player.UserId, 
                            Username = guess.Player.Name, 
                            GameId = guess.GameId, 
                            Number = guess.GuessNumber, 
                            DateMade = guess.DateCreated, 
                            CowsCount = guess.Cows, 
                            BullsCount = guess.Bulls
                        };
            }
        }

        public int Id { get; set; }

        public string UserId { get; set; }

        public string Username { get; set; }

        public int GameId { get; set; }

        public string Number { get; set; }

        public DateTime DateMade { get; set; }

        public int CowsCount { get; set; }

        public int BullsCount { get; set; }
    }
}