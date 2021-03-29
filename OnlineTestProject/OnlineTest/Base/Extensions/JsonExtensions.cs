using Newtonsoft.Json;

namespace OnlineTest.Base.Extensions
{
    public static class JsonExtensions
    {
        public static string Serialize(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T Deserialize<T>(this string json)
        {
            if (json.IsNullOrEmpty())
                return default;
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
