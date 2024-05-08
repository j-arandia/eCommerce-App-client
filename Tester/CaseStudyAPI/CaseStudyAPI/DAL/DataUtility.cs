
using CaseStudyAPI.DAL.DomainClasses;
using System.Text.Json;
namespace CaseStudyAPI.DAL
{
    public class DataUtility
    {
        private readonly AppDbContext _db;
        public DataUtility(AppDbContext context)
        {
            _db = context;
        }

        public async Task<bool> LoadNutritionInfoFromWebToDb(string stringJson)
        {
            bool brandLoaded = false;
            bool productLoaded = false;
            try
            {
                // an element that is typed as dynamic is assumed to support any operation
                dynamic? objectJson = JsonSerializer.Deserialize<Object>(stringJson);
                brandLoaded = await LoadBrands(objectJson);
                productLoaded = await LoadProducts(objectJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return brandLoaded && productLoaded;
        }

        private async Task<bool> LoadBrands(dynamic jsonObjectArray)
        {
            bool loadedBrand = false;
            try
            {
                // clear out the old rows
                _db.Brand?.RemoveRange(_db.Brand);
                await _db.SaveChangesAsync();
                List<String> allBrand = new();
                foreach (JsonElement element in jsonObjectArray.EnumerateArray())
                {
                    if (element.TryGetProperty("BRND", out JsonElement productsJson))
                    {
                        allBrand.Add(productsJson.GetString()!);
                    }
                }
                IEnumerable<String> brand = allBrand.Distinct<String>();
                foreach (string braname in brand)
                {
                    Brands bra = new();
                    bra.Name = braname;
                    await _db.Brand!.AddAsync(bra);
                    await _db.SaveChangesAsync();
                }
                loadedBrand = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
            }
            return loadedBrand;
        }
        private async Task<bool> LoadProducts(dynamic jsonObjectArray)
        {
            bool loadedItems = false;
            try
            {
                List<Brands> brand = _db.Brand!.ToList();
                // clear outthe old
                _db.Product?.RemoveRange(_db.Product);
                await _db.SaveChangesAsync();
                foreach (JsonElement element in jsonObjectArray.EnumerateArray())
                {
                    Products item = new();
                    item.Id = element.GetProperty("ID").GetString();
                    item.ProductName = element.GetProperty("PRDCTN").GetString();
                    item.GraphicName = element.GetProperty("GRPHCN").GetString();
                    item.CostPrice = Convert.ToDecimal(element.GetProperty("COSTP").GetString());
                    item.MSRP = Convert.ToDecimal(element.GetProperty("MSRP").GetString());
                    item.QtyOnHand = Convert.ToInt32(element.GetProperty("QTYOH").GetString());
                    item.QtyOnBackOrder = Convert.ToInt32(element.GetProperty("QTYOBO").GetString());
                    item.Description = element.GetProperty("DESC").GetString();
                    string? bra = element.GetProperty("BRND").GetString();
                    // add the FK here
                    foreach (Brands brands in brand)
                    {
                        if (brands.Name == bra)
                        {
                            item.Brands = brands;
                            break;
                        }
                    }
                    await _db.Product!.AddAsync(item);
                    await _db.SaveChangesAsync();
                }
                loadedItems = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
            }
            return loadedItems;
        }
    }
}