using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyPro2.Data;
using MoneyPro2.Extensions;
using MoneyPro2.Models;
using MoneyPro2.ViewModel;
using MoneyPro2.ViewModels.Category;
using System.Security.Claims;

namespace MoneyPro2.Controllers;

[ApiController]
public class CategoryController : ControllerBase
{
    [HttpGet("v1/category/all")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> GetAllAsync(
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<List<Category>>("E08X00 - Não autorizado"));

        try
        {
            var categories = await context.Categories
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.Name)
                .ToListAsync();

            var count = categories.Count();

            return Ok(new ResultViewModel<dynamic>(new { total = count, categories }));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<List<Category>>("E08X01 - Falha interna no servidor"));
        }
    }

    [HttpGet("v1/category/active")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> GetActiveAsync(
    [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<List<Category>>("E08X02 - Não autorizado"));

        try
        {
            var categories = await context.Categories
                .AsNoTracking()
                .Where(x => x.UserId == userId && x.Active)
                .OrderBy(x => x.Name)
                .ToListAsync();

            var count = categories.Count();

            return Ok(new ResultViewModel<dynamic>(new { total = count, categories }));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<List<Category>>("E08X03 - Falha interna no servidor"));
        }
    }

    [HttpGet("v1/category/{id:int}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] int id,
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<Category>("E08X04 - Não autorizado"));

        try
        {
            var category = await context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id);

            if (category == null)
                return NotFound(new ResultViewModel<Category>("E08X05 - Conteúdo não localizado"));

            return Ok(new ResultViewModel<Category>(category));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<List<Category>>("E08X06 - Falha interna no servidor"));
        }
    }

    [HttpPost("v1/category")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> InsertAsync(
        [FromBody] CategoryViewModel model,
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<Category>("E08X07 - Não autorizado"));

        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors("E08X08 - Conteúdo mal formatado")));

        var category = new Category()
        {
            UserId = userId,
            Name = model.Name,
            Description = model.Description,
            CategoryParentId = (model.CategoryParentId > 0 ? model.CategoryParentId : null),
            CategoryGroupId = (model.CategoryGroupId > 0 ? model.CategoryGroupId : null),
            CrdDeb = (model.CrdDeb != "M" ? model.CrdDeb : " "),
            VisualOrder = (model.VisualOrder > 0 ? model.VisualOrder : null),
            Fixed = (model.Fixed == "true"),
            System = (model.System == "true"),
            Active = true
        };

        try
        {
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
            return Ok(new ResultViewModel<Category>(category));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<Category>($"E08X09 - Falha interna no servidor: {ex.InnerException?.Message}"));
        }
    }

    [HttpPut("v1/category/{id:int}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] int id,
        [FromBody] CategoryViewModel model,
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<Category>("E08X0A - Não autorizado"));

        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors("E08X11 - Conteúdo mal formatado")));

        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id);

            if (category == null)
                return NotFound(new ResultViewModel<Category>("E08X0B - Conteúdo não encontrado"));

            category.Name = model.Name;
            category.Description = model.Description;
            category.CrdDeb = model.CrdDeb;
            category.VisualOrder = model.VisualOrder;
            category.Fixed = (model.Fixed == "true");
            category.System = (model.System == "true");

            context.Categories.Update(category);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Category>(category));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<Category>($"E08X0C - Falha interna no servidor: {ex.InnerException?.Message}"));
        }
    }

    [HttpDelete("v1/category/{id:int}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] int id,
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<string>("E08X0D - Não autorizado", false));

        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors("E08X0E - Conteúdo mal formatado")));

        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id);

            if (category == null)
                return NotFound(new ResultViewModel<Category>("E08X0F - Conteúdo não localizado"));

            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Category>($"A categoria {category.Name} foi excluída"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<Category>($"E08X10 - Falha interna no servidor: {ex.InnerException?.Message}"));
        }
    }

    [HttpPut("v1/category/active/{id:int}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> UpdateActiveAsync(
        [FromRoute] int id,
        [FromServices] MoneyDataContext context)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return Unauthorized(new ResultViewModel<Category>("E08X11 - Não autorizado"));

        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (category == null)
                return NotFound(new ResultViewModel<Category>("E08X12 - Conteúdo não encontrado"));

            category.Active = !category.Active;

            context.Categories.Update(category);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Category>(category));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<Category>($"E08X13 - Falha interna no servidor: {ex.InnerException?.Message}"));
        }
    }
}