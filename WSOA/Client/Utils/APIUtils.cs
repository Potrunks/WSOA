using Newtonsoft.Json;
using System.Text;

namespace WSOA.Client.Utils
{
    public static class APIUtils
    {
        public static StringContent ToJsonUtf8(this object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public static T ToObject<T>(this HttpContent content)
        {
            return JsonConvert.DeserializeObject<T>(content.ReadAsStringAsync().Result);
        }
    }
}
