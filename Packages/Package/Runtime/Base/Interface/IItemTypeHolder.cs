using MAG.Unity.ProductCatalogAPI.Runtime.Base.Enums;
using System.Collections.Generic;

namespace MAG.Unity.ProductCatalogAPI.Runtime.Base.Interface
{
    public interface IItemTypeHolder
    {
        IEnumerable<ItemType> ItemTypes { get; }
    }
}