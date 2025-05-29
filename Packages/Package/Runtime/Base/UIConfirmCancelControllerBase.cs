using UnityEngine;
using UnityEngine.UI;

namespace MAG.Unity.ProductCatalogAPI.Runtime.Base
{
    public abstract class UIConfirmCancelControllerBase : UIControllerBase
    {
        #region Components

        protected Transform _transformContent;
        protected Button _btnConfirm;
        protected Button _btnCancel;

        #endregion

        #region UIControllerBase Implementation

        protected override void InitializeComponents()
        {
            _transformContent = transform.FindDeepChild("Content");
            _btnConfirm = transform.FindDeepChild("Btn_Confirm")?.GetComponent<Button>();
            _btnCancel = transform.FindDeepChild("Btn_Cancel")?.GetComponent<Button>();

            _btnConfirm.onClick.AddListener(ConfirmClicked);
            _btnCancel?.onClick.AddListener(CloseControllerPanel);
        }

        #endregion

        #region Callback Methods

        protected abstract void ConfirmClicked();

        #endregion
    }
}