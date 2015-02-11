namespace BullsAndCows.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Http;

    using BullsAndCows.Data;
    using BullsAndCows.Models;
    using BullsAndCows.WebApi.Models;

    using Microsoft.AspNet.Identity;

    public class GamesController : BaseApiController
    {
        private const int DefaultPageSize = 10;

        private const int MaximumNumberOfDifferentDigits = 8;

        private const int MaximumNumberOfBulls = 4;

        public GamesController()
            : this(new BullsAndCowsData(new BullsAndCowsDbContext()))
        {
        }

        public GamesController(IBullsAndCowsData data)
            : base(data)
        {
        }

        [Authorize]
        [HttpPost]
        public IHttpActionResult Guess(int gameId, [FromBody] string number)
        {
            var game = this.Data.Games.Find(gameId);
            var player = this.GetAuthenticatedPlayer();
            if (game == null)
            {
                return this.BadRequest("Such game does not exist!");
            }

            if (!(game.RedPlayerId == player.Id || game.BluePlayerId == player.Id))
            {
                return this.BadRequest("You are not part of this game!");
            }

            if (game.GameState == GameState.WaitingForOpponent)
            {
                return this.BadRequest("The game has not yet started. Waiting for blue player!");
            }

            if (game.GameState == GameState.GameOver)
            {
                return this.StatusCode(HttpStatusCode.Forbidden);
            }

            Guess guess;

            // TODO FUCKIN REFACTOR!!!!
            if (game.RedPlayerId == player.Id && game.GameState == GameState.RedInTurn)
            {
                var bulls = this.GetNumberOfBulls(number, game.BluePlayerNumber);
                var cows = MaximumNumberOfDifferentDigits
                           - this.GetNumberOfDifferentDigits(number, game.BluePlayerNumber) - bulls;

                guess = new Guess
                            {
                                Bulls = bulls, 
                                Cows = cows, 
                                GameId = gameId, 
                                GuessNumber = number, 
                                DateCreated = DateTime.Now, 
                                PlayerId = player.Id
                            };

                if (bulls == MaximumNumberOfBulls)
                {
                    game.GameState = GameState.GameOver;

                    var winnerNotification = NotificationsController.SendNotification(
                        game, 
                        game.RedPlayer.Id, 
                        NotificationType.GameWon);
                    var loserNotification = NotificationsController.SendNotification(
                        game, 
                        game.BluePlayer.Id, 
                        NotificationType.GameLost);

                    game.RedPlayer.WonGames += 1;
                    game.BluePlayer.LostGames += 1;
                    this.UpdateUserRank(game.RedPlayer);
                    this.UpdateUserRank(game.BluePlayer);
                    this.Data.Notifications.Add(winnerNotification);
                    this.Data.Notifications.Add(loserNotification);
                    this.Data.SaveChanges();
                }
                else
                {
                    game.GameState = GameState.BlueInTurn;
                    var notification = NotificationsController.SendNotification(
                        game, 
                        game.BluePlayer.Id, 
                        NotificationType.YourTurn);
                    this.Data.Notifications.Add(notification);
                    this.Data.SaveChanges();
                }

                this.Data.Guesses.Add(guess);
                this.Data.SaveChanges();
            }
            else if (game.BluePlayerId == player.Id && game.GameState == GameState.BlueInTurn)
            {
                var bulls = this.GetNumberOfBulls(number, game.RedPlayerNumber);
                var cows = MaximumNumberOfDifferentDigits
                           - this.GetNumberOfDifferentDigits(number, game.RedPlayerNumber) - bulls;

                guess = new Guess
                            {
                                Bulls = bulls, 
                                Cows = cows, 
                                GameId = gameId, 
                                GuessNumber = number, 
                                DateCreated = DateTime.Now, 
                                PlayerId = player.Id
                            };

                if (bulls == MaximumNumberOfBulls)
                {
                    game.GameState = GameState.GameOver;

                    var loserNotification = NotificationsController.SendNotification(
                        game, 
                        game.RedPlayer.Id, 
                        NotificationType.GameLost);
                    var winnerNotification = NotificationsController.SendNotification(
                        game, 
                        game.BluePlayer.Id, 
                        NotificationType.GameWon);

                    game.BluePlayer.WonGames += 1;
                    game.RedPlayer.LostGames += 1;
                    this.UpdateUserRank(game.RedPlayer);
                    this.UpdateUserRank(game.BluePlayer);
                    this.Data.Notifications.Add(winnerNotification);
                    this.Data.Notifications.Add(loserNotification);
                    this.Data.SaveChanges();
                }
                else
                {
                    game.GameState = GameState.RedInTurn;
                    var notification = NotificationsController.SendNotification(
                        game, 
                        game.RedPlayer.Id, 
                        NotificationType.YourTurn);
                    this.Data.Notifications.Add(notification);
                    this.Data.SaveChanges();
                }

                this.Data.Guesses.Add(guess);
                this.Data.SaveChanges();
            }
            else
            {
                return this.BadRequest("It's not your turn!");
            }

            var guessModel = this.Data.Guesses.All().Where(g => g.Id == guess.Id).Select(GuessModel.FromGuess);
            return this.Ok(guessModel);
        }

        [Authorize]
        [HttpGet]
        public IHttpActionResult GetByID(int id)
        {
            var game = this.Data.Games.Find(id);
            if (game == null)
            {
                return this.BadRequest("Such game does not exist!");
            }

            var player = this.GetAuthenticatedPlayer();
            if (!(game.RedPlayerId == player.Id || game.BluePlayerId == player.Id))
            {
                return this.BadRequest("You are not part of this game!");
            }

            if (game.GameState == GameState.WaitingForOpponent)
            {
                return this.BadRequest("The game has not yet started. Waiting for blue player!");
            }

            if (game.GameState == GameState.GameOver)
            {
                return this.StatusCode(HttpStatusCode.Forbidden);
            }

            var gameDetails =
                this.Data.Games.All()
                    .Where(x => x.Id == id)
                    .Select(
                        game.RedPlayer.Id == player.Id
                            ? GameDetailsModel.FromGameRedPlayer
                            : GameDetailsModel.FromGameBluePlayer)
                    .FirstOrDefault();

            return this.Ok(gameDetails);
        }

        [HttpGet]
        public IHttpActionResult GetByPage(int page)
        {
            ICollection<GameCreatedModel> games;
            if (this.User.Identity.IsAuthenticated)
            {
                games = this.GetGamesByPage(page, true);
            }
            else
            {
                games = this.GetGamesByPage(page, false);
            }

            return this.Ok(games);
        }

        [Authorize]
        [HttpPost]
        public IHttpActionResult JoinGame(int id, [FromBody] string number)
        {
            var game = this.Data.Games.Find(id);
            var player = this.GetAuthenticatedPlayer();
            if (game == null)
            {
                return this.BadRequest("Such game does not exist!");
            }

            if (game.GameState != GameState.WaitingForOpponent)
            {
                return this.BadRequest("The game is not available for joining!");
            }

            if (!this.IsNumberValid(number))
            {
                return this.BadRequest("Invalid number! Please enter a 4 digit number with non-repeating digits.");
            }

            game.BluePlayer = player;
            game.BluePlayerNumber = number;
            game.GameState = Rand.Next(0, 2) == 1 ? GameState.RedInTurn : GameState.BlueInTurn;
            this.Data.SaveChanges();
            var gameModel =
                this.Data.Games.All().Where(g => g.Id == id).Select(GameCreatedModel.FromGame).FirstOrDefault();
            return this.Ok(gameModel);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            ICollection<GameCreatedModel> games;
            if (this.User.Identity.IsAuthenticated)
            {
                games = this.GetGamesByPage(0, true);
            }
            else
            {
                games = this.GetGamesByPage(0, false);
            }

            return this.Ok(games);
        }

        [Authorize]
        [HttpPost]
        public IHttpActionResult Create([FromBody] GameCreationInputModel inputModel)
        {
            var number = inputModel.Number;
            var name = inputModel.Name;
            if (!this.IsNumberValid(number))
            {
                return
                    this.BadRequest(
                        "Input number is not in the correct format! Please enter a 4 digit, non repeating sequence!");
            }

            var userId = this.User.Identity.GetUserId();
            var player = this.GetPlayerByUserId(userId);

            var game = new Game
                           {
                               DateCreated = DateTime.Now, 
                               Name = name, 
                               RedPlayer = player, 
                               RedPlayerId = player.Id, 
                               GameState = GameState.WaitingForOpponent, 
                               RedPlayerNumber = number
                           };
            this.Data.Games.Add(game);
            this.Data.SaveChanges();

            var gameModel = new GameCreatedModel
                                {
                                    Id = game.Id, 
                                    Name = game.Name, 
                                    Red = game.RedPlayer.Name, 
                                    Blue = "No blue player yet", 
                                    DateCreated = game.DateCreated, 
                                    GameState = game.GameState.ToString()
                                };
            return this.Ok(gameModel);
        }

        private int GetNumberOfBulls(string secretNumber, string guessNumber)
        {
            if (secretNumber.Length != guessNumber.Length)
            {
                throw new ArgumentException("Lengths of the two numbers dont match!");
            }

            var bulls = 0;
            for (var i = 0; i < secretNumber.Length; i++)
            {
                if (secretNumber[i] == guessNumber[i])
                {
                    bulls++;
                }
            }

            return bulls;
        }

        private int GetNumberOfDifferentDigits(string secretNumber, string guessNumber)
        {
            if (secretNumber.Length != guessNumber.Length)
            {
                throw new ArgumentException("Lengths of the two numbers dont match!");
            }

            var digits = new HashSet<char>();

            for (var i = 0; i < secretNumber.Length; i++)
            {
                digits.Add(secretNumber[i]);
                digits.Add(guessNumber[i]);
            }

            return digits.Count;
        }

        private ICollection<GameCreatedModel> GetGamesByPage(int page, bool isUserAuthenticated)
        {
            IQueryable<Game> games;
            if (isUserAuthenticated)
            {
                var player = this.GetAuthenticatedPlayer();
                games =
                    this.Data.Games.All()
                        .Where(
                            g =>
                            (g.GameState == GameState.WaitingForOpponent)
                            || (((g.BluePlayerId == player.Id) ^ (g.RedPlayerId == player.Id)) && g.BluePlayerId != null
                                && g.GameState != GameState.GameOver));

                    // No ordering by game state because we take only the games with state that is 'Waiting for opponent'
            }
            else
            {
                games = this.Data.Games.All().Where(g => g.GameState == GameState.WaitingForOpponent);
            }

            var queriedGames =
                games.OrderByDescending(g => g.GameState)
                    .ThenBy(g => g.Name)
                    .ThenBy(g => g.DateCreated)
                    .ThenBy(g => g.RedPlayer.Name)
                    .Skip(page * DefaultPageSize)
                    .Take(DefaultPageSize)
                    .Select(GameCreatedModel.FromGame)
                    .ToList();

            return queriedGames;
        }

        private Player GetPlayerByUserId(string userId)
        {
            var player = this.Data.Players.All().FirstOrDefault(p => p.UserId == userId);
            if (player == null)
            {
                throw new ArgumentException("Such player does not exist!");
            }

            return player;
        }

        private Player GetAuthenticatedPlayer()
        {
            var playerId = this.User.Identity.GetUserId();
            var player = this.GetPlayerByUserId(playerId);
            return player;
        }

        private bool IsNumberValid(string number)
        {
            if (number.Length != MaximumNumberOfBulls)
            {
                return false;
            }

            for (var i = 0; i < number.Length; i++)
            {
                for (var j = i + 1; j < number.Length; j++)
                {
                    if (number[i] == number[j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void UpdateUserRank(Player player)
        {
            var rank = 100 * player.WonGames + 15 * player.LostGames;
            player.Rank = rank;
            this.Data.Players.Update(player);
            this.Data.SaveChanges();
        }
    }
}