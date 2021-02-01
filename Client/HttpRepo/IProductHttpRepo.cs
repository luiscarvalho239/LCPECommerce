using LCPECommerce.Client.Features;
using LCPECommerce.Shared.Entities.ReqFeatures;
using LCPECommerce.Shared.Models;
using System.Threading.Tasks;

namespace LCPECommerce.Client.HttpRepo
{
    public interface IProductHttpRepo
    {
        Task<PagingResponse<Products>> GetProducts(ProductParameters productParameters);
    }
}
