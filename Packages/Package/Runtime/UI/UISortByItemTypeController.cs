using MAG.Unity.ProductCatalogAPI.Runtime.Base;
using MAG.Unity.ProductCatalogAPI.Runtime.Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MAG.Unity.ProductCatalogAPI.Runtime.UI
{
    public class UISortByItemTypeController : UIListControllerBase<ItemType>
    {
        #region Events

        public event ItemTypeArrayEventHandler OnSortByItemTypeConfirmed;

        #endregion

        #region Components

        private UIConfirmCancelController _confirmCancelController;

        #endregion

        #region UIListControllerBase Implementations

        public override string PrefabPathElement => "Prefabs/UI/Element_ItemTypeOrder";

        protected override void InitializeElements(IEnumerable<ItemType> data)
        {
            // Set the ItemType for each element. Length of itemTypes should be equal to the child count of _transformContent.
            for (int i = 0; i < _transformContent.childCount; i++)
            {
                UIElementItemTypeOrderController currentElement = _transformContent.GetChild(i).GetComponent<UIElementItemTypeOrderController>();
                currentElement.Initialize(data.ElementAt(i));
            }
        }

        protected override void InitializeComponents()
        {
            base.InitializeComponents();

            //This component will requires Confirm event to apply sorting.  
            _confirmCancelController = GetComponent<UIConfirmCancelController>();
            if(_confirmCancelController == null)
                _confirmCancelController = gameObject.AddComponent<UIConfirmCancelController>();

            _confirmCancelController.OnConfirmClicked += ConfirmClicked;
        }

        #endregion

        #region Callback Methods

        private void ConfirmClicked()
        {
            ItemType[] itemTypes = new ItemType[_transformContent.childCount];

            // Every single child in _transformContent should represent a single ItemType in the enum.
            // Element count of ItemType enum must be equal to the children count of _transformContent.
            // Every child transform should have a UIElementItemTypeOrderController component attached to it.
            try
            {
                for (int i = 0; i < _transformContent.childCount; i++)
                {
                    UIElementItemTypeOrderController currentElement = _transformContent.GetChild(i).GetComponent<UIElementItemTypeOrderController>();
                    itemTypes[i] = currentElement.ItemType;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"UISortByItemTypeController.ConfirmClicked Error: {ex.Message}");
            }

            OnSortByItemTypeConfirmed?.Invoke(itemTypes);
        }

        #endregion
    }
}