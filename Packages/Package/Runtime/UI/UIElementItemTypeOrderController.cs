using MAG.Unity.ProductCatalogAPI.Runtime.Base;
using MAG.Unity.ProductCatalogAPI.Runtime.Base.Enums;
using TMPro;
using UnityEngine.UI;

namespace MAG.Unity.ProductCatalogAPI.Runtime.UI
{
    public class UIElementItemTypeOrderController : UIControllerBase
    {
        #region Components

        private TMP_Text _txtName;
        private Button _btnUp;
        private Button _btnDown;

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

        #endregion

        #region UIControllerBase Implementations

        protected override void InitializeComponents()
        {
            _txtName = transform.FindDeepChild("Txt_Name")?.GetComponent<TMP_Text>();
            _btnUp = transform.FindDeepChild("Btn_Up")?.GetComponent<Button>();
            _btnDown = transform.FindDeepChild("Btn_Down")?.GetComponent<Button>();

            _btnUp?.onClick.AddListener(() => ChangeOrder(transform.GetSiblingIndex() - 1));
            _btnDown?.onClick.AddListener(() => ChangeOrder(transform.GetSiblingIndex() + 1));
        }

        #endregion

        #region Public Methods

        public void Initialize(ItemType itemType)
        {
            Initialize();

            _itemType = itemType;
            gameObject.name = $"Element_ItemTypeOrder_{_itemType.ToString()}";

            if (_txtName != null)
                _txtName.text = _itemType.ToString();
        }

        #endregion

        #region Private Methods

        private void ChangeOrder(int nextIndex)
        {
            //This UI element must always have a parent which will act as a container to determine its order in the list.
            if (transform.parent == null)
                return;

            // Can't move further up if is already on top or further down if is already on bottom.
            if (nextIndex < 0 || nextIndex == transform.parent.childCount)
                return;

            transform.SetSiblingIndex(nextIndex);
        }

        #endregion
    }
}