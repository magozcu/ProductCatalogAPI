using MAG.Unity.ProductCatalogAPI.Runtime.Base.Interface;
using System;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json;

namespace MAG.Unity.ProductCatalogAPI.Runtime.Base
{
    [Serializable]
    public abstract class MarketElementBase : IMarketElement
    {
        #region Variables  

        [JsonProperty]
        private string _discriminator;
        [JsonProperty]
        protected string _name;
        [JsonProperty]
        protected string _description;
        [JsonProperty]
        protected float _price;

        #endregion

        #region Constructors

        public MarketElementBase()
        {
            _discriminator = GetType().FullName;
        }

        public MarketElementBase(string name, string description, float price) : this()
        {
            _name = name;
            _description = description;
            _price = price;
        }

        #endregion

        #region IMarketElement Implementations  

        string IMarketElement.Name => _name;

        string IMarketElement.Description => _description;

        float IMarketElement.Price => _price;

        public abstract IEnumerable<IItem> Items { get; }

        #endregion
    }
}