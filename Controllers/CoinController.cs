using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyPro2.Data;
using MoneyPro2.Extensions;
using MoneyPro2.Models;
using MoneyPro2.ViewModel;
using MoneyPro2.ViewModels.Coins;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace MoneyPro2.Controllers;

[ApiController]
public class CoinController : ControllerBase
{
    [HttpGet("v1/coin/all")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> GetAllAsync(
        [FromServices] MoneyDataContext context)
    {
        try
        {
            var coins = await context.Coins
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToListAsync();

            return Ok(new ResultViewModel<List<Coin>>(coins));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<List<Coin>>("E03X00 - Falha interna no servidor"));
        }
    }

    [HttpGet("v1/coin/active")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> GetActiveAsync(
    [FromServices] MoneyDataContext context)
    {
        try
        {
            var coins = await context.Coins
                .AsNoTracking()
                .Where(x => x.Active)
                .OrderBy(x => x.Name)
                .ToListAsync();

            return Ok(new ResultViewModel<List<Coin>>(coins));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<List<Coin>>("E03X01 - Falha interna no servidor"));
        }
    }

    [HttpGet("v1/coin/{id:int}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] int id,
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<Coin>("E03X02 - Não autorizado"));

        try
        {
            var coin = await context.Coins
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (coin == null)
                return NotFound(new ResultViewModel<Coin>("E03X03 - Conteúdo não encontrado"));

            return Ok(new ResultViewModel<Coin>(coin));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Coin>("E03X04 - Falha interna no servidor"));
        }
    }

    [HttpGet("v1/coin/default")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> GetDefaultAsync(
    [FromServices] MoneyDataContext context)
    {
        try
        {
            var coin = await context.Coins.AsNoTracking().FirstOrDefaultAsync(x => x.Default);

            if (coin == null)
                return NotFound(new ResultViewModel<Coin>("E03X05 - Conteúdo não encontrado"));

            return Ok(new ResultViewModel<Coin>(coin));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<List<Coin>>("E03X06 - Falha interna no servidor"));
        }
    }

    [HttpPost("v1/coin")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> PostAsync(
        [FromBody] CoinViewModel model,
        [FromServices] MoneyDataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<Coin>(ModelState.GetErrors("E03X07 - Conteúdo mal formatado")));

        try
        {
            var coin = new Coin()
            {
                Name = model.Name,
                Symbol = model.Symbol,
                Default = false,
                Virtual = false,
                Active = true
            };

            await context.Coins.AddAsync(coin);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Coin>(coin));
        }
        catch (DbUpdateException ex)
        {
            // Usa Regex para pegar o conteúdo duplicado do banco de dados, que vem entre parênteses do Sql Server
            Regex filtro = new Regex(@"(.*\()|(\).*)", RegexOptions.Singleline);
            var chave = filtro.Replace(ex.InnerException?.Message ?? "", "");

            if (!string.IsNullOrEmpty(chave))
                return StatusCode(400, new ResultViewModel<Coin>($"E03X08 - Moeda já cadastrada: {chave}"));
            else
                return StatusCode(400, new ResultViewModel<Coin>("E03X08 - Moeda já cadastrada"));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Coin>("E03X09 - Falha interna no servidor"));
        }
    }

    [HttpPut("v1/coin/{id:int}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> PutAsync(
        [FromRoute] int id,
        [FromBody] CoinViewModel model,
        [FromServices] MoneyDataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<Coin>(ModelState.GetErrors("E03X10 - Conteúdo mal formatado")));

        try
        {
            var coin = await context.Coins.FirstOrDefaultAsync(x => x.Id == id);

            if (coin == null)
                return NotFound(new ResultViewModel<Coin>(ModelState.GetErrors("E03X11 - Conteúdo não encontrado")));

            coin.Name = model.Name;
            coin.Symbol = model.Symbol;

            context.Coins.Update(coin);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Coin>(coin));
        }
        catch (DbUpdateException ex)
        {
            // Usa Regex para pegar o conteúdo duplicado do banco de dados, que vem entre parênteses do Sql Server
            Regex filtro = new Regex(@"(.*\()|(\).*)", RegexOptions.Singleline);
            var chave = filtro.Replace(ex.InnerException?.Message ?? "", "");

            if (!string.IsNullOrEmpty(chave))
                return StatusCode(400, new ResultViewModel<Coin>($"E03X12 - Moeda já cadastrada: {chave}"));
            else
                return StatusCode(400, new ResultViewModel<Coin>("E03X12 - Moeda já cadastrada"));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Coin>("E03X13 - Falha interna no servidor"));
        }
    }

    [HttpPut("v1/coin/virtual/{id:int}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> VirtualAsync(
        [FromRoute] int id,
        [FromServices] MoneyDataContext context)
    {
        try
        {
            var coin = await context.Coins.FirstOrDefaultAsync(x => x.Id == id);

            if (coin == null)
                return NotFound(new ResultViewModel<Coin>(ModelState.GetErrors("E03X14 - Conteúdo não encontrado")));

            coin.Virtual = !coin.Virtual;

            context.Coins.Update(coin);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Coin>(coin));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Coin>("E03X15 - Falha interna no servidor"));
        }
    }

    [HttpPut("v1/coin/default/{id:int}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> DefaultAsync(
        [FromRoute] int id,
        [FromServices] MoneyDataContext context)
    {
        var newDefault = await context.Coins.FirstOrDefaultAsync(x => x.Id == id);

        if (newDefault == null)
            return NotFound(new ResultViewModel<Coin>(ModelState.GetErrors("E03X16 - Conteúdo não encontrado")));

        if (newDefault.Default)
            return Ok(new ResultViewModel<Coin>(newDefault));

        var oldDefault = await context.Coins.FirstOrDefaultAsync(x => x.Default);

        if (oldDefault == null)
            return NotFound(new ResultViewModel<Coin>(ModelState.GetErrors("E03X17 - Conteúdo não encontrado")));

        newDefault.Default = true;
        oldDefault.Default = false;

        using (var dbTransaction = context.Database.BeginTransaction())
        {
            try
            {
                context.Coins.Update(newDefault);
                context.Coins.Update(oldDefault);
                await context.SaveChangesAsync();

                dbTransaction.Commit();
                return Ok(new ResultViewModel<Coin>(newDefault));
            }
            catch (Exception)
            {
                dbTransaction.Rollback();
                return StatusCode(500, new ResultViewModel<Coin>("E03X18 - Falha interna no servidor"));
            }
        }
    }

    [HttpPut("v1/coin/active/{id:int}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> ActiveAsync(
        [FromRoute] int id,
        [FromServices] MoneyDataContext context)
    {
        try
        {
            var coin = await context.Coins.FirstOrDefaultAsync(x => x.Id == id);

            if (coin == null)
                return NotFound(new ResultViewModel<Coin>(ModelState.GetErrors("E03X19 - Conteúdo não encontrado")));

            if (coin.Default)
                return BadRequest(new ResultViewModel<Coin>(ModelState.GetErrors("E03X20 - A moeda padrão não pode ser inativada")));

            coin.Active = !coin.Active;

            context.Coins.Update(coin);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Coin>(coin));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Coin>("E03X21 - Falha interna no servidor"));
        }
    }

    [HttpDelete("v1/coin/{id:int}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] int id,
        [FromServices] MoneyDataContext context)
    {
        var coin = await context.Coins.FirstOrDefaultAsync(x => x.Id == id);

        if (coin == null)
            return NotFound(new ResultViewModel<Coin>("E03X22 - Conteúdo não encontrado"));

        if (coin.Default)
            return BadRequest(new ResultViewModel<Coin>("E03X23 - A moeda padrão não pode ser excluída"));

        try
        {
            context.Coins.Remove(coin);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<String>($"A moeda {coin.Name} foi excluída", null));
        }
        catch (DbUpdateException ex)
        {
            var msg = ex.InnerException?.Message;

            if (!string.IsNullOrEmpty(msg))
                return StatusCode(400, new ResultViewModel<Coin>($"E03X24 - Impossível excluir moeda: {msg}"));
            else
                return StatusCode(400, new ResultViewModel<Coin>("E03X25 - Impossível excluir moeda"));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Coin>("E03X26 - Falha interna no servidor"));
        }
    }
}