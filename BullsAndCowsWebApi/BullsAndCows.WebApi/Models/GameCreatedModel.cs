namespace BullsAndCows.WebApi.Models
{
    using System;
    using System.Linq.Expressions;

    using BullsAndCows.Models;

    public class GameCreatedModel
    {
        public static Expression<Func<Game, GameCreatedModel>> FromGame
        {
            get
            {
                return
                    game =>
                    new GameCreatedModel
                        {
                            Id = game.Id, 
                            Name = game.Name, 
                            Red = game.RedPlayer.Name, 
                            Blue = game.BluePlayer.Name ?? "No blue player yet", 
                            DateCreated = game.DateCreated, 
                            GameState = game.GameState.ToString()
                        };
            }
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Red { get; set; }

        public string Blue { get; set; }

        public string GameState { get; set; }

        public DateTime DateCreated { get; set; }
    }
}