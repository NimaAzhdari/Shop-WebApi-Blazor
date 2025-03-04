using System.ComponentModel.DataAnnotations;



public class ProductDto
{
    
    [Required(ErrorMessage = "لطفا مقدار دهی کنید")]
    public string Name { get; set; }
    
    public string Description { get; set; }

    [Required(ErrorMessage = "لطفا مقدار دهی کنید")]
    [Range(0, double.MaxValue, ErrorMessage = "مقدار صحیح وارد کنید")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "لطفا مقدار دهی کنید")]
    [Range(0, int.MaxValue, ErrorMessage = "مقدار صحیح وارد کنید")]
    public int Stock { get; set; }
}