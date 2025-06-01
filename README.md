# MAG.Unity.ProductCatalogAPI

MAG.Unity.ProductCatalogAPI is a flexible and extensible Unity package for loading, displaying, and interacting with a product catalog. It provides both runtime APIs and a ready-to-use UI that allows developers to sort and filter market elements such as Products and Bundles.

## Dependencies

This package depends on the following Unity packages:

- `com.unity.textmeshpro` – Required for UI text rendering
- `Newtonsoft.Json` – Used for JSON serialization/deserialization

> ℹ️ Make sure both packages are included in your project.

Also, for **TextMeshPro to work properly**, ensure you import the TMP Essentials:
- Go to `Window > TextMeshPro > Import TMP Essential Resources`

## Features
- JSON-based catalog loading via `Resources`
- Custom filtering and sorting support through `CatalogManager`
- Runtime-ready UI prefab for quick integration
- LINQ-style extension methods for advanced querying
- Demo scene demonstrating all use cases

## Installation
1. Add the package to your Unity project.
2. Place the `ProductCatalog` prefab located in `Runtime/Resources/Prefabs/UI` into your scene.
3. Ensure your `Catalog.json` file is in the `Resources` folder and follows the correct structure.

## Demo Scene
A fully functional demo scene is available under `Samples/Scenes/DemoScene_ProductCatalogAPI.unity`.
It demonstrates sorting, filtering, UI integration, and custom behavior. It is strongly recommended to review this scene for a complete understanding of the system.

## Usage Examples

### a. Get all market elements
```csharp
var allElements = CatalogManager.Instance.GetAllMarketElements().ToList();
```

### b. Only sort (e.g., by Price ascending)
```csharp
var sortedByPrice = CatalogManager.Instance
    .SortMarketElements(m => m.Price, ascending: true)
    .ToList();
```

### c. Only filter (e.g., names that contain "Gem")
```csharp
var filteredByName = CatalogManager.Instance
    .FilterMarketElements(m => m.Name.Contains("Gem"))
    .ToList();
```

### d. Filter and sort (e.g., only Coin item types, sorted by Price)
```csharp
var filteredSorted = CatalogManager.Instance
    .FilterMarketElements(ItemType.Coin)
    .OrderBy(m => m.Price)
    .ToList();
```

### e. Filter and sort (e.g., Price between 5 and 20, sorted by ItemType order: Ticket > Gem > Coin)
```csharp
var filteredSorted = CatalogManager.Instance
    .FilterMarketElements(m => m.Price >= 5 && m.Price <= 20)
    .OrderByItemType(new ItemType[] { ItemType.Ticket, ItemType.Gem, ItemType.Coin })
    .ToList();
```

### g. Filter and sort (e.g., Show only Gem and Ticket items, sorted by Price ascending)
```csharp
var filteredSorted = CatalogManager.Instance.FilterAndSortMarketElements(
    new ItemType[] { ItemType.Gem, ItemType.Ticket },
    keySelector: m => m.Price,
    ascending: true);
```

### h. Filter and sort (e.g., Show only Gem and Coin items, sorted by custom order: Coin > Gem)
```csharp
var filteredSorted = CatalogManager.Instance.FilterAndSortMarketElements(
    new ItemType[] { ItemType.Gem, ItemType.Coin },
    new ItemType[] { ItemType.Coin, ItemType.Gem });
```

## License
See [LICENSE.md](./LICENSE.md)
