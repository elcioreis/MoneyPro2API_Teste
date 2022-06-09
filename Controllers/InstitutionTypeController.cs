using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyPro2.Data;
using MoneyPro2.Extensions;
using MoneyPro2.Models;
using MoneyPro2.ViewModel;
using MoneyPro2.ViewModels.Institution;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace MoneyPro2.Controllers;

[ApiController]
public class InstitutionTypeController : ControllerBase
{
    [HttpGet("v1/institutiontype/all")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> GetAllAsync(
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<List<InstitutionType>>("E04X00 - Não autorizado"));

        try
        {
            var institutionTypes = await context.InstitutionTypes
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.Name)
                .ToListAsync();

            return Ok(new ResultViewModel<List<InstitutionType>>(institutionTypes));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<List<InstitutionType>>("E04X01 - Falha interna no servidor"));
        }
    }

    [HttpGet("v1/institutiontype/active")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> GetActiveAsync(
       [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<List<InstitutionType>>("E04X02 - Não autorizado"));

        try
        {
            var institutionTypes = await context.InstitutionTypes
                .AsNoTracking()
                .Where(x => x.UserId == userId && x.Active)
                .OrderBy(x => x.Name)
                .ToListAsync();

            return Ok(new ResultViewModel<List<InstitutionType>>(institutionTypes));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<List<InstitutionType>>("E04X03 - Falha interna no servidor"));
        }
    }

    [HttpGet("v1/institutiontype/{id:int}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] int id,
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<InstitutionType>("E04X04 - Não autorizado"));

        try
        {
            var institutiontype = await context.InstitutionTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (institutiontype == null)
                return NotFound(new ResultViewModel<InstitutionType>("E04X05 - Conteúdo não encontrado"));

            return Ok(new ResultViewModel<InstitutionType>(institutiontype));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<InstitutionType>("E04X06 - Falha interna do servidor"));
        }
    }

    [HttpPost("v1/institutiontype")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> PostAsync(
        [FromBody] InstitutionTypeViewModel model,
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<InstitutionType>("E04X07 - Não autorizado"));

        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<InstitutionType>(ModelState.GetErrors("E04X08 - Conteúdo mal formatado")));

        var institutionType = new InstitutionType()
        {
            UserId = userId,
            Name = model.Name,
            Description = model.Description,
            Active = true
        };

        try
        {
            await context.InstitutionTypes.AddAsync(institutionType);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<InstitutionType>(institutionType));
        }
        catch (DbUpdateException ex)
        {
            // Usa Regex para pegar o conteúdo duplicado do banco de dados, que vem entre parênteses do Sql Server
            Regex filtro = new Regex(@"(.*\()|(\).*)", RegexOptions.Singleline);
            var chave = filtro.Replace(ex.InnerException?.Message ?? "", "");

            if (!string.IsNullOrEmpty(chave))
                return StatusCode(400, new ResultViewModel<InstitutionType>($"E04X0A - Tipo de instituição já cadastrada para esse usuário: {chave}"));
            else
                return StatusCode(400, new ResultViewModel<InstitutionType>("E04X0A - Tipo de instituição já cadastrada para esse usuário"));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<InstitutionType>("E04X0B - Falha interna no servidor"));
        }
    }

    [HttpPut("v1/institutiontype/{id:int}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> Update(
        [FromRoute] int id,
        [FromBody] InstitutionTypeViewModel model,
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<InstitutionType>("E04X0C - Não autorizado"));

        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<InstitutionType>(ModelState.GetErrors("E04X0D - Conteúdo mal formatado")));

        try
        {
            var institutionType = await context.InstitutionTypes
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (institutionType == null)
                return NotFound(new ResultViewModel<InstitutionType>("E04X0E - Conteúdo não encontrado"));

            institutionType.Name = model.Name;
            institutionType.Description = model.Description;

            context.InstitutionTypes.Update(institutionType);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<InstitutionType>(institutionType));
        }
        catch (DbUpdateException ex)
        {
            // Usa Regex para pegar o conteúdo duplicado do banco de dados, que vem entre parênteses do Sql Server
            Regex filtro = new Regex(@"(.*\()|(\).*)", RegexOptions.Singleline);
            var chave = filtro.Replace(ex.InnerException?.Message ?? "", "");

            if (!string.IsNullOrEmpty(chave))
                return StatusCode(400, new ResultViewModel<InstitutionType>($"E04X0F - Tipo de instituição já cadastrada para esse usuário: {chave}"));
            else
                return StatusCode(400, new ResultViewModel<InstitutionType>("E04X0F - Tipo de instituição já cadastrada para esse usuário"));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<InstitutionType>("E04X10 - Falha interna no servidor"));
        }
    }

    [HttpDelete("v1/institutiontype/{id:int}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> Delete(
        [FromRoute] int id,
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<string>("E04X11 - Não autorizado"));

        try
        {
            var institutionType = await context.InstitutionTypes
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (institutionType == null)
                return NotFound(new ResultViewModel<string>("E04X12 - Conteúdo não encontrado"));

            context.InstitutionTypes.Remove(institutionType);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<String>($"O tipo de instituição {institutionType.Name} foi excluído", false));
        }
        catch (DbUpdateException ex)
        {
            var msg = ex.InnerException?.Message;

            if (!string.IsNullOrEmpty(msg))
                return StatusCode(400, new ResultViewModel<string>($"E04X13 - Impossível excluir o tipo de instituição: {msg}"));
            else
                return StatusCode(400, new ResultViewModel<string>("E04X13 - Impossível excluir o tipo de instituição"));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<string>("E04X14 - Falha interna no servidor"));
        }
    }

    [HttpPut("v1/institutiontype/active/{id:int}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> ChangeActiveByIdAsync(
        [FromRoute] int id,
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<InstitutionType>("E04X15 - Não autorizado"));

        try
        {
            var institutionType = await context.InstitutionTypes
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (institutionType == null)
                return NotFound(new ResultViewModel<InstitutionType>("E04X16 - Conteúdo não encontrado"));

            institutionType.Active = !institutionType.Active;

            context.InstitutionTypes.Update(institutionType);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<InstitutionType>(institutionType));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<InstitutionType>("E04X17 - Falha interna no servidor"));
        }
    }
}