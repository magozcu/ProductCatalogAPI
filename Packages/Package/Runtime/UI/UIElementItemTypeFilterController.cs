using MAG.Unity.ProductCatalogAPI.Runtime.Base;
using MAG.Unity.ProductCatalogAPI.Runtime.Base.Enums;
using TMPro;
using UnityEngine.UI;

namespace MAG.Unity.ProductCatalogAPI.Runtime.UI
{
    public class UIElementItemTypeFilterController : UIControllerBase
    {
        #region Components

        private TMP_Text _txtName;
        private Toggle _tglEnabled;

        #endregion

        #region Variables

        public ItemType ItemType
        {
            get
            {
                return _itemType;
            }
        }
        private ItemType _itemType;

        public bool Enabled
        {
            get
            {
                return _tglEnabled != null && _tglEnabled.isOn;
            }
        }

        #endregion

        #region UIControllerBase Implementation

        protected override void InitializeComponents()
        {
            _txtName = transform.FindDeepChild("Txt_Name")?.GetComponent<TMP_Text>();
            _tglEnabled = transform.FindDeepChild("Tgl_Enabled")?.GetComponent<Toggle>();
        }

        #endregion

        #region Public Methods

        public void Initialize(ItemType itemType, bool enabled)
        {
            Initialize();

            _itemType = itemType;
            gameObject.name = $"Element_ItemTypeFilter_{_itemType.ToString()}";

            if (_txtName != null)
                _txtName.text = _itemType.ToString();
            if(_tglEnabled != null)
                _tglEnabled.isOn = enabled;

            gameObject.SetActive(true);
        }

        #endregion
    }
}