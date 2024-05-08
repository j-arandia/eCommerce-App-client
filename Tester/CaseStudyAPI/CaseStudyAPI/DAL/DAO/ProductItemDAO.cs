using CaseStudyAPI.DAL.DomainClasses;
using Microsoft.EntityFrameworkCore;
namespace CaseStudyAPI.DAL.DAO
{
    public class ProductItemDAO
    {
        private readonly AppDbContext _db;
        public ProductItemDAO(AppDbContext ctx)
        {
            _db = ctx;
        }
        public async Task<List<Products>> GetAllByCategory(int id)
        {
            return await _db.Product!.Where(item => item.Brands!.Id == id).ToListAsync();
        }
    }
}