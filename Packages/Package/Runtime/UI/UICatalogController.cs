using MAG.Unity.ProductCatalogAPI.Runtime.Base;
using MAG.Unity.ProductCatalogAPI.Runtime.Base.Enums;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MAG.Unity.ProductCatalogAPI.Runtime.UI
{
    #region Delegates

    public delegate void ItemTypeArrayEventHandler(ItemType[] itemTypes);

    #endregion

    public class UICatalogController : UIControllerBase
    {
        #region Components

        private UISortByItemTypeController _sortByItemTypeController;
        private UIFilterByItemTypeController _filterByItemTypeController;

        private Button _btnSortByItemType;
        private Button _btnFilterByItemType;

        #endregion

        #region Variables

        private ItemType[] _sortedItemTypes;
        private ItemType[] _filteredItemTypes;

        #endregion

        #region UIControllerBase Implementation

        protected override void InitializeComponents()
        {
            _sortByItemTypeController = GetComponentInChildren<UISortByItemTypeController>(true);
            _filterByItemTypeController = GetComponentInChildren<UIFilterByItemTypeController>(true);

            _btnSortByItemType = transform.FindDeepChild("Btn_SortByItemType")?.GetComponent<Button>();
            _btnFilterByItemType = transform.FindDeepChild("Btn_FilterByItemType")?.GetComponent<Button>();

            if (_sortByItemTypeController != null)
            {
                _sortByItemTypeController.OnSortByItemTypeConfirmed += SortByItemTypeConfirmed;
                _sortByItemTypeController.CloseControllerPanel();
            }
            if(_filterByItemTypeController != null)
            {
                _filterByItemTypeController.OnFilterByItemTypeConfirmed += FilterByItemTypeConfirmed;
                _filterByItemTypeController.CloseControllerPanel();
            }

            _btnSortByItemType?.onClick.AddListener(SortByItemTypeButtonClicked);
            _btnFilterByItemType?.onClick.AddListener(FilterByItemTypeButtonClicked);

            //Initially all values of ItemType enum are stored in _sortedItemTypes.
            _sortedItemTypes = Enum.GetValues(typeof(ItemType)).Cast<ItemType>().ToArray();
            _filteredItemTypes = Enum.GetValues(typeof(ItemType)).Cast<ItemType>().ToArray();
        }

        #endregion

        #region Callback Methods

        private void SortByItemTypeConfirmed(ItemType[] sortedItemTypes)
        {
            _sortedItemTypes = sortedItemTypes;
        }

        private void FilterByItemTypeConfirmed(ItemType[] filteredItemTypes)
        {
            _filteredItemTypes = filteredItemTypes;
        }

        private void SortByItemTypeButtonClicked()
        {
            if(_sortByItemTypeController == null)
            {
                Debug.LogError($"UICatalogController.SortByItemTypeButtonClicked Error: _sortByItemTypeController component is null!");
                return;
            }

            _sortByItemTypeController.EnablePanel(_sortedItemTypes);
        }

        private void FilterByItemTypeButtonClicked()
        {
            if (_filterByItemTypeController == null)
            {
                Debug.LogError($"UICatalogController.FilterByItemTypeButtonClicked Error: _filterByItemTypeController component is null!");
                return;
            }

            _filterByItemTypeController.EnablePanel(_filteredItemTypes);
        }

        #endregion
    }
}