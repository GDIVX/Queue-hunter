using System.Collections.Generic;
using UnityEngine;

namespace Tzipory.Systems.SaveLoadSystem.PlayerPref
{
    public static class GameSaveUtilityPlayerPrefs
    {
        public static void SaveObject<T>(string key, T data) where T : struct , ISave
        {
            string dataAsString = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(key, dataAsString);
            PlayerPrefs.Save();
            Debug.Log($"The file have been saved successfully key: {key}");
        }

        public static bool LoadObject<T>(string key,out T saveData) where T : struct , ISave
        {
            if (!PlayerPrefs.HasKey(key))
            {
                saveData = default;
                Debug.LogWarning($"Can not find save data using key: {key}");
                return false;
            }
            
            string dataAsString = PlayerPrefs.GetString(key);
            saveData = JsonUtility.FromJson<T>(dataAsString);
            Debug.Log($"The file have been loaded successfully key: {key}");
            return true;
        }
        
        public static void SaveObjects<T>(string key, IEnumerable<T> data) where T : struct, ISave
        {
            SerializeArray<T> serializeData = new SerializeArray<T>(data);

            string dataAsString = JsonUtility.ToJson(serializeData);
            PlayerPrefs.SetString(key, dataAsString);
            PlayerPrefs.Save();
            Debug.Log($"The files have been saved successfully key: {key}");
        }

        public static bool LoadObjects<T>(string key,out IEnumerable<T> saveData) where T : struct ,ISave 
        {
            if (!PlayerPrefs.HasKey(key))
            {
                saveData = null;
                Debug.LogWarning($"Can not find save data using key: {key}");
                return false;
            }
            
            string dataAsString = PlayerPrefs.GetString(key);
            SerializeArray<T> serializeData = JsonUtility.FromJson<SerializeArray<T>>(dataAsString);
            saveData = serializeData.SavedData;
            Debug.Log($"The files have been loaded successfully key: {key}");
            return true;
        }

        public static bool DeleteKey(string key)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                Debug.LogWarning($"Can not find and delete save data using key: {key}");
                return false;
            }
            
            PlayerPrefs.DeleteKey(key);
            Debug.Log($"The file have been deleted successfully key: {key}");
            return true;
        }
    }
}
