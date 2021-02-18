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
    public class GameMove : ControllerBase
    {
        [HttpGet("{id}/{playerId}/{pieceId}")]
        public HttpResponseData Get(long id, int playerId, int pieceId)
        {
            var result = Backend.Move(id, playerId, pieceId);
            return new HttpResponseData(result.result ? "Moved!" : "Failed to move", result.gameState);
        }
    }
}
