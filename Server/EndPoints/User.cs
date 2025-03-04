
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;




[Route("api/[controller]")]
[ApiController]
public class User:ControllerBase
{
    public string filePath = "logs/log.txt";
    private readonly ApplicationDbContex db;
    public User( ApplicationDbContex _db)
    {
        db=_db;
    }
    [HttpGet("GetProduct")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
    {

        return await db.Products.ToListAsync();
    }
    [HttpGet("GetProductById/{id:guid}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {   
    var product = await db.Products.FindAsync(id);
    
    if (product == null)
    {
        
        return NotFound();
    }
    
    
    return Ok(product);
 
    }
[Authorize(Policy ="UserOnly")]
[HttpPost("ManageBasketCookie")]
    public IActionResult ManageBasketCookie([FromBody]Basket basket)
    {
        Basket existingBasket=new();
        if(Request.Cookies["Basket"] is not null)
        {
        existingBasket=JsonSerializer.Deserialize<Basket>(Request.Cookies["Basket"]);
        }

        foreach (var newItem in basket.Items)
        {
        var existingItem = existingBasket.Items.FirstOrDefault(item => item.ProductId == newItem.ProductId);
        if (existingItem != null)
        {
            existingItem.Quantity += newItem.Quantity;
        }
        else
        {
            existingBasket.Items.Add(newItem);
        }
        }

        var updatedBasketJson=JsonSerializer.Serialize<Basket>(existingBasket);
        Response.Cookies.Append("Basket",updatedBasketJson);
        return Ok();
    }
    [Authorize(Policy ="UserOnly")]
    [HttpGet("GetBasket")]
    public IActionResult GetBasket()
    {
        Basket existingBasket=new();
        if(Request.Cookies["Basket"] is not null)
        {
        existingBasket=JsonSerializer.Deserialize<Basket>(Request.Cookies["Basket"]);
        return Ok(existingBasket);
        }
        return NotFound();
    }
    [Authorize(Policy ="UserOnly")]
    [HttpPost("SubmitBuy")]
    public async Task<IActionResult> SubmitBuy([FromBody]Basket basket)
    {
        if (basket == null || basket.Items == null )
        {
             return BadRequest();
        }
        string userIdString=User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid UserId=Guid.Parse(userIdString);

        List<UserProduct> row=new();
        foreach (var item in basket.Items)
        {
            row.Add(new UserProduct{User=UserId,Product=item.ProductId,PurchaseDate=DateTime.Now,Quantity=item.Quantity,Price=item.Price,TotalAmount=item.TotalValue});
        }
        await db.UserProducts.AddRangeAsync(row);
        await db.SaveChangesAsync();
        Response.Cookies.Delete("Basket");
        return Ok();
    }
}