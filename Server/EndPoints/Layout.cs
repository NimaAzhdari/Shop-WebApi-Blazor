using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class LayoutController:ControllerBase
{

    [HttpGet("NavbarData")]
    public IActionResult NavBarData()
    {

        if (User.Identity.IsAuthenticated)
        {
            if(User.IsInRole("User"))
            {
                var data=new[]
                {
                    new NavBar{Name="محصولات",Href="GetProduct"},
                    new NavBar{Name="سبد خرید",Href="Orders"},
                    new NavBar{Name="خروج",Href="Logout"},

                };
                return Ok(data);
            }
            else if(User.IsInRole("Admin"))
            {
                var data=new[]
                {
                    new NavBar{Name="اضافه کردن محصول",Href="AddProduct"},
                    new NavBar{Name="ویرایش محصول",Href="GetProductAdmin"},
                    new NavBar{Name="چارت",Href="Chart"},
                    new NavBar{Name="خروج",Href="Logout"},

                    
                };
                return Ok(data);
                
            }
        }
        else//not authenticate
        {
            var data=new[]
                {
                    new NavBar{Name="ورود",Href="login"},
                    new NavBar{Name="ثبت نام",Href="signup"},
                    new NavBar{Name="ورود ادمین",Href="loginAdmin"}
                };
                return Ok(data);
        }

        return Ok();
    }
    [HttpGet("Status")]
    public IActionResult AuthenStatus()
    {
        if (User.Identity.IsAuthenticated)
        {
            var username = User.Identity?.Name;
        }
        else
        {}
        return Ok();
    }
}