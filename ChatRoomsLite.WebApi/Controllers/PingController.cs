using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatRoomsLite.WebApi.Controllers;

public class PingController : BaseController
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Pong");
    }
    
    [HttpGet("auth")]
    [Authorize]
    public IActionResult GetAuth()
    {
        return Ok("Pong");
    }
}