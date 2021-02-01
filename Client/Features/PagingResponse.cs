using System.Collections.Generic;
using LCPECommerce.Shared.Entities.ReqFeatures;

namespace LCPECommerce.Client.Features
{
    public class PagingResponse<T> where T : class
    {
        public List<T> Items { get; set; }
        public MetaData MetaData { get; set; }
    }
}
