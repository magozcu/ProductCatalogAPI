using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json;

namespace MAG.Unity.ProductCatalogAPI.Runtime.Base.Interface
{
    public interface IMarketElement
    {
        [JsonIgnore]
        public string Name { get; }
        [JsonIgnore]
        public string Description { get; }
        [JsonIgnore]
        public float Price { get; }
        [JsonIgnore]
        public IEnumerable<IItem> Items { get; }
    }
}