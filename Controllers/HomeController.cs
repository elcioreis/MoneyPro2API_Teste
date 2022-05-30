using Microsoft.AspNetCore.Mvc;
using MoneyPro2.Data;
using System.Diagnostics;

namespace MoneyPro2.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    [HttpGet("")]
    public IActionResult Get()
    {
        return Ok($"MoneyPro2 em {DateTime.UtcNow.ToString("G")} (UTC)");
    }

    [HttpGet("/ping")]
    public IActionResult GetPing(
        [FromServices] MoneyDataContext context)
    {
        try
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var count = context.Users.Count();
            stopwatch.Stop();

            var ms = Math.Floor(stopwatch.Elapsed.TotalMilliseconds);

            return Ok($"MoneyPro2 conversando com o banco de dados em {DateTime.UtcNow.ToString("G")} (UTC)\nTempo de acesso: {ms.ToString()} ms");
        }
        catch (Exception)
        {
            return StatusCode(500, "E01X00 - Falha interna no servidor");
        }
    }
}