using CaseStudyAPI.DAL.DomainClasses;
using Microsoft.EntityFrameworkCore;
namespace CaseStudyAPI.DAL.DAO
{
    public class BrandsDAO
    {
        private readonly AppDbContext _db;
        public BrandsDAO(AppDbContext ctx)
        {
            _db = ctx;
        }
        public async Task<List<Brands>> GetAll()
        {
            return await _db.Brand!.ToListAsync();
        }
    }
}