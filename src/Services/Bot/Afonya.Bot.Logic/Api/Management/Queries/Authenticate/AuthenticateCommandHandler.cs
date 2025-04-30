using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Afonya.Bot.Domain.Entities;
using Afonya.Bot.Domain.Repositories;
using Common.Options;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Api.Management.Queries.Authenticate;

public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, AuthenticateResponse?>
{

    private readonly IUserRepository _userRepository;
    private readonly AppSettings _appSettings;

    public AuthenticateCommandHandler(IUserRepository userRepository, IOptions<AppSettings> appSettings)
    {
        _userRepository = userRepository;
        _appSettings = appSettings.Value;
    }

    public async Task<AuthenticateResponse?> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return null;

        var user = _userRepository.GetByName(request.Username);
        if (user == null) return null;

        var dto = new UserDto(user.Id.ToString(), user.Login);
        var token = await GenerateJwtToken(user);
        
        return new AuthenticateResponse(dto, token);
    }

    private async Task<string> GenerateJwtToken(TelegramUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = await Task.Run(() =>
        {

            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity([new Claim("id", user.Id.ToString()), new Claim("name", user.Login)]),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            return tokenHandler.CreateToken(tokenDescriptor);
        });

        return tokenHandler.WriteToken(token);
    }
}
