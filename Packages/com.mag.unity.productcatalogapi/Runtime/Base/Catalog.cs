using MAG.Unity.ProductCatalogAPI.Runtime.Base.Interface;
using System;
using System.Collections.Generic;

namespace MAG.Unity.ProductCatalogAPI.Runtime.Base
{
    [Serializable]
    public class Catalog
    {
        #region Variables

        public IEnumerable<IMarketElement> MarketElements;

        #endregion
    }
}