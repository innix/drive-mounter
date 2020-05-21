using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace DriveMounter
{
    public class DriveLetterCache
    {
        private const string cacheFile = "drivelettercache.json";

        private readonly Dictionary<string, string> cached = new Dictionary<string, string>();

        public DriveLetterCache()
        {
            if (File.Exists(cacheFile))
            {
                string json = File.ReadAllText(cacheFile);
                cached = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
        }

        public void SaveDriveLetter(WindowsVolume volume)
        {
            cached[volume.ID] = volume.DriveLetter;

            string json = JsonConvert.SerializeObject(cached, Formatting.Indented);
            File.WriteAllText(cacheFile, json);
        }

        public void DeleteDriveLetter(WindowsVolume volume)
        {
            if (!cached.Remove(volume.ID))
            {
                return;
            }

            if (cached.Count == 0)
            {
                File.Delete(cacheFile);
                return;
            }

            string json = JsonConvert.SerializeObject(cached, Formatting.Indented);
            File.WriteAllText(cacheFile, json);
        }

        public string GetDriveLetter(WindowsVolume volume)
        {
            if (cached.TryGetValue(volume.ID, out string driveLetter))
            {
                return driveLetter;
            }

            return null;
        }
    }
}
