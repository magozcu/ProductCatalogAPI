using MAG.Unity.ProductCatalogAPI.Runtime.Base.Enums;
using MAG.Unity.ProductCatalogAPI.Runtime.Base.Interface;
using System;
using Unity.Plastic.Newtonsoft.Json;

namespace MAG.Unity.ProductCatalogAPI.Runtime.Base
{
    [Serializable]
    public class Item : IItem
    {
        #region Variables

        [JsonProperty]
        private ItemType _itemType;
        [JsonProperty]
        public int _amount;

        #endregion

        #region Constructors

        public Item(ItemType itemType, int amount)
        {
            _itemType = itemType;
            _amount = amount;
        }

        #endregion

        #region IItem Implementations

        public ItemType ItemType => _itemType;

        public int Amount => _amount;

        #endregion
    }
}