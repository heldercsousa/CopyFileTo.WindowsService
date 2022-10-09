using Newtonsoft.Json;

namespace CopyFileToWindowsService
{
    public class ConfigLoad
    {
        public ConfigModel Model { get; private set; }

        public ConfigLoad()
        {
            Model = new ConfigModel();
        }
        public void LoadJson()
        {
            var json = File.ReadAllText("config.json");
            Model = JsonConvert.DeserializeObject<ConfigModel>(json);
        }        
    }
}