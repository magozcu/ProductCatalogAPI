using MAG.Unity.ProductCatalogAPI.Runtime.Base.Enums;
using System;
using System.Collections.Generic;

namespace MAG.Unity.ProductCatalogAPI.Runtime.Base
{
    [Serializable]
    public class Product : MarketElementBase
    {
        #region Variables

        public ItemType ItemType;
        public int Amount;

        #endregion

        #region MarketElementBase Implementations

        public override IEnumerable<ItemType> ItemTypes => new List<ItemType> { ItemType };

        #endregion
    }
}