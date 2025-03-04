

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

public class JwtToken : IJwtToken
{
    private readonly IConfiguration _config;
    public JwtToken(IConfiguration config)
    {
        _config=config;
    }
    public string Generate(IdentityUser user)
    {
        var tokenhandller=new JwtSecurityTokenHandler();
        var key=Encoding.ASCII.GetBytes(_config["JwtSetting : SecretKey"]);
        var tokenDescriptor=new SecurityTokenDescriptor
        {
            Subject=new ClaimsIdentity(new Claim[]{
                new Claim(ClaimTypes.Name,user.UserName)
            }),
            Expires=DateTime.UtcNow.AddDays(8),
            SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
        };
        var token=tokenhandller.CreateToken(tokenDescriptor);

        return tokenhandller.WriteToken(token);
    }
}