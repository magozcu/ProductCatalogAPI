using MAG.Unity.ProductCatalogAPI.Runtime.Base;
using MAG.Unity.ProductCatalogAPI.Runtime.Base.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MAG.Unity.ProductCatalogAPI.Runtime.UI
{
    public class UIMarketElementsController : UIControllerBase
    {
        #region Prefabs

        private readonly string _prefabPathElementMarketElement = "Prefabs/UI/Element_MarketElement";

        #endregion

        #region Components

        private Transform _transformContent;

        #endregion

        #region Public Methods

        public void EnablePanel(IEnumerable<IMarketElement> marketElements)
        {
            Initialize();

            CreateMarketElements(marketElements);

            gameObject.SetActive(true);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates or updates the item type order elements count in the content transform based on the provided item types.
        /// </summary>
        /// <param name="marketElements"></param>
        private void CreateMarketElements(IEnumerable<IMarketElement> marketElements)
        {
            try
            {
                int createdMarketElementCount = _transformContent.childCount;

                // Too many elements exist. Remove the excess.
                if (createdMarketElementCount > marketElements.Count())
                {
                    for (int i = 0; i < createdMarketElementCount - marketElements.Count(); i++)
                    {
                        Destroy(_transformContent.GetChild(0).gameObject);
                    }
                }
                // Not enough elements exist. Create the missing ones.
                else if (createdMarketElementCount < marketElements.Count())
                {
                    for (int i = 0; i < marketElements.Count() - createdMarketElementCount; i++)
                    {
                        GameObject goNewElement = Instantiate(Resources.Load<GameObject>(_prefabPathElementMarketElement), _transformContent);
                    }
                }

                // Set the MarketElement for each element. Length of marketElements should be equal to the child count of _transformContent.
                for (int i = 0; i < _transformContent.childCount; i++)
                {
                    UIElementMarketElementController currentElement = _transformContent.GetChild(i).GetComponent<UIElementMarketElementController>();
                    currentElement.Initialize(marketElements.ElementAt(i));
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"UIMarketElementsController.CreateMarketElements Error: {ex.Message}");
            }
        }

        #endregion

        #region UIControllerBase Implementation

        protected override void InitializeComponents()
        {
            _transformContent = transform.FindDeepChild("Content");
        }

        #endregion
    }
}