using MAG.Unity.ProductCatalogAPI.Runtime.Base.Enums;
using MAG.Unity.ProductCatalogAPI.Runtime.Base.Interface;
using System;
using System.Collections.Generic;

namespace MAG.Unity.ProductCatalogAPI.Runtime.Base
{
    [Serializable]
    public abstract class MarketElementBase : IMarketElement
    {
        #region Variables  

        public string Name;
        public string Description;
        public float Price;

        #endregion

        #region IMarketElement Implementations  

        string IMarketElement.Name => Name;

        string IMarketElement.Description => Description;

        float IMarketElement.Price => Price;

        public abstract IEnumerable<ItemType> ItemTypes { get; }

        #endregion
    }
}