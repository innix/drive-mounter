using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DriveMounter
{
    public class DriveLetterCache
    {
        private const string cacheFile = "drivelettercache.json";

        private static readonly string[] cacheFileWarningText = {
            "// It is not advised to delete this file.",
            "// DriveMounter uses this cache to restore the original drive letter when remounting volumes.",
            "// If this file is deleted, DriveMounter will prompt you for the drive letter instead."
        };

        private Dictionary<string, string> cached = new Dictionary<string, string>();

        public void SaveDriveLetter(WindowsVolume volume)
        {
            LoadCache();

            cached[volume.ID] = volume.DriveLetter;

            string json = JsonConvert.SerializeObject(cached, Formatting.Indented);
            File.WriteAllText(cacheFile, $"{WarningText}{Environment.NewLine}{json}");
        }

        public void DeleteDriveLetter(WindowsVolume volume)
        {
            LoadCache();

            if (!cached.Remove(volume.ID))
            {
                return;
            }

            // Tidy up file system if nothing cached.
            if (cached.Count == 0)
            {
                File.Delete(cacheFile);
                return;
            }

            string json = JsonConvert.SerializeObject(cached, Formatting.Indented);
            File.WriteAllText(cacheFile, $"{WarningText}{Environment.NewLine}{json}");
        }

        public string GetDriveLetter(WindowsVolume volume)
        {
            LoadCache();

            if (cached.TryGetValue(volume.ID, out string driveLetter))
            {
                return driveLetter;
            }

            return null;
        }

        private void LoadCache()
        {
            if (File.Exists(cacheFile))
            {
                string json = File.ReadAllText(cacheFile);
                cached = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
            else if (cached == null)
            {
                cached = new Dictionary<string, string>();
            }
        }

        private static string WarningText => string.Join(Environment.NewLine, cacheFileWarningText);
    }
}
