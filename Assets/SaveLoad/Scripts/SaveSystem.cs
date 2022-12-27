using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";
    public static void Init()
    {
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }

    public static void Save(string saveString)
    {
        int saveNumber = 1;
        while (File.Exists("save_" + saveNumber + ".txt"))
        {
            saveNumber++;
        }
        File.WriteAllText(SAVE_FOLDER + "save" + saveNumber + ".txt", saveString);
    }

    public static string Load()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);
        FileInfo[] saveFiles = directoryInfo.GetFiles("*.txt");
        FileInfo mostRecentFile = null;
        foreach (FileInfo fileInfo in saveFiles)
        {
            if (mostRecentFile == null)
            {
                mostRecentFile = fileInfo;
            }
            else
            {
                if (fileInfo.LastWriteTime > mostRecentFile.LastWriteTime)
                {
                    mostRecentFile = fileInfo;
                }
            }
        }

        if (mostRecentFile != null)
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + mostRecentFile.FullName);
            return saveString;
        }
        else
        {
            return null;
        }
    }
}
