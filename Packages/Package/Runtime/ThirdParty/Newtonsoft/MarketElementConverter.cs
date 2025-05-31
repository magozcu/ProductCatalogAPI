using MAG.Unity.ProductCatalogAPI.Runtime.Base.Interface;
using MAG.Unity.ProductCatalogAPI.Runtime.Base;
using System;
using Unity.Plastic.Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Linq;
using System.Linq;

namespace MAG.Unity.ProductCatalogAPI.Runtime.ThirdParty.Newtonsoft
{
    public class MarketElementConverter : JsonConverter
    {
        #region JsonConverter Implementations

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IMarketElement) || objectType == typeof(MarketElementBase);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);
            var typeName = jo["_discriminator"]?.ToString();

            if (string.IsNullOrWhiteSpace(typeName))
                throw new JsonSerializationException("Missing Discriminator.");

            var resolvedType = Type.GetType(typeName);
            if (resolvedType == null)
            {
                // Optional fallback using assembly search
                resolvedType = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(t => t.FullName == typeName);
            }

            if (resolvedType == null)
                throw new JsonSerializationException($"Unable to resolve type '{typeName}'");

            var target = Activator.CreateInstance(resolvedType);
            serializer.Populate(jo.CreateReader(), target);
            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JObject jo = JObject.FromObject(value, serializer);
            jo.AddFirst(new JProperty("_discriminator", value.GetType().FullName));
            jo.WriteTo(writer);
        }

        #endregion
    }
}