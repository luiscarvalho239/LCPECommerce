using LCPECommerce.Client.Features;
using LCPECommerce.Shared.Entities.ReqFeatures;
using LCPECommerce.Shared.Models;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace LCPECommerce.Client.HttpRepo
{
    public class ProductHttpRepo : IProductHttpRepo
    {
        private readonly HttpClient _client;

        public ProductHttpRepo(HttpClient client)
        {
            _client = client;
        }

        public async Task<PagingResponse<Products>> GetProducts(ProductParameters productParameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = productParameters.PageNumber.ToString()
            };

            var response = await _client.GetAsync(QueryHelpers.AddQueryString("https://localhost:5001/api/products", queryStringParam));
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var pagingResponse = new PagingResponse<Products>
            {
                Items = JsonSerializer.Deserialize<List<Products>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                MetaData = JsonSerializer.Deserialize<MetaData>(response.Headers.GetValues("X-Pagination").First(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            };

            return pagingResponse;
        }
    }
}
