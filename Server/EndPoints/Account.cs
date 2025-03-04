
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AccountController:ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInMnager;

    public AccountController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager)
    {
        _userManager=userManager;
        _signInMnager=signInManager;

    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]LoginModel model)
        {
            var user=await _userManager.FindByNameAsync(model.UserName);
            if(user == null )//for not ok condition
            {
                return Unauthorized();
            }
            var result=await _signInMnager.CheckPasswordSignInAsync(user,model.Password,false);//check pass and set authen cookie 
            if(result.Succeeded)
            {
            if(!await _userManager.IsInRoleAsync(user,"User"))
            {
                await _userManager.AddToRoleAsync(user,"User");
            }
            await _signInMnager.SignInAsync(user,true);
            return Ok();
            }
            return BadRequest();
            
        }
    [HttpPost("signin")]
    public async Task<IActionResult> Signin([FromBody]SigninModel model)
        {
            var user=new IdentityUser{UserName=model.UserName,PhoneNumber=model.PhoneNumber};
            var result=await _userManager.CreateAsync(user,model.Password);
            if(result.Succeeded)
            return Ok();
            return BadRequest(result.Errors);
        }

    [HttpPost("test")]
    [Authorize(Policy ="UserOnly")]
    public  IActionResult Test()
    {
        return Ok("from admin test");
    }

    [HttpPost("logout")]
    public  IActionResult LogOut()
    {
        _signInMnager.SignOutAsync();
        return Ok();
    }
     [HttpPost("loginAdmin")]
    public async Task<IActionResult> LoginAdmin([FromBody]LoginModel model)
        {
            var user=await _userManager.FindByNameAsync(model.UserName);
            if(user == null )//for not ok condition
            {
                return Unauthorized();
            }
            var result=await _signInMnager.CheckPasswordSignInAsync(user,model.Password,false);//check pass and set authen cookie 
            if(result.Succeeded)
            {
            if(!await _userManager.IsInRoleAsync(user,"Admin"))
            {
                await _userManager.AddToRoleAsync(user,"Admin");
            }
            await _signInMnager.SignInAsync(user,true);
            return Ok();
            }
            return BadRequest();
        }

}
