using MAG.Unity.ProductCatalogAPI.Runtime.Base.Enums;
using Unity.Plastic.Newtonsoft.Json;

namespace MAG.Unity.ProductCatalogAPI.Runtime.Base.Interface
{
    public interface IItem
    {
        [JsonIgnore]
        public ItemType ItemType { get; }

        [JsonIgnore]
        public int Amount { get; }
    }
}