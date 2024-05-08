using Microsoft.EntityFrameworkCore;
using CaseStudyAPI.DAL.DomainClasses;
namespace CaseStudyAPI.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public virtual DbSet<Products>? Product { get; set; }
        public virtual DbSet<Brands>? Brand { get; set; }
        public virtual DbSet<Customer>? Customer { get; set; }
        public virtual DbSet<Order>? Orders { get; set; }
        public virtual DbSet<OrderLineItem>? LineItem { get; set; }
        public virtual DbSet<Branch>? Branches{ get; set; }
    }
}