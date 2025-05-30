using MAG.Unity.ProductCatalogAPI.Runtime.Base;
using MAG.Unity.ProductCatalogAPI.Runtime.Base.Enums;
using MAG.Unity.ProductCatalogAPI.Runtime.Base.Interface;
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
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        };

        #endregion

        #region Variables

        public static CatalogManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    GameObject catalogManagerObject = new GameObject("CatalogManager");
                    _instance = catalogManagerObject.AddComponent<CatalogManager>();

                    DontDestroyOnLoad(catalogManagerObject);
                }

                return _instance;
            }
        }
        private static CatalogManager _instance;

        private Catalog _catalog;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            //Singleton Logic
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
            else
                Destroy(gameObject);

            LoadCatalog();

            //_catalog.MarketElements = SortMarketElements<float>(element => element.Price);
            //_catalog.MarketElements = SortMarketElements(ItemType.Ticket, ItemType.Gem);
            //_catalog.MarketElements = FilterMarketElements(ItemType.Ticket);
            //_catalog.MarketElements = FilterAndSortMarketElements<float>(new ItemType[] { ItemType.Ticket }, element => element.Price, false);
            //_catalog.MarketElements = FilterAndSortMarketElements(null, new ItemType[] { ItemType.Ticket, ItemType.Gem});
            //_catalog.MarketElements = GetAllMarketElements().OrderBy(a => a.Price).ToList().OrderByItemType(new ItemType[] { ItemType.Ticket, ItemType.Gem }).ToList().FilterMarketElements(ItemType.Ticket, ItemType.Coin);

            //for (int i = 0; i < _catalog.MarketElements.Count(); i++)
            //    Debug.Log($"Element {i}: {_catalog.MarketElements.ElementAt(i).Name} {_catalog.MarketElements.ElementAt(i).Description} {_catalog.MarketElements.ElementAt(i).Price}");

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

            return _catalog.MarketElements;
        }

        /// <summary>
        /// Returns a sorted IEnumerable of market elements based on a custom sorting key selector.
        /// </summary>
        /// <typeparam name="TKey">The type of the key to sort by (e.g., string for name, float for price).</typeparam>
        /// <param name="keySelector">A lambda expression that selects the property to sort by.</param>
        /// <param name="ascending">If true, sorts in ascending order; otherwise, descending.</param>
        /// <returns>A new IEnumerable of sorted <see cref="IMarketElement"/> objects.</returns>
        public IEnumerable<IMarketElement> SortMarketElements<TKey>(Func<IMarketElement, TKey> keySelector, bool ascending = true)
        {
            if (keySelector == null)
            {
                Debug.LogError($"CatalogManager.SortMarketElements Error: keySelector cannot be null! Please provide a valid filter predicate.");
                return Enumerable.Empty<IMarketElement>();
            }

            if (!IsCatalogLoaded())
                return Enumerable.Empty<IMarketElement>();

            return ascending
                ? _catalog.MarketElements.OrderBy(keySelector)
                : _catalog.MarketElements.OrderByDescending(keySelector);
        }

        /// <summary>
        /// Returns a sorted IEnumerable of market elements based on a custom priority order of <see cref="ItemType"/>.
        /// </summary>
        /// <param name="itemTypeOrder">An array defining the preferred order of item types (e.g., Coins > Gems > Tickets).</param>
        /// <returns>A new IEnumerable of sorted <see cref="IMarketElement"/> objects where elements are ordered by matching item types.</returns>
        public IEnumerable<IMarketElement> SortMarketElements(params ItemType[] itemTypeOrder)
        {
            if (!IsCatalogLoaded())
                return Enumerable.Empty<IMarketElement>();

            return _catalog.MarketElements.OrderByItemType(itemTypeOrder);
        }

        /// <summary>
        /// Returns a filtered IEnumerable of market elements using a custom predicate.
        /// </summary>
        /// <param name="predicate">A lambda expression that defines the filter condition (e.g., m => m.Price >= 10 && m.Price <= 20).</param>
        /// <returns>A filtered IEnumerable of <see cref="IMarketElement"/> objects.</returns>
        public IEnumerable<IMarketElement> FilterMarketElements(Func<IMarketElement, bool> predicate)
        {
            if(predicate == null)
            {
                Debug.LogError($"CatalogManager.FilterMarketElements Error: predicate cannot be null! Please provide a valid filter predicate.");
                return Enumerable.Empty<IMarketElement>();
            }

            if (!IsCatalogLoaded())
                return Enumerable.Empty<IMarketElement>();

            return _catalog.MarketElements.Where(predicate);
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

            return _catalog.MarketElements.FilterByItemType(filteredItemTypes);
        }

        /// <summary>
        /// Filters and sorts market elements based on the specified item types and a custom key selector.
        /// </summary>
        /// <typeparam name="TKey">The type of the key to sort by (e.g., string for name, float for price).</typeparam>
        /// <param name="filteredItemTypes">The item types to include (e.g., Coins, Tickets). If empty or null, returns all market elements.</param>
        /// <param name="keySelector">A lambda expression that selects the property to sort by.</param>
        /// <param name="ascending">If true, sorts in ascending order; otherwise, descending.</param>
        /// <returns>Returns filtered and sorted IENumerable of IMarketElements.</returns>
        public IEnumerable<IMarketElement> FilterAndSortMarketElements<TKey>(ItemType[] filteredItemTypes, Func<IMarketElement, TKey> keySelector, bool ascending = true)
        {
            if (keySelector == null)
            {
                Debug.LogError($"CatalogManager.FilterAndSortMarketElements Error: keySelector cannot be null! Please provide a valid filter predicate.");
                return Enumerable.Empty<IMarketElement>();
            }

            IEnumerable<IMarketElement> filteredElements = FilterMarketElements(filteredItemTypes);
            if(filteredElements == null || !filteredElements.Any())
                return Enumerable.Empty<IMarketElement>();

            return ascending 
                ? filteredElements.OrderBy(keySelector)
                : filteredElements.OrderByDescending(keySelector);
        }

        /// <summary>
        /// Filters and sorts market elements based on a custom predicate and a key selector.
        /// </summary>
        /// <typeparam name="TKey">The type of the key to sort by (e.g., string for name, float for price).</typeparam>
        /// <param name="filterPredicate">A lambda expression for applying custom filter.</param>
        /// <param name="keySelector">A lambda expression that selects the property to sort by.</param>
        /// <param name="ascending">If true, sorts in ascending order; otherwise, descending.</param>
        /// <returns>Returns filtered and sorted IENumerable of IMarketElements.</returns>
        public IEnumerable<IMarketElement> FilterAndSortMarketElements<TKey>(Func<IMarketElement, bool> filterPredicate, Func<IMarketElement, TKey> keySelector, bool ascending = true)
        {
            if (keySelector == null)
            {
                Debug.LogError($"CatalogManager.FilterAndSortMarketElements Error: keySelector cannot be null! Please provide a valid filter predicate.");
                return Enumerable.Empty<IMarketElement>();
            }

            IEnumerable<IMarketElement> filteredElements = FilterMarketElements(filterPredicate);
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
        /// <returns>Returns filtered and sorted IENumerable of IMarketElements.</returns>
        public IEnumerable<IMarketElement> FilterAndSortMarketElements(ItemType[] filteredItemTypes, ItemType[] itemTypeOrder)
        {
            IEnumerable<IMarketElement> filteredElements = FilterMarketElements(filteredItemTypes);
            if (filteredElements == null || !filteredElements.Any())
                return Enumerable.Empty<IMarketElement>();

            return filteredElements.OrderByItemType(itemTypeOrder);
        }

        /// <summary>
        /// Filters and sorts market elements based on the custom filter predicate and a custom priority order of <see cref="ItemType"/>.
        /// </summary>
        /// <param name="filterPredicate">A lambda expression for applying custom filter.</param>
        /// <param name="itemTypeOrder">An array defining the preferred order of item types (e.g., Coins > Gems > Tickets).</param>
        /// <returns>Returns filtered and sorted IENumerable of IMarketElements.</returns>
        public IEnumerable<IMarketElement> FilterAndSortMarketElements(Func<IMarketElement, bool> filterPredicate, ItemType[] itemTypeOrder)
        {
            IEnumerable<IMarketElement> filteredElements = FilterMarketElements(filterPredicate);
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
            _catalog.MarketElements = new List<IMarketElement>()
            {
                new Product("Small Coin Pack", "Small Coin Pack Description", 9.99f, ItemType.Coin, 100),
                new Product("Large Coin Pack", "Large Coin Pack Description", 19.99f, ItemType.Coin, 250),
                new Product("Huge Coin Pack", "Huge Coin Pack Description", 29.99f, ItemType.Coin, 600),
                new Product("Small Gem Pack", "Small Gem Pack Description", 4.99f, ItemType.Gem, 50),
                new Product("Large Gem Pack", "Large Gem Pack Description", 14.99f, ItemType.Gem, 120),
                new Product("Huge Gem Pack", "Huge Gem Pack Description", 24.99f, ItemType.Gem, 240),
                new Product("Single Ticket", "Single Ticket Description", 0.99f, ItemType.Ticket, 1),
                new Product("Double Ticket", "Double Ticket Description", 1.49f, ItemType.Ticket, 2),
                new Product("Multiple Ticket", "Multiple Ticket Description", 3.99f, ItemType.Ticket, 5),
                new Bundle("Starter Bundle", "Starter Bundle Description", 9.99f, new List<Item>() { new Item (ItemType.Coin, 100), new Item (ItemType.Gem, 50), new Item (ItemType.Ticket, 1)}),
                new Bundle("Advanced Bundle", "Advanced Bundle Description", 29.99f, new List<Item>() { new Item (ItemType.Coin, 250), new Item (ItemType.Gem, 100), new Item (ItemType.Ticket, 2)}),
                new Bundle("No Way Bundle", "No Way Bundle Description", 39.99f, new List<Item>() { new Item (ItemType.Coin, 500), new Item (ItemType.Gem, 200)})
            };

            string serializedCatalog = JsonConvert.SerializeObject(_catalog, _serializerSettings);
            File.WriteAllText(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "Catalog.json"), serializedCatalog);
        }

        #endregion
    }
}