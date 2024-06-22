using System;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public ObjectPlacer objectPlacer;

    private void Start()
    {
        // Asigna el objectPlacer si no está asignado desde el editor
        if (objectPlacer == null)
        {
            objectPlacer = FindObjectOfType<ObjectPlacer>();
            if (objectPlacer == null)
            {
                Debug.LogError("ObjectPlacer component not found in the scene.");
                return;
            }
        }
        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void SaveGame()
    {
        if (objectPlacer == null)
        {
            Debug.LogError("ObjectPlacer is not assigned.");
            return;
        }

        var fullPath = Path.Combine(Application.persistentDataPath, "data.game");

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string json = objectPlacer.ToJson();
            Debug.Log("Saving data: " + json);

            using FileStream stream = new(fullPath, FileMode.Create);
            using StreamWriter writer = new(stream);

            writer.Write(json);
        }
        catch (Exception e)
        {
            Debug.LogError("Error occurred while saving data to the file: " + fullPath + "\n" + e);
        }
    }

    public void LoadGame()
    {
        if (objectPlacer == null)
        {
            Debug.LogError("ObjectPlacer is not assigned.");
            return;
        }

        var fullPath = Path.Combine(Application.persistentDataPath, "data.game");

        if (!File.Exists(fullPath)) return;
        try
        {
            using FileStream stream = new(fullPath, FileMode.Open);
            using StreamReader reader = new(stream);

            string json = reader.ReadToEnd();

            objectPlacer.FromJson(json);
        }
        catch (Exception e)
        {
            Debug.LogError("Error occurred while loading data from the file: " + fullPath + "\n" + e);
        }
    }
}