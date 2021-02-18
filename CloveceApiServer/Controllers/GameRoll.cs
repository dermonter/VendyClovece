using CloveceApiServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloveceApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameRoll : ControllerBase
    {
        [HttpGet("{id}/{playerId}")]
        public HttpResponseData Get(long id, int playerId)
        {
            int rolled = Backend.Roll(id, playerId);
            if (rolled == -1)
                return new HttpResponseData("Failed to roll.");
            return new HttpResponseData("Rolled!", rolled);
        }

    }
}
