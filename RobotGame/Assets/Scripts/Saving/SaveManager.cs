using UnityEngine;
using System.IO;

public static class SaveManager
{
    //handels serialization of save files
    static string path = (Application.persistentDataPath + "/save.json"); //file path to the save file

    public static void Save(SaveFile saveFile) 
    {
        //serializes give save file to path

        if (!File.Exists(path))
        {
            File.Create(path).Close();
        }

        File.WriteAllText(path, JsonUtility.ToJson(saveFile, true));
    }

    public static SaveFile Load()
    {
        //deserializes and returns a save file from path

        if (File.Exists(path))
        {
            return JsonUtility.FromJson<SaveFile>(File.ReadAllText(path));
        }
        return null;
    }
}
