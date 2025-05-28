using MAG.Unity.ProductCatalogAPI.Runtime.Base;
using MAG.Unity.ProductCatalogAPI.Runtime.Base.Enums;
using MAG.Unity.ProductCatalogAPI.Runtime.Base.Interface;
using PlasticPipe.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace MAG.Unity.ProductCatalogAPI.Runtime
{
    public class CatalogManager : MonoBehaviour
    {
        #region Components

        private JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.All,
        };

        #endregion

        #region Variables

        public static CatalogManager Instance { get; private set; }

        private Catalog _catalog;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            //Singleton Logic
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            LoadCatalog();

            //_catalog.MarketElements = SortMarketElements<float>(element => element.Price, false);
            //_catalog.MarketElements = SortMarketElements(ItemType.Ticket, ItemType.Gem);
            //_catalog.MarketElements = FilterMarketElements(ItemType.Ticket);
            //_catalog.MarketElements = FilterAndSortMarketElements<float>(new ItemType[] { ItemType.Ticket }, element => element.Price, false);
            //_catalog.MarketElements = FilterAndSortMarketElements(null, new ItemType[] { ItemType.Ticket, ItemType.Gem});
            //_catalog.MarketElements = GetAllMarketElements().OrderBy(a => a.Price).ToList().OrderByItemType(new ItemType[] { ItemType.Ticket, ItemType.Gem }).ToList().FilterMarketElements();

            for (int i = 0; i < _catalog.MarketElements.Count(); i++)
                Debug.Log($"Element {i}: {_catalog.MarketElements.ElementAt(i).Name} {_catalog.MarketElements.ElementAt(i).Description} {_catalog.MarketElements.ElementAt(i).Price}");

            //Test_InitializeCatalog();
        }

        #endregion

        #region Public API Methods

        /// <summary>
        /// Returns all market elements.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IMarketElement> GetAllMarketElements()
        {
            if (!IsCatalogLoaded())
                return Enumerable.Empty<IMarketElement>();

            return _catalog.MarketElements.ToList();
        }

        /// <summary>
        /// Returns a sorted IEnumerable of market elements based on a custom sorting key selector.
        /// </summary>
        /// <typeparam name="TKey">The type of the key to sort by (e.g., string for name, float for price).</typeparam>
        /// <param name="keySelector">A lambda expression that selects the property to sort by.</param>
        /// <param name="ascending">If true, sorts in ascending order; otherwise, descending.</param>
        /// <returns>A new IEnumerable of sorted <see cref="MarketElementBase"/> objects.</returns>
        public IEnumerable<IMarketElement> SortMarketElements<TKey>(Func<IMarketElement, TKey> keySelector, bool ascending = true)
        {
            if (!IsCatalogLoaded())
                return Enumerable.Empty<IMarketElement>();

            return ascending
                ? _catalog.MarketElements.OrderBy(keySelector).ToList()
                : _catalog.MarketElements.OrderByDescending(keySelector).ToList();
        }

        /// <summary>
        /// Returns a sorted IEnumerable of market elements based on a custom priority order of <see cref="ItemType"/>.
        /// </summary>
        /// <param name="itemTypeOrder">An array defining the preferred order of item types (e.g., Coins > Gems > Tickets).</param>
        /// <returns>A new IEnumerable of sorted <see cref="MarketElementBase"/> objects where elements are ordered by matching item types.</returns>
        public IEnumerable<IMarketElement> SortMarketElements(params ItemType[] itemTypeOrder)
        {
            if (!IsCatalogLoaded())
                return Enumerable.Empty<IMarketElement>();

            return _catalog.MarketElements.OrderByItemType(itemTypeOrder).ToList();
        }

        /// <summary>
        /// Returns only the market elements that contain at least one of the specified <see cref="ItemType"/> values.
        /// </summary>
        /// <param name="filteredItemTypes">The item types to include (e.g., Coins, Tickets). If empty or null, returns all market elements.</param>
        /// <returns>A filtered IEnumerable of <see cref="IMarketElement"/> objects.</returns>
        public IEnumerable<IMarketElement> FilterMarketElements(params ItemType[] filteredItemTypes)
        {
            if (!IsCatalogLoaded())
                return Enumerable.Empty<IMarketElement>();

            return _catalog.MarketElements.FilterMarketElements(filteredItemTypes).ToList();
        }

        /// <summary>
        /// Filters and sorts market elements based on the specified item types and a custom key selector.
        /// </summary>
        /// <typeparam name="TKey">The type of the key to sort by (e.g., string for name, float for price).</typeparam>
        /// <param name="filteredItemTypes">The item types to include (e.g., Coins, Tickets). If empty or null, returns all market elements.</param>
        /// <param name="keySelector">A lambda expression that selects the property to sort by.</param>
        /// <param name="ascending">If true, sorts in ascending order; otherwise, descending.</param>
        /// <returns></returns>
        public IEnumerable<IMarketElement> FilterAndSortMarketElements<TKey>(ItemType[] filteredItemTypes, Func<IMarketElement, TKey> keySelector, bool ascending = true)
        {
            IEnumerable<IMarketElement> filteredElements = FilterMarketElements(filteredItemTypes);
            if(filteredElements == null || !filteredElements.Any())
                return Enumerable.Empty<IMarketElement>();

            return ascending 
                ? filteredElements.OrderBy(keySelector)
                : filteredElements.OrderByDescending(keySelector);
        }

        /// <summary>
        /// Filters and sorts market elements based on the specified item types and a custom priority order of <see cref="ItemType"/>.
        /// </summary>
        /// <param name="filteredItemTypes">The item types to include (e.g., Coins, Tickets). If empty or null, returns all market elements.</param>
        /// <param name="itemTypeOrder">An array defining the preferred order of item types (e.g., Coins > Gems > Tickets).</param>
        /// <returns></returns>
        public IEnumerable<IMarketElement> FilterAndSortMarketElements(ItemType[] filteredItemTypes, ItemType[] itemTypeOrder)
        {
            IEnumerable<IMarketElement> filteredElements = FilterMarketElements(filteredItemTypes);
            if (filteredElements == null || !filteredElements.Any())
                return Enumerable.Empty<IMarketElement>();

            return filteredElements.OrderByItemType(itemTypeOrder);
        }

        #endregion

        #region Private Methods

        private bool IsCatalogLoaded()
        {
            bool result = _catalog != null && _catalog.MarketElements != null && _catalog.MarketElements.Any();
            if(!result)
                Debug.LogWarning($"CatalogManager.IsCatalogLoaded: Catalog or MarketElements is null or empty!");

            return result;
        }

        private void LoadCatalog()
        {
            TextAsset serializedCatalog = Resources.Load<TextAsset>("Catalog");
            if(serializedCatalog == null)
            {
                Debug.LogWarning("CatalogManager.LoadCatalog: Catalog resource not found! Please ensure the Catalog.json file is placed in the Resources folder.");
                return;
            }

            _catalog = JsonConvert.DeserializeObject<Catalog>(serializedCatalog.text, _serializerSettings);
        }

        #endregion

        #region Test Functions

        private void Test_InitializeCatalog()
        {
            _catalog = new Catalog();
            //_catalog.Products = new List<Product>()
            //{
            //    new Product()
            //    {
            //        ItemType = ItemType.Coin,
            //        Name = "Small Coin Pack",
            //        Description = "Small Coin Pack Description",
            //        Price = 9.99f,
            //        Amount = 100
            //    },
            //    new Product()
            //    {
            //        ItemType = ItemType.Coin,
            //        Name = "Large Coin Pack",
            //        Description = "Large Coin Pack Description",
            //        Price = 19.99f,
            //        Amount = 250
            //    },
            //    new Product()
            //    {
            //        ItemType = ItemType.Coin,
            //        Name = "Huge Coin Pack",
            //        Description = "Huge Coin Pack Description",
            //        Price = 29.99f,
            //        Amount = 600
            //    },
            //    new Product()
            //    {
            //        ItemType = ItemType.Gem,
            //        Name = "Small Gem Pack",
            //        Description = "Small Gem Pack Description",
            //        Price = 4.99f,
            //        Amount = 50
            //    },
            //    new Product()
            //    {
            //        ItemType = ItemType.Gem,
            //        Name = "Large Gem Pack",
            //        Description = "Large Gem Pack Description",
            //        Price = 14.99f,
            //        Amount = 120
            //    },
            //    new Product()
            //    {
            //        ItemType = ItemType.Gem,
            //        Name = "Huge Gem Pack",
            //        Description = "Huge Gem Pack Description",
            //        Price = 24.99f,
            //        Amount = 240
            //    },
            //    new Product()
            //    {
            //        ItemType = ItemType.Ticket,
            //        Name = "Single Ticket",
            //        Description = "Single Ticket Description",
            //        Price = 0.99f,
            //        Amount = 1
            //    },
            //    new Product()
            //    {
            //        ItemType = ItemType.Ticket,
            //        Name = "Double Ticket",
            //        Description = "Double Ticket Description",
            //        Price = 1.49f,
            //        Amount = 2
            //    },
            //    new Product()
            //    {
            //        ItemType = ItemType.Ticket,
            //        Name = "Multiple Ticket",
            //        Description = "Multiple Ticket Description",
            //        Price = 3.99f,
            //        Amount = 5
            //    },
            //};
            //_catalog.Bundles = new List<Bundle>()
            //{
            //    new Bundle()
            //    {
            //        Name = "Starter Bundle",
            //        Description = "Starter Bundle Description",
            //        Price = 9.99f,
            //        Items = new List<BundleItem>()
            //        {
            //            new BundleItem()
            //            {
            //                ItemType = ItemType.Coin,
            //                Amount = 100
            //            },
            //            new BundleItem()
            //            {
            //                ItemType = ItemType.Gem,
            //                Amount = 50
            //            },
            //            new BundleItem()
            //            {
            //                ItemType = ItemType.Ticket,
            //                Amount = 1
            //            },
            //        }
            //    },
            //    new Bundle()
            //    {
            //        Name = "Advanced Bundle",
            //        Description = "Advanced Bundle Description",
            //        Price = 19.99f,
            //        Items = new List<BundleItem>()
            //        {
            //            new BundleItem()
            //            {
            //                ItemType = ItemType.Coin,
            //                Amount = 250
            //            },
            //            new BundleItem()
            //            {
            //                ItemType = ItemType.Gem,
            //                Amount = 100
            //            },
            //            new BundleItem()
            //            {
            //                ItemType = ItemType.Ticket,
            //                Amount = 2
            //            },
            //        }
            //    }
            //};

            _catalog.MarketElements = new List<IMarketElement>()
            {
                new Product()
                {
                    ItemType = ItemType.Coin,
                    Name = "Small Coin Pack",
                    Description = "Small Coin Pack Description",
                    Price = 9.99f,
                    Amount = 100
                },
                new Product()
                {
                    ItemType = ItemType.Coin,
                    Name = "Large Coin Pack",
                    Description = "Large Coin Pack Description",
                    Price = 19.99f,
                    Amount = 250
                },
                new Product()
                {
                    ItemType = ItemType.Coin,
                    Name = "Huge Coin Pack",
                    Description = "Huge Coin Pack Description",
                    Price = 29.99f,
                    Amount = 600
                },
                new Product()
                {
                    ItemType = ItemType.Gem,
                    Name = "Small Gem Pack",
                    Description = "Small Gem Pack Description",
                    Price = 4.99f,
                    Amount = 50
                },
                new Product()
                {
                    ItemType = ItemType.Gem,
                    Name = "Large Gem Pack",
                    Description = "Large Gem Pack Description",
                    Price = 14.99f,
                    Amount = 120
                },
                new Product()
                {
                    ItemType = ItemType.Gem,
                    Name = "Huge Gem Pack",
                    Description = "Huge Gem Pack Description",
                    Price = 24.99f,
                    Amount = 240
                },
                new Product()
                {
                    ItemType = ItemType.Ticket,
                    Name = "Single Ticket",
                    Description = "Single Ticket Description",
                    Price = 0.99f,
                    Amount = 1
                },
                new Product()
                {
                    ItemType = ItemType.Ticket,
                    Name = "Double Ticket",
                    Description = "Double Ticket Description",
                    Price = 1.49f,
                    Amount = 2
                },
                new Product()
                {
                    ItemType = ItemType.Ticket,
                    Name = "Multiple Ticket",
                    Description = "Multiple Ticket Description",
                    Price = 3.99f,
                    Amount = 5
                },
                new Bundle()
                {
                    Name = "Starter Bundle",
                    Description = "Starter Bundle Description",
                    Price = 9.99f,
                    Items = new List<BundleItem>()
                    {
                        new BundleItem()
                        {
                            ItemType = ItemType.Coin,
                            Amount = 100
                        },
                        new BundleItem()
                        {
                            ItemType = ItemType.Gem,
                            Amount = 50
                        },
                        new BundleItem()
                        {
                            ItemType = ItemType.Ticket,
                            Amount = 1
                        },
                    }
                },
                new Bundle()
                {
                    Name = "Advanced Bundle",
                    Description = "Advanced Bundle Description",
                    Price = 19.99f,
                    Items = new List<BundleItem>()
                    {
                        new BundleItem()
                        {
                            ItemType = ItemType.Coin,
                            Amount = 250
                        },
                        new BundleItem()
                        {
                            ItemType = ItemType.Gem,
                            Amount = 100
                        },
                        new BundleItem()
                        {
                            ItemType = ItemType.Ticket,
                            Amount = 2
                        },
                    }
                }
            };

            string serializedCatalog = JsonConvert.SerializeObject(_catalog, _serializerSettings);
            File.WriteAllText(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "Catalog.json"), serializedCatalog);
        }

        #endregion
    }
}