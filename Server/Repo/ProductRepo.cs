using LCPECommerce.Server.Data;
using LCPECommerce.Shared.Entities.ReqFeatures;
using LCPECommerce.Server.Paging;
using LCPECommerce.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LCPECommerce.Server.Repo
{
    public class ProductRepo : IProductRepo
    {
        private readonly AppDbContext _context;

        public ProductRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedList<Products>> GetProducts(ProductParameters productParameters)
        {
            var products = await _context.Product.ToListAsync();

            return PagedList<Products>
                .ToPagedList(products, productParameters.PageNumber, productParameters.PageSize);
        }
    }
}
