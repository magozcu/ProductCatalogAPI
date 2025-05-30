using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MAG.Unity.ProductCatalogAPI.Runtime.Base
{
    /// <summary>
    /// Designed for UI controllers that manage a list of elements, such as item types or market elements.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public abstract class UIListControllerBase<TData> : UIControllerBase
    {
        #region Prefabs

        /// <summary>
        /// Resources path required for creating element game objects
        /// </summary>
        public abstract string PrefabPathElement { get; }

        #endregion

        #region Components

        protected Transform _transformContent;

        #endregion

        #region Public Methods

        public virtual void EnablePanel(IEnumerable<TData> data)
        {
            Initialize();

            SynchronizeElementCount(data);
            InitializeElements(data);

            gameObject.SetActive(true);
        }

        #endregion

        #region Protected Methods

        protected virtual void SynchronizeElementCount(IEnumerable<TData> data)
        {
            try
            {
                int createdElementCount = _transformContent.childCount;

                // Not enough elements exist. Create the missing ones.
                if (createdElementCount < data.Count())
                {
                    for (int i = 0; i < data.Count() - createdElementCount; i++)
                    {
                        GameObject goNewElement = Instantiate(Resources.Load<GameObject>(PrefabPathElement), _transformContent);
                    }
                }

                // Disable all existing elements. Required ones will be enabled on initialize.
                for(int i = 0; i < _transformContent.childCount; i++)
                {
                    _transformContent.GetChild(i).gameObject.SetActive(false);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"UIListControllerBase.SynchronizeElementCount Error: {ex.Message}");
            }
        }

        protected abstract void InitializeElements(IEnumerable<TData> data);

        #endregion

        #region UIControllerBase Implementations

        protected override void InitializeComponents()
        {
            _transformContent = transform.FindDeepChild("Content");
        }

        #endregion
    }
}