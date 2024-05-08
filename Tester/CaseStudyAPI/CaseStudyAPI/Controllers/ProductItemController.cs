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
    public class ProductItemController : ControllerBase
    {
        readonly AppDbContext _db;
        public ProductItemController(AppDbContext context)
        {
            _db = context;
        }
        [HttpGet]
        [Route("{brnid}")]
        public async Task<ActionResult<List<Products>>> Index(int brnid)
        {
            ProductItemDAO dao = new(_db);
            List<Products> itemsForBrand = await dao.GetAllByCategory(brnid);
            return itemsForBrand;
        }
    }
}