# Changelog

All notable changes to this project will be documented in this file.

## [1.0.0] - 2025-05-31

### Added
- Initial release of `MAG.Unity.ProductCatalogAPI` package
- Core API (`CatalogManager`) for loading, filtering, and sorting catalog data
- Support for:
  - Property-based sorting (e.g., Name, Description, Price)
  - ItemType-based sorting using custom priority arrays
  - Predicate-based filtering using LINQ expressions
  - Combined filtering and sorting with overloads
- JSON-based catalog loading using `Resources/Catalog.json`
- Runtime-prefab UI for displaying product catalog
- Scrollable, responsive product display with dynamic height support
- Sample item types (`Coin`, `Gem`, `Ticket`), products, and bundles
- Custom `JsonConverter` for polymorphic deserialization via discriminator
- Fully interactive demo scene (`DemoScene_ProductCatalogAPI`)
- Sample UI logic for custom sort/filter input
- LINQ-style extension methods (`OrderByItemType`, `FilterMarketElements`, etc.)

### Changed
- N/A

### Fixed
- N/A