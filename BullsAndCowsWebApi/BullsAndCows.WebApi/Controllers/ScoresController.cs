namespace BullsAndCows.WebApi.Controllers
{
    using System.Linq;
    using System.Web.Http;

    using BullsAndCows.Data;
    using BullsAndCows.WebApi.Models;

    public class ScoresController : BaseApiController
    {
        private const int DefaultPlayersRankingCount = 10;

        public ScoresController()
            : this(new BullsAndCowsData(new BullsAndCowsDbContext()))
        {
        }

        public ScoresController(IBullsAndCowsData data)
            : base(data)
        {
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var scores =
                this.Data.Players.All()
                    .Select(ScoreRankModel.FromPlayer)
                    .OrderByDescending(x => x.Rank)
                    .ThenBy(x => x.Username)
                    .Take(DefaultPlayersRankingCount);

            return this.Ok(scores);
        }
    }
}