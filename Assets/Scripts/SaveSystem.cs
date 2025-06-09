using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public static class SaveSystem
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "gamesave.dat");

    [System.Serializable]
    public class SaveData
    {
        public int anomaliesFound;
        public int totalAnomalies;
        public string currentScene;
        public List<int> foundAnomalyIDs = new List<int>();
    }

    public static void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(SavePath, FileMode.Create);

        SaveData data = new SaveData
        {
            anomaliesFound = InteractiveAnomaly.GetAnomalyCount(),
            totalAnomalies = InteractiveAnomaly.GetTotalAnomalies(),
            currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
            foundAnomalyIDs = InteractiveAnomaly.GetFoundAnomalyIDs()
        };

        Debug.Log($"Saving game data: Scene={data.currentScene}, Found={data.anomaliesFound}, Total={data.totalAnomalies}");
        Debug.Log($"Save file location: {SavePath}");

        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("Game saved successfully!");
    }

    public static SaveData LoadGame()
    {
        if (File.Exists(SavePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(SavePath, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            Debug.Log($"Loading game data: Scene={data.currentScene}, Found={data.anomaliesFound}, Total={data.totalAnomalies}");
            Debug.Log($"Load file location: {SavePath}");

            return data;
        }
        else
        {
            Debug.LogWarning($"No save file found at: {SavePath}");
            return null;
        }
    }

    public static bool SaveExists()
    {
        bool exists = File.Exists(SavePath);
        Debug.Log($"Checking save file: {SavePath} - Exists: {exists}");
        return exists;
    }
} 