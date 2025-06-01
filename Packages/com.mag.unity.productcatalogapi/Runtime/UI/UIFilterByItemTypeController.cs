using MAG.Unity.ProductCatalogAPI.Runtime.Base;
using MAG.Unity.ProductCatalogAPI.Runtime.Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MAG.Unity.ProductCatalogAPI.Runtime.UI
{
    public class UIFilterByItemTypeController : UIListControllerBase<ItemType>
    {
        #region Events

        public event ItemTypeArrayEventHandler OnFilterByItemTypeConfirmed;

        #endregion

        #region Components

        private UIConfirmCancelController _confirmCancelController;

        #endregion

        #region Variables

        private ItemType[] _allItemTypes;

        #endregion

        #region UIListControllerBase Implementations

        public override string PrefabPathElement => "Prefabs/UI/Element_ItemTypeFilter";

        public override void EnablePanel(IEnumerable<ItemType> data)
        {
            Initialize();

            SynchronizeElementCount(_allItemTypes);
            InitializeElements(data);

            gameObject.SetActive(true);
        }

        protected override void InitializeElements(IEnumerable<ItemType> data)
        {
            // Set the ItemType for each element. Length of itemTypes should be equal to the child count of _transformContent.
            // Passes the enabled value as true if the current item type is in the enabledItemTypes array.
            for (int i = 0; i < _transformContent.childCount; i++)
            {
                UIElementItemTypeFilterController currentElement = _transformContent.GetChild(i).GetComponent<UIElementItemTypeFilterController>();
                currentElement.Initialize(_allItemTypes[i], data.Contains(_allItemTypes[i]));
            }
        }

        protected override void InitializeComponents()
        {
            base.InitializeComponents();

            //This component will requires Confirm event to apply filtering.  
            _confirmCancelController = GetComponent<UIConfirmCancelController>();
            if (_confirmCancelController == null)
                _confirmCancelController = gameObject.AddComponent<UIConfirmCancelController>();

            _confirmCancelController.OnConfirmClicked += ConfirmClicked;

            _allItemTypes = Enum.GetValues(typeof(ItemType)).Cast<ItemType>().ToArray();
        }

        #endregion

        #region Callback Methods

        private void ConfirmClicked()
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
        }

        #endregion
    }
}