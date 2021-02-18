using CloveceApiServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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
        public string Post()
        {
            Backend.CreateGame();
            return "Game created";
        }
    }
}
