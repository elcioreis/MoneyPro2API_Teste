using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyPro2.Data;
using MoneyPro2.Extensions;
using MoneyPro2.Models;
using MoneyPro2.Services;
using MoneyPro2.Tools;
using MoneyPro2.ViewModel;
using MoneyPro2.ViewModels.Users;
using System.Security.Claims;

namespace MoneyPro2.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    [HttpPost("v1/register")]
    public async Task<IActionResult> RegisterAsync(
        [FromBody] RegisterViewModel model,
        [FromServices] MoneyDataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors("E02X00 - Conteúdo mal formatado")));

        var user = new User()
        {
            Name = model.Name,
            Email = model.Email,
            ControlStart = model.ControlStart,
            Slug = model.Email.Replace("@", "-").Replace(".", "-"),
            PasswordHash = Hash.Generator(new LoginViewModel() { Email = model.Email, Password = model.Password }),
        };

        var role = context.Roles.FirstOrDefault(x => x.Slug == "user");
        if (role != null)
            user.Roles.Add(role);

        var entry = new Entry()
        {
            Name = "Abertura de Balanço",
            Description = "Abertura de Balanço",
            Active = true,
            System = true
        };

        using var dbTransaction = context.Database.BeginTransaction();
        try
        {
            await context.Users.AddAsync(user);
            entry.User = user;
            await context.Entries.AddAsync(entry);
            await context.SaveChangesAsync();
            dbTransaction.Commit();
            return Ok(new ResultViewModel<User>(user));
        }
        catch (DbUpdateException)
        {
            dbTransaction.Rollback();
            return StatusCode(400, new ResultViewModel<User>("E02X01 - Este E-mail já está cadastrado"));
        }
        catch (Exception)
        {
            dbTransaction.Rollback();
            return StatusCode(500, new ResultViewModel<User>("E02X02 - Falha interna no servidor"));
        }
    }

    [HttpPost("v1/login")]
    public async Task<IActionResult> LoginAsync(
        [FromBody] LoginViewModel model,
        [FromServices] MoneyDataContext context,
        [FromServices] TokenService tokenService)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors("E02X03 - Conteúdo mal formatado")));

        var user = await context
            .Users
            .AsNoTracking()
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Email == model.Email);

        if (user == null)
            return Unauthorized(new ResultViewModel<string>("Usuário ou senha inválidos"));

        if (user.PasswordHash != Hash.Generator(new LoginViewModel() { Email = model.Email, Password = model.Password }))
            return Unauthorized(new ResultViewModel<string>("Usuário ou senha inválidos"));

        try
        {
            RegisterLogin(context, user);

            var token = tokenService.GenerateToken(user);
            return Ok(new ResultViewModel<string>(token, null));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<User>("E02X04 - Falha interna no servidor"));
        }
    }

    [HttpGet("v1/logintest")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> GetAsync(
        [FromServices] MoneyDataContext context)
    {
        if (int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int id))
            return Unauthorized(new ResultViewModel<User>("E02X05 - Não autorizado"));

        try
        {
            var user = await context
                .Users
                .AsNoTracking()
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return Unauthorized(new ResultViewModel<User>("E02X06 - Não autorizado"));

            return Ok(new ResultViewModel<User>(user));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<User>("E02X07 - Falha interna no servidor"));
        }
    }

    private void RegisterLogin(MoneyDataContext context, User user)
    {
        var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

        var login = new Login()
        {
            UserId = user.Id,
            LoginDate = DateTime.UtcNow,
            IpAddress = remoteIpAddress?.ToString() ?? "ignorado"
        };

        context.Logins.Add(login);
        context.SaveChangesAsync();
    }
}