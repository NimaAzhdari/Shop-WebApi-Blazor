using System.ComponentModel.DataAnnotations;

public class Product
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(500)]
    public string Description { get; set; }
    public string? Category { get; set; }

    [StringLength(50)]
    public string? ProductCode { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
    public string ImageURL { get; set; }
}