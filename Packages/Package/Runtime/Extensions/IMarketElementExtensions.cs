using MAG.Unity.ProductCatalogAPI.Runtime.Base.Enums;
using MAG.Unity.ProductCatalogAPI.Runtime.Base.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

public static class IMarketElementExtensions
{
    /// <summary>
    /// Returns a sorted list of market elements based on a custom priority order of <see cref="ItemType"/>.
    /// </summary>
    /// <param name="elements">IEnumerable of market elements.</param>
    /// <param name="itemTypeOrder">An array defining the preferred order of item types (e.g., Coins > Gems > Tickets).</param>
    /// <returns></returns>
    public static IEnumerable<IMarketElement> OrderByItemType(this IEnumerable<IMarketElement> elements, params ItemType[] itemTypeOrder)
    {
        return elements.OrderBy(element =>
        {
            if (element == null || element.Items == null || !element.Items.Any())
                return int.MaxValue;

            // Iterated through the customItemOrder rather than IItemTypeHolder to find the ItemType with smallest index in the current Market Element.
            // This way if the ItemType with the smallest index exists in the Market Element's ItemTypes, the Market Element will be on top.
            for (int i = 0; i < itemTypeOrder.Length; i++)
            {
                ItemType currentItemType = itemTypeOrder[i];

                if (element.Items.FirstOrDefault(item => item.ItemType == currentItemType) != null)
                    return Array.IndexOf(itemTypeOrder, currentItemType);
            }

            return int.MaxValue;
        }).ToList();
    }

    /// <summary>
    /// Returns only the market elements that contain at least one of the specified <see cref="ItemType"/> values.
    /// </summary>
    /// <param name="elements">IEnumerable of market elements.</param>
    /// <param name="filteredItemTypes">The item types to include (e.g., Coins, Tickets). If empty or null, returns all market elements.</param>
    /// <returns></returns>
    public static IEnumerable<IMarketElement> FilterMarketElements(this IEnumerable<IMarketElement> elements, params ItemType[] filteredItemTypes)
    {
        if (filteredItemTypes == null || !filteredItemTypes.Any())
            return elements;

        return elements.Where(element => element.Items != null && element.Items.Any(itemType => filteredItemTypes.Contains(itemType.ItemType))).ToList();
    }
}