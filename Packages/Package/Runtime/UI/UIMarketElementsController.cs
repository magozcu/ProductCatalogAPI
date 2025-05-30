using MAG.Unity.ProductCatalogAPI.Runtime.Base;
using MAG.Unity.ProductCatalogAPI.Runtime.Base.Interface;
using System.Collections.Generic;
using System.Linq;

namespace MAG.Unity.ProductCatalogAPI.Runtime.UI
{
    public class UIMarketElementsController : UIListControllerBase<IMarketElement>
    {
        #region UIListControllerBase Implementations

        public override string PrefabPathElement => "Prefabs/UI/Element_MarketElement";

        protected override void InitializeElements(IEnumerable<IMarketElement> data)
        {
            // Set the MarketElement for each element. Length of marketElements should be equal to the child count of _transformContent.
            //for (int i = 0; i < _transformContent.childCount; i++)
            for (int i = 0; i < data.Count(); i++)
            {
                UIElementMarketElementController currentElement = _transformContent.GetChild(i).GetComponent<UIElementMarketElementController>();
                currentElement.Initialize(data.ElementAt(i));
            }
        }

        #endregion
    }
}