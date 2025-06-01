using MAG.Unity.ProductCatalogAPI.Runtime.Base.Enums;
using MAG.Unity.ProductCatalogAPI.Runtime.Base.Interface;
using System;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json;

namespace MAG.Unity.ProductCatalogAPI.Runtime.Base
{
    [Serializable]
    public class Product : MarketElementBase, IItem
    {
        #region Variables

        [JsonProperty]
        protected ItemType _itemType;
        [JsonProperty]
        protected int _amount;

        #endregion

        #region Constructors

        public Product() : base()
        {

        }

        public Product(string name, string description, float price, ItemType itemType, int amount) : base(name, description, price)
        {
            _itemType = itemType;
            _amount = amount;
        }

        #endregion

        #region MarketElementBase Implementations

        public override IEnumerable<IItem> Items => new List<IItem> { this };

        #endregion

        #region IItem Implementations

        public ItemType ItemType => _itemType;

        public int Amount => _amount;

        #endregion
    }
}