namespace MAG.Unity.ProductCatalogAPI.Runtime.Enums
{
    /// <summary>
    /// Represents the sorting type for market elements.
    /// </summary>
    public enum SortingType
    {
        /// <summary>
        /// Sorting using a property of the class (e.g. name through LINQ)
        /// </summary>
        Property,

        /// <summary>
        /// Using the custom sorting method based on ItemType.
        /// </summary>
        ItemType,
    }
}