using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyPro2.Data;
using MoneyPro2.Extensions;
using MoneyPro2.Models;
using MoneyPro2.ViewModel;
using MoneyPro2.ViewModels.Category;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace MoneyPro2.Controllers;

[ApiController]
public class CategoryGroupController : ControllerBase
{
    [HttpGet("v1/categorygroup/all")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> GetAllAsync(
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<List<CategoryGroup>>("E07X00 - Não autorizado"));

        try
        {
            var categoryGroups = await context.CategoryGroups
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.Name)
                .ToListAsync();

            var count = categoryGroups.Count();

            return Ok(new ResultViewModel<dynamic>(new { total = count, categoryGroups }));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<List<CategoryGroup>>("E07X01 - Falha interna no servidor"));
        }
    }

    [HttpGet("v1/categorygroup/active")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> GetActiveAsync(
    [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<List<CategoryGroup>>("E07X02 - Não autorizado"));

        try
        {
            var categoryGroups = await context.CategoryGroups
                .AsNoTracking()
                .Where(x => x.UserId == userId && x.Active)
                .OrderBy(x => x.Name)
                .ToListAsync();

            var count = categoryGroups.Count();

            return Ok(new ResultViewModel<dynamic>(new { total = count, categoryGroups }));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<List<CategoryGroup>>("E07X03 - Falha interna no servidor"));
        }
    }

    [HttpGet("v1/categorygroup/{id:int}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] int id,
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<CategoryGroup>("E07X04 - Não autorizado"));

        try
        {
            var categoryGroup = await context.CategoryGroups
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id);

            if (categoryGroup == null)
                return NotFound(new ResultViewModel<CategoryGroup>("E07X05 - Conteúdo não encotnrado"));

            return Ok(new ResultViewModel<CategoryGroup>(categoryGroup));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<CategoryGroup>("E07X06 - Falha interna no servidor"));
        }
    }

    [HttpPost("v1/categorygroup")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> InsertAsync(
        [FromBody] CategoryGroupViewModel model,
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<CategoryGroup>("E07X07 - Não autorizado"));

        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<CategoryGroup>(ModelState.GetErrors("E07X08 - Conteúdo mal formatado")));

        var categoryGroup = new CategoryGroup()
        {
            UserId = userId,
            Name = model.Name,
            Description = model.Description,
            Active = true
        };

        try
        {
            await context.CategoryGroups.AddAsync(categoryGroup);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<CategoryGroup>(categoryGroup));
        }
        catch (DbUpdateException ex)
        {
            // Usa Regex para pegar o conteúdo duplicado do banco de dados, que vem entre parênteses do Sql Server
            Regex filtro = new Regex(@"(.*\()|(\).*)", RegexOptions.Singleline);
            var chave = filtro.Replace(ex.InnerException?.Message ?? "", "");

            if (!string.IsNullOrEmpty(chave))
                return StatusCode(400, new ResultViewModel<CategoryGroup>($"E07X09 - Grupo de categorias já cadastrada para esse usuário: {chave}"));
            else
                return StatusCode(400, new ResultViewModel<CategoryGroup>("E07X09 - Grupo de categorias já cadastrada para esse usuário"));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<CategoryGroup>("E07X0A - Falha interna no servidor"));
        }
    }

    [HttpPut("v1/categorygroup/{id:int}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] int id,
        [FromBody] CategoryGroupViewModel model,
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<CategoryGroup>("E07X0B - Não autorizado"));

        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<CategoryGroup>("E07X0C - Conteúdo mal formatado"));

        try
        {
            var categoryGroup = await context.CategoryGroups.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (categoryGroup == null)
                return NotFound(new ResultViewModel<CategoryGroup>("E07X0D - Conteúdo não encontrado"));

            categoryGroup.Name = model.Name;
            categoryGroup.Description = model.Description;

            context.CategoryGroups.Update(categoryGroup);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<CategoryGroup>(categoryGroup));
        }
        catch (DbUpdateException ex)
        {
            // Usa Regex para pegar o conteúdo duplicado do banco de dados, que vem entre parênteses do Sql Server
            Regex filtro = new Regex(@"(.*\()|(\).*)", RegexOptions.Singleline);
            var chave = filtro.Replace(ex.InnerException?.Message ?? "", "");

            if (!string.IsNullOrEmpty(chave))
                return StatusCode(400, new ResultViewModel<CategoryGroup>($"E07X0E - Grupo de categorias já cadastrada para esse usuário: {chave}"));
            else
                return StatusCode(400, new ResultViewModel<CategoryGroup>("E07X0E - Grupo de categorias já cadastrada para esse usuário"));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<CategoryGroup>("E07X0F - Falha interna no servidor"));
        }
    }

    [HttpPut("v1/categorygroup/active/{id:int}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> ChangeActiveByIdAsync(
        [FromRoute] int id,
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<CategoryGroup>("E07X10 - Não autorizado"));

        try
        {
            var categoryGroup = await context.CategoryGroups.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (categoryGroup == null)
                return NotFound(new ResultViewModel<CategoryGroup>("E07X11 - Conteúdo não encontrado"));

            categoryGroup.Active = !categoryGroup.Active;

            context.CategoryGroups.Update(categoryGroup);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<CategoryGroup>(categoryGroup));
        }
        catch (DbUpdateException ex)
        {
            // Usa Regex para pegar o conteúdo duplicado do banco de dados, que vem entre parênteses do Sql Server
            Regex filtro = new Regex(@"(.*\()|(\).*)", RegexOptions.Singleline);
            var chave = filtro.Replace(ex.InnerException?.Message ?? "", "");

            if (!string.IsNullOrEmpty(chave))
                return StatusCode(400, new ResultViewModel<CategoryGroup>($"E07X12 - Grupo de categorias já cadastrada para esse usuário: {chave}"));
            else
                return StatusCode(400, new ResultViewModel<CategoryGroup>("E07X12 - Grupo de categorias já cadastrada para esse usuário"));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<CategoryGroup>("E07X13 - Falha interna no servidor"));
        }
    }

    [HttpDelete("v1/categorygroup/{id:int}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] int id,
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<string>("E07X14 - Não autorizado"));

        try
        {
            var categoryGroup = await context.CategoryGroups.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (categoryGroup == null)
                return NotFound(new ResultViewModel<string>("E07X15 - Conteúdo não encontrado"));

            context.CategoryGroups.Remove(categoryGroup);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<string>($"O Grupo de Categorias {categoryGroup.Name} foi removido.", false));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<string>("E07X16 - Falha interna no servidor"));
        }
    }
}