using CloveceApiServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace CloveceApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameInfo : ControllerBase
    {
        [HttpGet]
        public HttpResponseData Get()
        {
            return new HttpResponseData("No id provided");
        }

        [HttpGet("{id}")]
        public HttpResponseData Get(long id)
        {
            GameModel game = Backend.GetGame(id);
            if (game is null)
            {
                return new HttpResponseData("No game found!");
            }
            return new HttpResponseData("Game found", game);
        }

        [HttpPost]
        public HttpResponseData Post()
        {
            long gameId = Backend.CreateGame();
            return new HttpResponseData("Game created", gameId);
        }
    }
}
