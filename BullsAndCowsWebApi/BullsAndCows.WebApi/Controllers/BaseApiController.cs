namespace BullsAndCows.WebApi.Controllers
{
    using System;
    using System.Web.Http;

    using BullsAndCows.Data;

    // [Authorize]
    public abstract class BaseApiController : ApiController
    {
        protected static Random Rand;

        protected IBullsAndCowsData Data;

        static BaseApiController()
        {
            Rand = new Random();
        }

        protected BaseApiController(IBullsAndCowsData data)
        {
            this.Data = data;
        }
    }
}