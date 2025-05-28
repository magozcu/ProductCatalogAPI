using MAG.Unity.ProductCatalogAPI.Runtime.Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MAG.Unity.ProductCatalogAPI.Runtime.Base
{
    [Serializable]
    public class Bundle : MarketElementBase
    {
        #region Variables
     
        public List<BundleItem> Items;

        #endregion

        #region MarketElementBase Implementations

        public override IEnumerable<ItemType> ItemTypes => Items?.Select(a => a.ItemType).Distinct().ToList();

        #endregion
    }
}