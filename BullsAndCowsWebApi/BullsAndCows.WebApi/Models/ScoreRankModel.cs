namespace BullsAndCows.WebApi.Models
{
    using System;
    using System.Linq.Expressions;

    using BullsAndCows.Models;

    public class ScoreRankModel
    {
        public static Expression<Func<Player, ScoreRankModel>> FromPlayer
        {
            get
            {
                return player => new ScoreRankModel { Username = player.Name, Rank = player.Rank };
            }
        }

        public string Username { get; set; }

        public int Rank { get; set; }
    }
}