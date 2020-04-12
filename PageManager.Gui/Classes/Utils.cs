using Newtonsoft.Json;
using System.IO;

namespace PageManager.Gui.Classes {
    public static class Utils {
        public static T DeserializeJson<T>(string json) {
            T jsonObject = JsonConvert.DeserializeObject<T>(json);
            return jsonObject;
        }

        public static T DeserializeJsonFile<T>(string path) {
            var jsonString = File.ReadAllText(path);
            T jsonObject = DeserializeJson<T>(jsonString);
            return jsonObject;
        }
    }
}
