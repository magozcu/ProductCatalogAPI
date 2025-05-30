using MAG.Unity.ProductCatalogAPI.Runtime.Base;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace MAG.Unity.ProductCatalogAPI.Runtime.UI
{
    public class UIConfirmCancelController : UIControllerBase
    {
        #region Events

        public event Action OnConfirmClicked;
        public event Action OnCancelClicked;

        #endregion

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

            _btnConfirm?.onClick.AddListener(ConfirmClicked);
            _btnCancel?.onClick.AddListener(CancelClicked);
        }

        #endregion

        #region Callback Methods

        private void ConfirmClicked()
        {
            OnConfirmClicked?.Invoke();
            CloseControllerPanel();
        }

        private void CancelClicked()
        {
            OnCancelClicked?.Invoke();
            CloseControllerPanel();
        }

        #endregion
    }
}