using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.Extensions
{
    public static class JsonSerializerExtension
    {
        public static string SerializeToJson<T>(this T obj)
        {
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };

            var result = JsonConvert.SerializeObject(obj, jsonSerializerSettings);

            return result;
        }

        public static T DeserializeFromJson<T>(this string jsonText)
        {
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
            };

            T result = JsonConvert.DeserializeObject<T>(jsonText, jsonSerializerSettings);

            return result;
        }
    }
}
