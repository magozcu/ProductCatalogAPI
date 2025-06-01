using MAG.Unity.ProductCatalogAPI.Runtime.Base.Enums;
using MAG.Unity.ProductCatalogAPI.Runtime.Base.Interface;
using MAG.Unity.ProductCatalogAPI.Runtime.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MAG.Unity.ProductCatalogAPI.Runtime.Demo
{
    public class ProductCatalogAPIDemoSceneManager : MonoBehaviour
    {
        #region Components

        [SerializeField]
        private UICatalogController _catalogController;

        [SerializeField]
        private Button _btnSortBasedOnNameLengthDescending;
        [SerializeField]
        private Button _btnFilterBasedOnNameContainsGem;
        [SerializeField]
        private Button _btnSortNamesDescendingAndFilterPriceUnder20;

        #endregion

        #region Variables

        private ItemType[] _allItemTypes;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _allItemTypes = Enum.GetValues(typeof(ItemType)).Cast<ItemType>().ToArray();

            _btnSortBasedOnNameLengthDescending?.onClick.AddListener(SortBasedOnNameLengthDescendingClicked);
            _btnFilterBasedOnNameContainsGem?.onClick.AddListener(FilterBasedOnNameContainsGem);
            _btnSortNamesDescendingAndFilterPriceUnder20?.onClick.AddListener(SortNamesDescendingAndFilterPriceUnder20Clicked);
        }

        #endregion

        #region Callback Methods

        private void SortBasedOnNameLengthDescendingClicked()
        {
            IEnumerable<IMarketElement> sortedMarketElements = CatalogManager.Instance.SortMarketElements((element => element.Name.Length), false);
            _catalogController.DisplayCatalog(sortedMarketElements);
        }

        private void FilterBasedOnNameContainsGem()
        {
            IEnumerable<IMarketElement> filteredMarketElements = CatalogManager.Instance.FilterMarketElements(element => element.Name.Contains("Gem"));
            _catalogController.DisplayCatalog(filteredMarketElements);
        }

        private void SortNamesDescendingAndFilterPriceUnder20Clicked()
        {
            IEnumerable<IMarketElement> filteredAndSortedMarketElements = CatalogManager.Instance
                .FilterMarketElements(m => m.Price < 20)
                .OrderByDescending(m => m.Name);

            _catalogController.DisplayCatalog(filteredAndSortedMarketElements);
        }

        #endregion
    }
}