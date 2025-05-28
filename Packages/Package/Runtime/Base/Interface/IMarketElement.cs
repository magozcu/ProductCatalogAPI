namespace MAG.Unity.ProductCatalogAPI.Runtime.Base.Interface
{
    public interface IMarketElement : IItemTypeHolder
    {
        public string Name { get; }
        public string Description { get; }
        public float Price { get; }
    }
}