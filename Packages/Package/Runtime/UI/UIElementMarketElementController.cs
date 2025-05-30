using MAG.Unity.ProductCatalogAPI.Runtime.Base;
using MAG.Unity.ProductCatalogAPI.Runtime.Base.Interface;
using System;
using System.Linq;
using TMPro;

namespace MAG.Unity.ProductCatalogAPI.Runtime.UI
{
    public class UIElementMarketElementController : UIControllerBase
    {
        #region Components

        private TMP_Text _txtName;
        private TMP_Text _txtDescription;
        private TMP_Text _txtPrice;
        private TMP_Text _txtItems;

        #endregion

        #region Variables

        public IMarketElement MarketElement
        {
            get
            {
                return _marketElement;
            }
        }
        private IMarketElement _marketElement;

        #endregion

        #region Public Methods

        public void Initialize(IMarketElement marketElement)
        {
            _marketElement = marketElement;

            if (_marketElement == null || _marketElement.Items == null || !_marketElement.Items.Any())
            {
                Destroy(gameObject);
                return;
            }

            if (_txtName != null)
                _txtName.text = _marketElement.Name;
            if (_txtDescription != null)
                _txtDescription.text = _marketElement.Description;
            if (_txtPrice != null)
                _txtPrice.text = _marketElement.Price.ToString();
            if (_txtItems != null)
                _txtItems.text = string.Join(Environment.NewLine, _marketElement.Items.Select(item => $"{item.ItemType.ToString()}: {item.Amount}"));
        }

        #endregion

        #region UIControllerBase Implementation

        protected override void InitializeComponents()
        {
            _txtName = transform.FindDeepChild("Txt_Name")?.GetComponent<TMP_Text>();
            _txtDescription = transform.FindDeepChild("Txt_Description")?.GetComponent<TMP_Text>();
            _txtPrice = transform.FindDeepChild("Txt_Price")?.GetComponent<TMP_Text>();
            _txtItems = transform.FindDeepChild("Txt_Items")?.GetComponent<TMP_Text>();
        }

        #endregion
    }
}