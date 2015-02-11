namespace BullsAndCows.WebApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using BullsAndCows.Models;

    public class GameDetailsModel
    {
        public static Expression<Func<Game, GameDetailsModel>> FromGameRedPlayer
        {
            get
            {
                return
                    game =>
                    new GameDetailsModel
                        {
                            Id = game.Id, 
                            Name = game.Name, 
                            Red = game.RedPlayer.Name, 
                            Blue = game.BluePlayer.Name, 
                            DateCreated = game.DateCreated, 
                            YourNumber = game.RedPlayerNumber, 
                            YourGuesses =
                                game.Guesses.AsQueryable()
                                .Select(GuessModel.FromGuess)
                                .Where(g => g.UserId == game.RedPlayer.UserId)
                                .OrderByDescending(gu => gu.DateMade), 
                            OpponentGuesses =
                                game.Guesses.AsQueryable()
                                .Select(GuessModel.FromGuess)
                                .Where(g => g.UserId == game.BluePlayer.UserId)
                                .OrderByDescending(gu => gu.DateMade), 
                            YourColor = "Red", 
                            GameState = game.GameState.ToString()
                        };
            }
        }

        public static Expression<Func<Game, GameDetailsModel>> FromGameBluePlayer
        {
            get
            {
                return
                    game =>
                    new GameDetailsModel
                        {
                            Id = game.Id, 
                            Name = game.Name, 
                            Red = game.RedPlayer.Name, 
                            Blue = game.BluePlayer.Name, 
                            DateCreated = game.DateCreated, 
                            YourNumber = game.BluePlayerNumber, 
                            YourGuesses =
                                game.Guesses.AsQueryable()
                                .Select(GuessModel.FromGuess)
                                .Where(g => g.UserId == game.BluePlayer.UserId)
                                .OrderByDescending(gu => gu.DateMade), 
                            OpponentGuesses =
                                game.Guesses.AsQueryable()
                                .Select(GuessModel.FromGuess)
                                .Where(g => g.UserId == game.RedPlayer.UserId)
                                .OrderByDescending(gu => gu.DateMade), 
                            YourColor = "Blue", 
                            GameState = game.GameState.ToString()
                        };
            }
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Red { get; set; }

        public string Blue { get; set; }

        public DateTime DateCreated { get; set; }

        public string YourNumber { get; set; }

        public string YourColor { get; set; }

        public string GameState { get; set; }

        public IEnumerable<GuessModel> YourGuesses { get; set; }

        public IEnumerable<GuessModel> OpponentGuesses { get; set; }
    }
}