
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
//-----------------
private readonly ApplicationDbContex db;

public HomeController( ApplicationDbContex _db)
{
    db=_db;
}
//-------------------------
[HttpGet("SlideBarImages")]
public async Task SlideBarImages()
{

}

}