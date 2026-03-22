using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DentFlow.Domain.Identity;
using DentFlow.Infrastructure.Persistence;

namespace DentFlow.Identity.Endpoints;

[AllowAnonymous]
public class LoginEndpoint(
    UserManager<ApplicationUser> userManager,
    ApplicationDbContext db,
    IConfiguration configuration)
    : Endpoint<LoginRequest, LoginResponse>
{
    public override void Configure()
    {
        Post("/auth/login");
        AllowAnonymous();
        Version(1);
        Summary(s =>
        {
            s.Summary = "Login";
            s.Description = "Authenticate with email and password to receive a JWT access token.";
        });
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(req.Email);

        if (user is null || !user.IsActive)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var passwordValid = await userManager.CheckPasswordAsync(user, req.Password);
        if (!passwordValid)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var roles = await userManager.GetRolesAsync(user);

        var signingKey = configuration["Jwt:SigningKey"]!;
        var expiryMinutes = int.TryParse(configuration["Jwt:AccessTokenExpiryMinutes"], out var m) ? m : 15;

        var token = JwtBearer.CreateToken(o =>
        {
            o.SigningKey = signingKey;
            o.ExpireAt = DateTime.UtcNow.AddMinutes(expiryMinutes);
            o.User.Roles.AddRange(roles);
            o.User.Claims.Add(("sub", user.Id.ToString()));
            o.User.Claims.Add(("email", user.Email!));
            o.User.Claims.Add(("name", user.FullName));
            if (user.TenantId.HasValue)
                o.User.Claims.Add(("tid", user.TenantId.Value.ToString()));
        });

        user.LastLoginAt = DateTime.UtcNow;
        await userManager.UpdateAsync(user);

        var language = await db.UserPreferences
            .Where(p => p.UserId == user.Id)
            .Select(p => p.Language)
            .FirstOrDefaultAsync(ct) ?? "en";

        await SendOkAsync(new LoginResponse(
            AccessToken: token,
            ExpiresIn: expiryMinutes * 60,
            UserId: user.Id,
            Email: user.Email!,
            FullName: user.FullName,
            Roles: [.. roles],
            Language: language
        ), ct);
    }
}

public record LoginRequest(string Email, string Password);

public record LoginResponse(
    string AccessToken,
    int ExpiresIn,
    Guid UserId,
    string Email,
    string FullName,
    List<string> Roles,
    string Language);

