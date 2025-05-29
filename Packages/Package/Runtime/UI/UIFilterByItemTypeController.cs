using MAG.Unity.ProductCatalogAPI.Runtime.Base;
using MAG.Unity.ProductCatalogAPI.Runtime.Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MAG.Unity.ProductCatalogAPI.Runtime.UI
{
    public class UIFilterByItemTypeController : UIConfirmCancelControllerBase
    {
        #region Events

        public event ItemTypeArrayEventHandler OnFilterByItemTypeConfirmed;

        #endregion

        #region Prefabs

        private readonly string _prefabPathElementItemTypeFilter = "Prefabs/UI/Element_ItemTypeFilter";

        #endregion

        #region Variables

        private ItemType[] _allItemTypes;

        #endregion

        #region Public Methods

        public void EnablePanel(ItemType[] enabledItemTypes)
        {
            Initialize();

            CreateItemTypeFilterElements(_allItemTypes, enabledItemTypes);

            gameObject.SetActive(true);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates or updates the item type order elements count in the content transform based on the provided item types.
        /// </summary>
        /// <param name="allItemTypes"></param>
        private void CreateItemTypeFilterElements(ItemType[] allItemTypes, ItemType[] enabledItemTypes)
        {
            try
            {
                int createdItemTypeFilterElementCount = _transformContent.childCount;

                // Too many elements exist. Remove the excess.
                if (createdItemTypeFilterElementCount > allItemTypes.Length)
                {
                    for (int i = 0; i < createdItemTypeFilterElementCount - allItemTypes.Length; i++)
                    {
                        Destroy(_transformContent.GetChild(0).gameObject);
                    }
                }
                // Not enough elements exist. Create the missing ones.
                else if (createdItemTypeFilterElementCount < allItemTypes.Length)
                {
                    for (int i = 0; i < allItemTypes.Length - createdItemTypeFilterElementCount; i++)
                    {
                        GameObject goNewElement = Instantiate(Resources.Load<GameObject>(_prefabPathElementItemTypeFilter), _transformContent);
                    }
                }

                // Set the ItemType for each element. Length of itemTypes should be equal to the child count of _transformContent.
                // Passes the enabled value as true if the current item type is in the enabledItemTypes array.
                for (int i = 0; i < _transformContent.childCount; i++)
                {
                    UIElementItemTypeFilterController currentElement = _transformContent.GetChild(i).GetComponent<UIElementItemTypeFilterController>();
                    currentElement.Initialize(allItemTypes[i], enabledItemTypes.Contains(allItemTypes[i]));
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"UIFilterByItemTypeController.CreateItemTypeFilterElements Error: {ex.Message}");
            }
        }

        #endregion

        #region UIControllerBase Implementation

        protected override void InitializeComponents()
        {
            base.InitializeComponents();

            _allItemTypes = Enum.GetValues(typeof(ItemType)).Cast<ItemType>().ToArray();

            _btnConfirm?.onClick.AddListener(ConfirmClicked);
        }

        protected override void ConfirmClicked()
        {
            List<ItemType> enabledItemTypes = new List<ItemType>();

            // Every single child in _transformContent should represent a single ItemType in the enum.
            // Element count of ItemType enum must be equal to the children count of _transformContent.
            // Every child transform should have a UIElementItemTypeFilterController component attached to it.
            try
            {
                for (int i = 0; i < _transformContent.childCount; i++)
                {
                    UIElementItemTypeFilterController currentElement = _transformContent.GetChild(i).GetComponent<UIElementItemTypeFilterController>();

                    if (currentElement.Enabled)
                        enabledItemTypes.Add(currentElement.ItemType);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"UISortByItemTypeController.ConfirmClicked Error: {ex.Message}");
            }

            OnFilterByItemTypeConfirmed?.Invoke(enabledItemTypes.ToArray());
            gameObject.SetActive(false);
        }

        #endregion
    }
}