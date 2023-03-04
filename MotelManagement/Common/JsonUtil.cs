using Newtonsoft.Json;

namespace MotelManagement.Common
{
    public class JsonUtil
    {
        public static String SerializeObject(Object t) { 
            return JsonConvert.SerializeObject(t);
        } 

        public static object DeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
