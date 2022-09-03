using UnityEngine;

namespace Utils
{
    public sealed class PlayerSettings
    {
        private const string _moroshkoviekochki = "MoroshkovieKochki";
        private const string _masterVolumePrefName = "MasterVolume";

        public static void Init()
        {
            if (HasKey(_moroshkoviekochki))
                return;

            CreateDefaultSettingsPrefs();
        }

        public static float GetMasterVolumeValue() => 
            GetFloat(_masterVolumePrefName);

        public static void SafeMasterVolumeValue(float value) =>
            SetFloat(_masterVolumePrefName, value);
        
        private static void CreateDefaultSettingsPrefs()
        {
            PlayerPrefs.SetString(_moroshkoviekochki, "true");
            PlayerPrefs.SetFloat(_masterVolumePrefName, 1f);
        }

        private static void SetFloat(string key, float value) => 
            PlayerPrefs.SetFloat(key, value);

        private static float GetFloat(string key) => 
            PlayerPrefs.GetFloat(key);

        private static bool HasKey(string key) => 
            PlayerPrefs.HasKey(key);
    }
}