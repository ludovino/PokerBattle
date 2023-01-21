using System;
using System.IO;
using UnityEngine;

public class SaveManager : ScriptableObject
{
    private static SaveManager _instance;
    public static SaveManager Instance => _instance != null ? _instance : SetInstance();
    private static SaveManager SetInstance()
    {
        return _instance = Resources.Load<SaveManager>("SaveManager");
    }
    private static string SavePath => Application.persistentDataPath;
    private static string SaveFileName(string UniqueName) => Path.Combine(SavePath, $"{UniqueName}.save");
    public void SaveItem<T>(ISaveable<T> saveable) where T : class
    {
        var path = SaveFileName(saveable.UniqueName);
        var json = JsonUtility.ToJson(saveable.Save());
        File.WriteAllText(path, json);
    }

    public void LoadItem<T>(ISaveable<T> saveable) where T : class
    {
        T item;
        try
        {
            var fileString = File.ReadAllText(SaveFileName(saveable.UniqueName));
            item = JsonUtility.FromJson<T>(fileString);
        }
        catch(Exception ex)
        {
            Debug.Log(ex);
            File.Create(SaveFileName(saveable.UniqueName));
            item = default;
        }
        saveable.Load(item);
    }
}

