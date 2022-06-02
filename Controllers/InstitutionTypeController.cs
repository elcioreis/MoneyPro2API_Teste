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
        if (Int32.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out int userId))
        {
            try
            {
                var institutionTypes = await context.InstitutionTypes
                    .AsNoTracking()
                    .Where(x => x.UserId == userId)
                    .OrderBy(x => x.Nickname).ToListAsync();

                return Ok(new ResultViewModel<List<InstitutionType>>(institutionTypes));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<List<InstitutionType>>("E04X00 - Falha interna no servidor"));
            }
        }
        else
        {
            return StatusCode(401, new ResultViewModel<List<InstitutionType>>("E04X01 - Não autorizado"));
        }
    }

    [HttpGet("v1/institutiontype/active")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> GetActiveAsync(
       [FromServices] MoneyDataContext context)
    {
        if (Int32.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out int userId))
        {
            try
            {
                var institutionTypes = await context.InstitutionTypes
                    .AsNoTracking()
                    .Where(x => x.UserId == userId && x.Active)
                    .OrderBy(x => x.Nickname).ToListAsync();

                return Ok(new ResultViewModel<List<InstitutionType>>(institutionTypes));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<List<InstitutionType>>("E04X00 - Falha interna no servidor"));
            }
        }
        else
        {
            return StatusCode(401, new ResultViewModel<List<InstitutionType>>("E04X01 - Não autorizado"));
        }
    }

    [HttpPost("v1/institutiontype")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> PostAsync(
        [FromBody] InstitutionTypeViewModel model,
        [FromServices] MoneyDataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<InstitutionType>(ModelState.GetErrors("E04X02 - Conteúdo mal formatado")));

        if (!Int32.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out int userId))
            return StatusCode(401, new ResultViewModel<List<InstitutionType>>("E04X03 - Não autorizado"));

        var institutionType = new InstitutionType()
        {
            UserId = userId,
            Nickname = model.Nickname,
            Description = model.Description,
            Active = true
        };

        try
        {
            _ = await context.InstitutionTypes.AddAsync(institutionType);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<InstitutionType>(institutionType));
        }
        catch (DbUpdateException ex)
        {
            // Usa Regex para pegar o conteúdo duplicado do banco de dados, que vem entre parênteses do Sql Server
            Regex filtro = new Regex(@"(.*\()|(\).*)", RegexOptions.Singleline);
            var chave = filtro.Replace(ex.InnerException?.Message ?? "", "");

            if (!string.IsNullOrEmpty(chave))
                return StatusCode(400, new ResultViewModel<InstitutionType>($"E04X04 - Tipo de instituição já cadastrada para esse usuário: {chave}"));
            else
                return StatusCode(400, new ResultViewModel<InstitutionType>("E04X04 - Tipo de instituição já cadastrada para esse usuário"));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<InstitutionType>("E04X05 - Falha interna no servidor"));
        }
    }

    [HttpPut("v1/institutiontype/{id:int}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> Update(
        [FromRoute] int id,
        [FromBody] InstitutionTypeViewModel model,
        [FromServices] MoneyDataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<InstitutionType>(ModelState.GetErrors("E04X06 - Conteúdo mal formatado")));

        if (!Int32.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out int userId))
            return StatusCode(401, new ResultViewModel<List<InstitutionType>>("E04X07 - Não autorizado"));

        try
        {
            var institutionType = await context.InstitutionTypes.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (institutionType == null)
                return NotFound(new ResultViewModel<InstitutionType>("E04X08 - Conteúdo não encontrado"));

            institutionType.Nickname = model.Nickname;
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
                return StatusCode(400, new ResultViewModel<InstitutionType>($"E04X09 - Tipo de instituição já cadastrada para esse usuário: {chave}"));
            else
                return StatusCode(400, new ResultViewModel<InstitutionType>("E04X09 - Tipo de instituição já cadastrada para esse usuário"));
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
        if (!Int32.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out int userId))
            return StatusCode(401, new ResultViewModel<List<InstitutionType>>("E04X11 - Não autorizado"));

        try
        {
            var institutionType = await context.InstitutionTypes.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (institutionType == null)
                return NotFound(new ResultViewModel<InstitutionType>("E04X12 - Conteúdo não encontrado"));

            context.InstitutionTypes.Remove(institutionType);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<String>($"O tipo de instituição {institutionType.Nickname} foi excluído", null));
        }
        catch (DbUpdateException ex)
        {
            var msg = ex.InnerException?.Message;

            if (!string.IsNullOrEmpty(msg))
                return StatusCode(400, new ResultViewModel<Coin>($"E04X13 - Impossível excluir o tipo de instituição: {msg}"));
            else
                return StatusCode(400, new ResultViewModel<Coin>("E04X13 - Impossível excluir o tipo de instituição"));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Coin>("E04X14 - Falha interna no servidor"));
        }
    }

    [HttpPut("v1/institutiontype/active/{id:int}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> ChangeActiveAsync(
        [FromRoute] int id,
        [FromServices] MoneyDataContext context)
    {
        if (!Int32.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return StatusCode(401, new ResultViewModel<InstitutionType>("E04X15 - Não autorizado"));

        try
        {
            var institutionType = await context.InstitutionTypes.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

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