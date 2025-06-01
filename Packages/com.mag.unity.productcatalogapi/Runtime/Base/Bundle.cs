using MAG.Unity.ProductCatalogAPI.Runtime.Base.Interface;
using System;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json;

namespace MAG.Unity.ProductCatalogAPI.Runtime.Base
{
    [Serializable]
    public class Bundle : MarketElementBase
    {
        #region Variables

        [JsonProperty]
        private List<Item> _items;

        #endregion

        #region Constructors

        public Bundle() : base()
        {
            _items = new List<Item>();
        }

        public Bundle(string name, string description, float price, List<Item> items) : base(name, description, price)
        {
            _items = items ?? new List<Item>();
        }

        #endregion

        #region MarketElementBase Implementations

        public override IEnumerable<IItem> Items => _items;

        #endregion
    }
}