

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContex :IdentityDbContext
{
    public DbSet<Product> Products {get;set;}
    public DbSet<UserProduct> UserProducts{get;set;}
     public ApplicationDbContex(DbContextOptions<ApplicationDbContex> options)
        : base(options)
    {
    }

   
}