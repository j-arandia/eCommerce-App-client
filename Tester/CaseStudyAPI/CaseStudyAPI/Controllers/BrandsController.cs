using CaseStudyAPI.DAL;
using CaseStudyAPI.DAL.DAO;
using CaseStudyAPI.DAL.DomainClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace CaseStudyAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        readonly AppDbContext _db;
        public BrandsController(AppDbContext context)
        {
            _db = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Brands>>> Index()
        {
            BrandsDAO dao = new(_db);
            List<Brands> allBrands = await dao.GetAll();
            return allBrands;
        }
    }
}