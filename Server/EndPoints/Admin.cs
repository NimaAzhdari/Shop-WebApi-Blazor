using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;



[Route("api/[controller]")]
[ApiController]
[Authorize(Policy ="AdminOnly")]

public class AdminController:ControllerBase
{
    private readonly ApplicationDbContex db;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInMnager;

    public AdminController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager,ApplicationDbContex context)
    {
        _userManager=userManager;
        _signInMnager=signInManager;
        db=context;
    }

   
        [HttpPost("CreateProduct")]
        public async  Task<IActionResult> CreateProduct([FromForm] IFormFile File,[FromForm] string ProductJson)
        {
            if (File == null || File.Length == 0)
                return BadRequest("No file uploaded.");

            var ProductDto=JsonSerializer.Deserialize<ProductDto>(ProductJson);
            if (ProductDto == null)
                return BadRequest("Invalid product data.");

             var filePathdb=Path.Combine("Uploads", File.FileName).Replace("\\", "/");
             var filePath = Path.Combine("wwwroot","Uploads", File.FileName).Replace("\\", "/");
             Directory.CreateDirectory("Uploads");
             using (var stream = new FileStream(filePath, FileMode.Create))
                         {
                            await File.CopyToAsync(stream);
                         }

            Product product=new()
            {
                Id=new Guid(),
                Name=ProductDto.Name,
                Description=ProductDto.Description,
                Price=ProductDto.Price,
                Stock=ProductDto.Stock,
                ImageURL=filePathdb
            };
            db.Products.Add(product);
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product=await db.Products.FindAsync(id);
            if(product==null)
            {
                return NotFound();
            }
             db.Products.Remove(product);
             await db.SaveChangesAsync();
             return NoContent();

        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            db.Entry(product).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return Ok();
        }

         [HttpPut("UpdateProductWithImage")]
        public async Task<IActionResult> UpdateProduct([FromForm] IFormFile File,[FromForm] string ProductJson)
        {
            if (File == null || File.Length == 0)
                return BadRequest("No file uploaded.");

            var UpdateProduct=JsonSerializer.Deserialize<Product>(ProductJson);
            if (UpdateProduct == null)
                return BadRequest("Invalid product data.");

             var filePathdb=Path.Combine("Uploads", File.FileName).Replace("\\", "/");
             var filePath = Path.Combine("wwwroot","Uploads", File.FileName).Replace("\\", "/");
             Directory.CreateDirectory("Uploads");
             using (var stream = new FileStream(filePath, FileMode.Create))
                         {
                            await File.CopyToAsync(stream);
                         }

            Product product=new()
            {
                Id=UpdateProduct.Id,
                Name=UpdateProduct.Name,
                Description=UpdateProduct.Description,
                Price=UpdateProduct.Price,
                Stock=UpdateProduct.Stock,
                ImageURL=filePathdb
            };
            db.Entry(product).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("ChartData")]
        public async Task<IActionResult> ChartData()
        {
            List<ChartData> chartDatas=new();
            var result=await db.UserProducts.Select(o => new {o.PurchaseDate.Date,o.TotalAmount}).ToListAsync();


            var groupedResult = result
                .GroupBy(o => o.Date)  
                .Select(g => new 
                {
                    PurchaseDate = g.Key,       
                    TotalAmountSum = g.Sum(o => o.TotalAmount) 
                })
                .ToList();
            foreach(var row in groupedResult)
            {
                chartDatas.Add(new ChartData{X=row.PurchaseDate,Y=row.TotalAmountSum});
            }
            return Ok(chartDatas);



        }


}