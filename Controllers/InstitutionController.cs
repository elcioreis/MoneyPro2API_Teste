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
public class InstitutionController : ControllerBase
{
    [HttpGet("v1/institution/all")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> GetAllAsync(
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<List<Institution>>("E05X00 - Não autorizado"));

        try
        {
            var institutions = await context.Institutions
               .AsNoTracking()
               .Where(x => x.UserId == userId)
               .OrderBy(x => x.Name)
               .ToListAsync();

            return Ok(new ResultViewModel<List<Institution>>(institutions));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<List<Institution>>("E05X01 - Falha interna no servidor"));
        }
    }

    [HttpGet("v1/institution/active")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> GetActiveAsync(
    [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<List<Institution>>("E05X02 - Não autorizado"));

        try
        {
            var institutions = await context.Institutions
               .AsNoTracking()
               .Where(x => x.UserId == userId && x.Active)
               .OrderBy(x => x.Name)
               .ToListAsync();

            return Ok(new ResultViewModel<List<Institution>>(institutions));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<List<Institution>>("E05X03 - Falha interna no servidor"));
        }
    }

    [HttpGet("v1/institution/{id:int}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] int id,
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<Institution>("E05X04 - Não autorizado"));

        try
        {
            var institution = await context.Institutions
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (institution == null)
                return NotFound(new ResultViewModel<Institution>("E05X05 - Conteúdo não encontrado"));

            return Ok(new ResultViewModel<Institution>(institution));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Institution>("E05X06 - Falha interna no servidor"));
        }
    }

    [HttpPost("v1/institution")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> InsertAsync(
        [FromBody] InstitutionViewModel model,
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return StatusCode(501, new ResultViewModel<Institution>("E05X07 - Não autorizado"));

        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<Institution>(ModelState.GetErrors("E05X08 - Conteúdo mal formatado")));

        try
        {
            var institutionType = await context.InstitutionTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == model.InstitutionTypeId && x.UserId == userId);

            if (institutionType == null)
                return BadRequest(new ResultViewModel<Institution>("E05X09 - Tipo de instituição não encontrado"));

            var institution = new Institution()
            {
                UserId = userId,
                InstitutionTypeId = institutionType.Id,
                Name = model.Name,
                Description = model.Description,
                BankNumber = model.BankNumber
            };

            await context.Institutions.AddAsync(institution);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Institution>(institution));
        }
        catch (DbUpdateException ex)
        {
            // Usa Regex para pegar o conteúdo duplicado do banco de dados, que vem entre parênteses do Sql Server
            Regex filtro = new Regex(@"(.*\()|(\).*)", RegexOptions.Singleline);
            var chave = filtro.Replace(ex.InnerException?.Message ?? "", "");

            if (!string.IsNullOrEmpty(chave))
                return StatusCode(400, new ResultViewModel<Institution>($"E05X10 - Instituição já cadastrada para esse usuário: {chave}"));
            else
                return StatusCode(400, new ResultViewModel<Institution>("E05X10 - Instituição já cadastrada para esse usuário"));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Institution>("E05X11 - Falha interna no servidor"));
        }
    }

    [HttpPut("v1/institution/{id:int}")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> UpdateAsync(
        [FromRoute] int id,
        [FromBody] InstitutionViewModel model,
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return StatusCode(501, new ResultViewModel<Institution>("E05X12 - Não autorizado"));

        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<Institution>(ModelState.GetErrors("E05X13 - Conteúdo mal formatado")));

        try
        {
            var institution = await context.Institutions.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (institution == null)
                return NotFound(new ResultViewModel<Institution>("E05X14 - Conteúdo não encontrado"));

            institution.InstitutionTypeId = model.InstitutionTypeId;
            institution.Name = model.Name;
            institution.Description = model.Description;
            institution.BankNumber = model.BankNumber;

            context.Institutions.Update(institution);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Institution>(institution));
        }
        catch (DbUpdateException ex)
        {
            // Usa Regex para pegar o conteúdo duplicado do banco de dados, que vem entre parênteses do Sql Server
            Regex filtro = new Regex(@"(.*\()|(\).*)", RegexOptions.Singleline);
            var chave = filtro.Replace(ex.InnerException?.Message ?? "", "");

            if (!string.IsNullOrEmpty(chave))
                return StatusCode(400, new ResultViewModel<Institution>($"E05X15 - Instituição já cadastrada para esse usuário: {chave}"));
            else
                return StatusCode(400, new ResultViewModel<Institution>("E05X15 - Instituição já cadastrada para esse usuário"));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Institution>("E05X16 - Falha interna no servidor"));
        }
    }

    [HttpDelete("v1/institution/{id:int}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] int id,
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<string>("E05X17 - Não autorizado"));

        try
        {
            var institution = await context.Institutions.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (institution == null)
                return NotFound(new ResultViewModel<string>("E05X18 - Conteúdo não encontrado"));

            context.Institutions.Remove(institution);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<string>($"A instituição {institution.Name} foi excluída", null));
        }
        catch (DbUpdateException ex)
        {
            // Usa Regex para pegar o conteúdo duplicado do banco de dados, que vem entre parênteses do Sql Server
            Regex filtro = new Regex(@"(.*\()|(\).*)", RegexOptions.Singleline);
            var chave = filtro.Replace(ex.InnerException?.Message ?? "", "");

            if (!string.IsNullOrEmpty(chave))
                return StatusCode(400, new ResultViewModel<Institution>($"E05X19 - Instituição não pode ser excluída: {chave}"));
            else
                return StatusCode(400, new ResultViewModel<Institution>("E05X19 - Instituição não pode ser excluída"));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Institution>("E05X20 - Falha interna no servidor"));
        }
    }

    [HttpPut("v1/institution/active/{id:int}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> ChangeActiveByIdAsync(
        [FromRoute] int id,
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<Institution>("E05X21 - Não autorizado"));

        try
        {
            var institution = await context.Institutions
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (institution == null)
                return NotFound(new ResultViewModel<Institution>("E05X22 - Conteúdo não encontrado"));

            institution.Active = !institution.Active;

            context.Institutions.Update(institution);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Institution>(institution));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Institution>("E05X23 - Falha interna no servidor"));
        }
    }
}