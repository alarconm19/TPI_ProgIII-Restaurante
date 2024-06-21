using UnityEngine;
using System.IO;
using System;

public class FileDataHandler
{
    private readonly string dataDirPath = "";
    private readonly string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new(fullPath, FileMode.Open))
                {
                    using StreamReader reader = new(stream);

                    dataToLoad = reader.ReadToEnd();
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occurred while loading data from the file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData gameData)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(gameData, true);

            using FileStream stream = new(fullPath, FileMode.Create);
            using StreamWriter writer = new(stream);

            writer.Write(dataToStore);
        }
        catch (Exception e)
        {
            Debug.LogError("Error occurred while saving data to the file: " + fullPath + "\n" + e);
        }
    }
}