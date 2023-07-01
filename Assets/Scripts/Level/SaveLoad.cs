using System.IO;
using UnityEngine;

public static class SaveLoad
{
    private static SaveData saveData;
    private static string saveFilePath;

    public static int LevelProgress
    {
        get => saveData.levelProgress;

        set
        {
            saveData.levelProgress = value;
            WriteFile();
        }
    }

    static SaveLoad()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath,
            "save.json");

        if (File.Exists(saveFilePath))
        {
            string data = File.ReadAllText(saveFilePath);
            saveData = JsonUtility.FromJson<SaveData>(data);
        }
        else
        {
            saveData = new SaveData();
        }
    }

    private static void WriteFile()
    {
        string data = JsonUtility.ToJson(saveData);
        File.WriteAllText(saveFilePath, data);
    }
}
