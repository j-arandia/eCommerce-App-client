using CaseStudyAPI.DAL.DomainClasses;
using Microsoft.EntityFrameworkCore;

namespace CaseStudyAPI.DAL.DAO
{
    public class CustomerDAO
    {
        private readonly AppDbContext _db;
        public CustomerDAO(AppDbContext ctx)
        {
            _db = ctx;
        }
        public async Task<Customer> Register(Customer cust)
        {
            await _db.Customer!.AddAsync(cust);
            await _db.SaveChangesAsync();
            return cust;
        }
        public async Task<Customer?> GetByEmail(string? email)
        {
            Customer? cust = await _db.Customer!.FirstOrDefaultAsync(u => u.Email == email);
            return cust;
        }
    }
}
