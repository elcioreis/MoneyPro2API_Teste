using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyPro2.Data;
using MoneyPro2.Extensions;
using MoneyPro2.Models;
using MoneyPro2.ViewModel;
using MoneyPro2.ViewModels.Entries;
using System.Security.Claims;
using System.Text.RegularExpressions;

[ApiController]
public class EntryController : ControllerBase
{

    [HttpGet("v1/entry/all")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> GetAllAsync(
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<List<Entry>>("E06X00 - Não autorizado"));

        try
        {
            var entries = await context.Entries
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.Name)
                .ToListAsync();

            return Ok(new ResultViewModel<List<Entry>>(entries));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<List<Entry>>("E06X01 - Falha interna no servidor"));
        }
    }

    [HttpGet("v1/entry/active")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> GetActiveAsync(
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<List<Entry>>("E06X02 - Não autorizado"));

        try
        {
            var entries = await context.Entries
                .AsNoTracking()
                .Where(x => x.UserId == userId && x.Active)
                .OrderBy(x => x.Name)
                .ToListAsync();

            return Ok(new ResultViewModel<List<Entry>>(entries));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<List<Entry>>("E06X03 - Falha interna no servidor"));
        }
    }

    [HttpGet("v1/entry/{id:int}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] int id,
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<Entry>("E06X04 - Não autorizado"));

        try
        {
            var entry = await context.Entries.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id);

            if (entry == null)
                return NotFound(new ResultViewModel<Entry>("E06X05 - Conteúdo não encontrado"));

            return Ok(new ResultViewModel<Entry>(entry));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Entry>("E06X06 - Falha interna no servidor"));
        }
    }

    [HttpPost("v1/entry")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> InsertAsync(
        [FromBody] EntryViewModel model,
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<Entry>("E06X07 - Não autorizado"));

        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<Entry>(ModelState.GetErrors("E06X08 - Conteúdo mal formatado")));

        var entry = new Entry()
        {
            UserId = userId,
            Name = model.Name,
            Description = model.Description,
            Active = true,
            System = false
        };

        try
        {
            await context.Entries.AddAsync(entry);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Entry>(entry));
        }
        catch (DbUpdateException ex)
        {
            // Usa Regex para pegar o conteúdo duplicado do banco de dados, que vem entre parênteses do Sql Server
            Regex filtro = new Regex(@"(.*\()|(\).*)", RegexOptions.Singleline);
            var chave = filtro.Replace(ex.InnerException?.Message ?? "", "");

            if (!string.IsNullOrEmpty(chave))
                return StatusCode(400, new ResultViewModel<Entry>($"E06X09 - Entrada já cadastrada para esse usuário: {chave}"));
            else
                return StatusCode(400, new ResultViewModel<Entry>("E06X09 - Entrada já cadastrada para esse usuário"));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Entry>("E06X0A - Falha interna no servidor"));
        }
    }

    [HttpPut("v1/entry/{id:int}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] int id,
        [FromBody] EntryViewModel model,
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<Entry>("E06X0B - Não autorizado"));

        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<Entry>(ModelState.GetErrors("E06X0C - Conteúdo mal formatado")));

        try
        {
            var entry = await context.Entries.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (entry == null)
                return NotFound(new ResultViewModel<Entry>("E06X0D - Conteúdo não encontrado"));

            entry.Name = model.Name;
            entry.Description = model.Description;

            context.Entries.Update(entry);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Entry>(entry));
        }
        catch (DbUpdateException ex)
        {
            // Usa Regex para pegar o conteúdo duplicado do banco de dados, que vem entre parênteses do Sql Server
            Regex filtro = new Regex(@"(.*\()|(\).*)", RegexOptions.Singleline);
            var chave = filtro.Replace(ex.InnerException?.Message ?? "", "");

            if (!string.IsNullOrEmpty(chave))
                return StatusCode(400, new ResultViewModel<Entry>($"E06X0E - Entrada já cadastrada para esse usuário: {chave}"));
            else
                return StatusCode(400, new ResultViewModel<Entry>("E06X0E - Entrada já cadastrada para esse usuário"));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Entry>("E06X0F - Falha interna no servidor"));
        }
    }

    [HttpPut("v1/entry/active/{id:int}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> ChangeActiveByIdAsync(
        [FromRoute] int id,
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<Entry>("E06X10 - Não autorizado"));

        try
        {
            var entry = context.Entries.FirstOrDefault(x => x.Id == id && x.UserId == userId);

            if (entry == null)
                return NotFound(new ResultViewModel<Entry>("E06X11 - Conteúdo não encontrado"));

            entry.Active = !entry.Active;

            context.Entries.Update(entry);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Entry>(entry));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Entry>("E06X12 - Falha interna no servidor"));
        }
    }

    [HttpDelete("v1/entry/{id:int}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] int id,
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<string>("E06X13 - Não autorizado"));

        try
        {
            var entry = await context.Entries.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (entry == null)
                return NotFound(new ResultViewModel<string>("E06X14 - Conteúdo não localizado"));

            context.Entries.Remove(entry);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<string>($"A entrada {entry.Name} foi excluída", null));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<string>("E06X15 - Falha interna no servidor"));
        }
    }
}