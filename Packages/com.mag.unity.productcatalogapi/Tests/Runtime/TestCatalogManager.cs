using MAG.Unity.ProductCatalogAPI.Runtime.Base;
using MAG.Unity.ProductCatalogAPI.Runtime.Base.Enums;
using MAG.Unity.ProductCatalogAPI.Runtime.Base.Interface;
using MAG.Unity.ProductCatalogAPI.Runtime.ThirdParty.Newtonsoft;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace MAG.Unity.ProductCatalogAPI.Tests.Runtime
{
    public class TestCatalogManager : MonoBehaviour
    {
        #region Variables

        private Catalog _catalog;

        private JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Converters = new List<JsonConverter>()
            {
                new MarketElementConverter()
            }
        };

        #endregion

        #region Unity Methods

        private void Awake()
        {
            InitializeCatalog();
        }

        private void Start()
        {
            LoadCatalog();
        }

        #endregion

        #region Private Methods

        private void InitializeCatalog()
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

        private void LoadCatalog()
        {
            TextAsset serializedCatalog = Resources.Load<TextAsset>("Catalog");
            if (serializedCatalog == null)
            {
                Debug.LogWarning("TestCatalogManager.LoadCatalog: Catalog resource not found! Please ensure the Catalog.json file is placed in the Resources folder.");
                return;
            }

            _catalog = JsonConvert.DeserializeObject<Catalog>(serializedCatalog.text, _serializerSettings);
            PrintMarketElements(_catalog.MarketElements);
        }

        private void PrintMarketElements(IEnumerable<IMarketElement> marketElements)
        {
            if(marketElements == null || !marketElements.Any())
            {
                Debug.LogWarning($"TestCatalogManager.PrintMarketElements(): marketElements is null or empty.");
                return;
            }

            for (int i = 0; i < marketElements.Count(); i++)
                Debug.Log($"Element {i}: {marketElements.ElementAt(i).Name} {marketElements.ElementAt(i).Description} {marketElements.ElementAt(i).Price}");
        }

        #endregion
    }
}