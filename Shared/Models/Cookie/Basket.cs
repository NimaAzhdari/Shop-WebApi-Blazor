

public class OrderItem
{
public Guid ProductId{get;set;}
public int Quantity { get; set; }
public decimal Price{get;set;}
public string? Name { get; set; }
public string? ImageUrl { get; set; }
public decimal TotalValue => Price*Quantity;

}

public class Basket 
{
    public List<OrderItem> Items { get; set; }=new();
}