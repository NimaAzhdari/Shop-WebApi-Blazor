using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserProduct
{
    [Key]
    public int Id{get;set;}

    [ForeignKey("AspNetUsers")]
    public Guid User { get; set; }

    [ForeignKey("Product")]
    public Guid Product { get; set; }
    public DateTime PurchaseDate { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal TotalAmount { get; set; }
}