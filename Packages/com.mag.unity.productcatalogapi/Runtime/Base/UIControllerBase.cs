using UnityEngine;

namespace MAG.Unity.ProductCatalogAPI.Runtime.Base
{
    #region Delegates

    public delegate void ControllerPanelClosedEventHandler();

    #endregion

    public abstract class UIControllerBase : MonoBehaviour
    {
        #region Events

        public event ControllerPanelClosedEventHandler OnControllerPanelClosed;

        #endregion

        #region Variables

        protected bool _isInitialized = false;

        #endregion

        #region Unity Methods

        protected virtual void Awake()
        {
            Initialize();
        }

        #endregion

        #region Public Methods

        public void Initialize()
        {
            if(!_isInitialized)
            {
                InitializeComponents();

                _isInitialized = true;
            }
        }

        public void CloseControllerPanel()
        {
            OnControllerPanelClosed?.Invoke();
            gameObject.SetActive(false);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// For initializing the components of the controller. Is called in Initialize function. Should not be called more than once.
        /// </summary>
        protected abstract void InitializeComponents();

        #endregion
    }
}