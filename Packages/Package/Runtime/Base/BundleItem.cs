using MAG.Unity.ProductCatalogAPI.Runtime.Base.Enums;
using System;

namespace MAG.Unity.ProductCatalogAPI.Runtime.Base
{
    [Serializable]
    public class BundleItem
    {
        #region Variables

        public ItemType ItemType;
        public int Amount;

        #endregion
    }
}