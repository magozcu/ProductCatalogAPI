using MAG.Unity.ProductCatalogAPI.Runtime.Base;
using MAG.Unity.ProductCatalogAPI.Runtime.Base.Enums;
using System;
using UnityEngine;

namespace MAG.Unity.ProductCatalogAPI.Runtime.UI
{
    public class UISortByItemTypeController : UIConfirmCancelControllerBase
    {
        #region Events

        public event ItemTypeArrayEventHandler OnSortByItemTypeConfirmed;

        #endregion

        #region Prefabs

        private readonly string _prefabPathElementItemTypeOrder = "Prefabs/UI/Element_ItemTypeOrder";

        #endregion

        #region Variables

        private ItemType[] _itemTypes;

        #endregion

        #region Public Methods

        public void EnablePanel(ItemType[] itemTypes)
        {
            Initialize();

            _itemTypes = itemTypes;
            CreateItemTypeOrderElements(_itemTypes);

            gameObject.SetActive(true);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates or updates the item type order elements count in the content transform based on the provided item types.
        /// </summary>
        /// <param name="itemTypes"></param>
        private void CreateItemTypeOrderElements(ItemType[] itemTypes)
        {
            try
            {
                int createdItemTypeOrderElementCount = _transformContent.childCount;

                // Too many elements exist. Remove the excess.
                if (createdItemTypeOrderElementCount > itemTypes.Length)
                {
                    for (int i = 0; i < createdItemTypeOrderElementCount - itemTypes.Length; i++)
                    {
                        Destroy(_transformContent.GetChild(0).gameObject);
                    }
                }
                // Not enough elements exist. Create the missing ones.
                else if (createdItemTypeOrderElementCount < itemTypes.Length)
                {            
                    for (int i = 0; i < itemTypes.Length - createdItemTypeOrderElementCount; i++)
                    {
                        GameObject goNewElement = Instantiate(Resources.Load<GameObject>(_prefabPathElementItemTypeOrder), _transformContent);
                    }
                }

                // Set the ItemType for each element. Length of itemTypes should be equal to the child count of _transformContent.
                for(int i = 0; i < _transformContent.childCount; i++)
                {
                    UIElementItemTypeOrderController currentElement = _transformContent.GetChild(i).GetComponent<UIElementItemTypeOrderController>();
                    currentElement.Initialize(itemTypes[i]);
                }
            }
            catch(Exception ex)
            {
                Debug.LogError($"UISortByItemTypeController.CreateItemTypeOrderElements Error: {ex.Message}");
            }
        }

        #endregion

        #region UIControllerBase Implementations

        protected override void InitializeComponents()
        {
            base.InitializeComponents();

            _btnConfirm?.onClick.AddListener(ConfirmClicked);
        }

        protected override void ConfirmClicked()
        {
            // Every single child in _transformContent should represent a single ItemType in the enum.
            // Element count of ItemType enum must be equal to the children count of _transformContent.
            // Every child transform should have a UIElementItemTypeOrderController component attached to it.
            try
            {
                for (int i = 0; i < _transformContent.childCount; i++)
                {
                    UIElementItemTypeOrderController currentElement = _transformContent.GetChild(i).GetComponent<UIElementItemTypeOrderController>();
                    _itemTypes[i] = currentElement.ItemType;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"UISortByItemTypeController.ConfirmClicked Error: {ex.Message}");
            }

            OnSortByItemTypeConfirmed?.Invoke(_itemTypes);
            gameObject.SetActive(false);
        }

        #endregion
    }
}