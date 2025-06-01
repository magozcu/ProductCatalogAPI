using MAG.Unity.ProductCatalogAPI.Runtime.Base;
using MAG.Unity.ProductCatalogAPI.Runtime.Base.Enums;
using MAG.Unity.ProductCatalogAPI.Runtime.Base.Interface;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// UI Controller designed for displaying market elements in the catalog.
        /// </summary>
        private UIMarketElementsController _marketElementsController;
        /// <summary>
        /// UI Controller designed for selecting the property to sort the market elements.
        /// </summary>
        private UIMarketElementSortController _marketElementSortController;
        /// <summary>
        /// UI Controller designed for handling sorting the market elements based on their item type.
        /// </summary>
        private UISortByItemTypeController _sortByItemTypeController;
        /// <summary>
        /// UI Controller designed for handling filtering the market elements based on their item type.
        /// </summary>
        private UIFilterByItemTypeController _filterByItemTypeController;

        /// <summary>
        /// Button to open the sort by item type panel for the user to select their preferred sorting order.
        /// </summary>
        private Button _btnSortByItemType;
        /// <summary>
        /// Button to open the filter by item type panel for the user to select their preferred item types to filter the market elements.
        /// </summary>
        private Button _btnFilterByItemType;

        #endregion

        #region Variables

        /// <summary>
        /// Holds the sorting method selected by the user.
        /// </summary>
        private SortingType _sortingType = SortingType.ItemType;
        /// <summary>
        /// Holds the key selector function used for sorting market elements based on a specific property.
        /// </summary>
        private Func<IMarketElement, object> _sortKeySelector = null;
        /// <summary>
        /// Holds the ascending or descending order for sorting market elements.
        /// </summary>
        private bool _isSortAscending = true;
        /// <summary>
        /// Holds the sorted item types based on the user's selection.
        /// </summary>
        private ItemType[] _sortedItemTypes;
        /// <summary>
        /// Holds the filtered item types based on the user's selection.
        /// </summary>
        private ItemType[] _filteredItemTypes;

        #endregion

        #region Unity Methods

        private void Start()
        {
            RefreshCatalog();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Displays the given sorted and filtered market elements in the catalog.
        /// </summary>
        /// <param name="filteredAndSortedMarketElements"></param>
        public void DisplayCatalog(IEnumerable<IMarketElement> filteredAndSortedMarketElements)
        {
            try
            {
                _marketElementsController.EnablePanel(filteredAndSortedMarketElements);
            }
            catch (Exception ex)
            {
                Debug.LogError($"UICatalogController.DisplayCatalog Error: {ex.Message}");
            }
        }

        #endregion

        #region Private Methods

        private void RefreshCatalog()
        {
            try
            {
                if (_marketElementsController == null)
                {
                    Debug.LogError($"UICatalogController.RefreshCatalog Error: _marketElementsController component is null!");
                    return;
                }

                IEnumerable<IMarketElement> sortedAndFilteredMarketElements = null;
                switch (_sortingType)
                {
                    case SortingType.Property:
                        sortedAndFilteredMarketElements = CatalogManager.Instance.FilterAndSortMarketElements(_filteredItemTypes, _sortKeySelector, _isSortAscending);
                        break;
                    case SortingType.ItemType:
                        sortedAndFilteredMarketElements = CatalogManager.Instance.FilterAndSortMarketElements(_filteredItemTypes, _sortedItemTypes);
                        break;
                }

                _marketElementsController.EnablePanel(sortedAndFilteredMarketElements);
            }
            catch (Exception ex)
            {
                Debug.LogError($"UICatalogController.RefreshCatalog Error: {ex.Message}");
            }
        }

        #endregion

        #region UIControllerBase Implementation

        protected override void InitializeComponents()
        {
            //Finding the script components in the children of this GameObject.
            _marketElementsController = GetComponentInChildren<UIMarketElementsController>(true);
            _marketElementSortController = GetComponentInChildren<UIMarketElementSortController>(true);
            _sortByItemTypeController = GetComponentInChildren<UISortByItemTypeController>(true);
            _filterByItemTypeController = GetComponentInChildren<UIFilterByItemTypeController>(true);

            //Finding the UI elements in the children of this GameObject.
            _btnSortByItemType = transform.FindDeepChild("Btn_SortByItemType")?.GetComponent<Button>();
            _btnFilterByItemType = transform.FindDeepChild("Btn_FilterByItemType")?.GetComponent<Button>();

            // Subscribing to the events of the sort and filter controllers to handle the confirmed item types.
            if(_marketElementSortController != null)
            {
                _marketElementSortController.OnSortSelected += SortByPropertySelected;
            }
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

            // Adding listeners to the buttons to handle click events.
            _btnSortByItemType?.onClick.AddListener(SortByItemTypeButtonClicked);
            _btnFilterByItemType?.onClick.AddListener(FilterByItemTypeButtonClicked);

            //Initially all values of ItemType enum are stored in _sortedItemTypes.
            _sortedItemTypes = Enum.GetValues(typeof(ItemType)).Cast<ItemType>().ToArray();
            //Initially all values of ItemType enum are stored in _filteredItemTypes to see all item types.
            _filteredItemTypes = Enum.GetValues(typeof(ItemType)).Cast<ItemType>().ToArray();
        }

        #endregion

        #region Callback Methods

        private void SortByPropertySelected(Func<IMarketElement, object> keySelector, bool ascending)
        {
            _sortKeySelector = keySelector;
            _isSortAscending = ascending;
            _sortingType = SortingType.Property;

            RefreshCatalog();
        }

        private void SortByItemTypeConfirmed(ItemType[] sortedItemTypes)
        {
            _sortedItemTypes = sortedItemTypes;
            _sortingType = SortingType.ItemType;

            RefreshCatalog();
        }

        private void FilterByItemTypeConfirmed(ItemType[] filteredItemTypes)
        {
            _filteredItemTypes = filteredItemTypes;

            //There should be at least one Item Type in the array. If not, then all of them will be enabled.
            if(_filteredItemTypes == null || !_filteredItemTypes.Any())
                _filteredItemTypes = Enum.GetValues(typeof(ItemType)).Cast<ItemType>().ToArray();

            RefreshCatalog();
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