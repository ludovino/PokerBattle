using System.IO;
using UnityEngine;

public class SaveManager : ScriptableObject
{
    private static string SavePath => Application.persistentDataPath;
    private static string SaveFileName(string UniqueName) => Path.Combine(SavePath, $"{UniqueName}.save");
    public void SaveItem<T>(string UniqueName, T saveData)
    {
        var path = SaveFileName(UniqueName);
        var json = JsonUtility.ToJson(saveData);
        File.WriteAllText(path, json);
    }

    public T LoadItem<T>(string UniqueName)
    {
        var fileString = File.ReadAllText(SaveFileName(UniqueName));
        return JsonUtility.FromJson<T>(fileString);
    }
}

