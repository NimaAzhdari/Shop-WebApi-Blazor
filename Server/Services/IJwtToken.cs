
using Microsoft.AspNetCore.Identity;

public interface IJwtToken
{
    string Generate(IdentityUser user);
}