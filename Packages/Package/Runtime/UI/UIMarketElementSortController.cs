using MAG.Unity.ProductCatalogAPI.Runtime.Base;
using MAG.Unity.ProductCatalogAPI.Runtime.Base.Interface;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MAG.Unity.ProductCatalogAPI.Runtime.UI
{
    #region Delegates

    public delegate void SortSelectedEventHandler(Func<IMarketElement, object> keySelector, bool ascending);

    #endregion

    public class UIMarketElementSortController : UIControllerBase
    {
        #region Events

        public event SortSelectedEventHandler OnSortSelected;

        #endregion

        #region Components

        private Button _btnName;
        private Button _btnDescription;
        private Button _btnPrice;

        #endregion

        #region Variables

        private Dictionary<string, Func<IMarketElement, object>> _sortPropertyDict = new Dictionary<string, Func<IMarketElement, object>>()
        {
            { "Name", element => element.Name },
            { "Description", element => element.Description },
            { "Price", element => element.Price },
        };

        private string _previousSelectedProperty = string.Empty;
        private bool _isAscending = true;

        #endregion

        #region UIControllerBase Implementation

        protected override void InitializeComponents()
        {
            _btnName = transform.FindDeepChild("Btn_Name")?.GetComponent<Button>();
            _btnDescription = transform.FindDeepChild("Btn_Description")?.GetComponent<Button>();
            _btnPrice = transform.FindDeepChild("Btn_Price")?.GetComponent<Button>();

            _btnName?.onClick.AddListener(() => SortPropertySelected("Name"));
            _btnDescription?.onClick.AddListener(() => SortPropertySelected("Description"));
            _btnPrice?.onClick.AddListener(() => SortPropertySelected("Price"));
        }

        #endregion

        #region Callback Methods

        private void SortPropertySelected(string propertyName)
        {
            try
            {
                if (_previousSelectedProperty.Equals(propertyName))
                    _isAscending = !_isAscending;
                else
                    _isAscending = true;

                _previousSelectedProperty = propertyName;
                OnSortSelected?.Invoke(_sortPropertyDict[propertyName], _isAscending);
            }
            catch(Exception ex)
            {
                Debug.LogError($"UIMarketElementSortController.SortPropertySelected Error: {ex}");
            }
        }

        #endregion
    }
}