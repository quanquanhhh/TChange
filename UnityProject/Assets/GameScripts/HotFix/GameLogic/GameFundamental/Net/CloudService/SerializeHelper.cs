using System.Text;
using Newtonsoft.Json;

namespace GameScripts
{
    public class SerializeHelper
    {
        public static byte[] SerializeToByteArray<T>(T obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return Encoding.UTF8.GetBytes(json);
        }

        public static T DeserializeFromByteArray<T>(byte[] data)
        {
            var json = Encoding.UTF8.GetString(data);
            return JsonConvert.DeserializeObject<T>(json);
        }
        
    }
}