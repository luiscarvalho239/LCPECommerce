using LCPECommerce.Shared.Entities.ReqFeatures;
using LCPECommerce.Server.Paging;
using LCPECommerce.Shared.Models;
using System.Threading.Tasks;

namespace LCPECommerce.Server.Repo
{
    public interface IProductRepo
    {
        Task<PagedList<Products>> GetProducts(ProductParameters productParameters);
    }
}
