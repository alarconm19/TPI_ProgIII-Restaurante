using System;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public ObjectPlacer objectPlacer;

    // Singleton pattern (opcional)
    //public static SaveLoadManager Instance { get; private set; }

    // private void Awake()
    // {
    //     if (Instance == null)
    //     {
    //         Instance = this;
    //         DontDestroyOnLoad(gameObject);  // Mantiene el objeto entre escenas si es necesario
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    private void Start()
    {
        // Buscar GridDataManager en la escena
        var gridDataManager = FindObjectOfType<GridDataManager>();
        if (gridDataManager == null)
        {
            Debug.LogError("GridDataManager not found in the scene.");
            return;
        }

        // Buscar ObjectPlacer en la escena
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

        var fullPath = Path.Combine(Application.persistentDataPath, "data.game");

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            var gridDataManager = GridDataManager.Instance;
            if (gridDataManager == null)
            {
                Debug.LogError("GridDataManager instance is not set.");
                return;
            }

            string gridDataJson = gridDataManager.ToJson();
            string objectPlacerJson = objectPlacer.ToJson();

            using FileStream stream = new(fullPath, FileMode.Create);
            using StreamWriter writer = new(stream);

            writer.Write(gridDataJson);
            writer.Write(objectPlacerJson);
        }
        catch (Exception e)
        {
            Debug.LogError("Error occurred while saving data to the file: " + fullPath + "\n" + e);
        }
    }

    public void LoadGame()
    {
        // if (PlayerPrefs.HasKey("GridData"))
        // {
        //     string json = PlayerPrefs.GetString("GridData");
        //     objectPlacer.FromJson(json);
        // }

        var fullPath = Path.Combine(Application.persistentDataPath, "data.game");

        if (!File.Exists(fullPath)) return;
        try
        {
            using FileStream stream = new(fullPath, FileMode.Open);
            using StreamReader reader = new(stream);

            //objectPlacer.FromJson(reader.ReadToEnd());

            string gridDataJson = reader.ReadToEnd();
            string objectPlacerJson = reader.ReadToEnd();

            var gridDataManager = GridDataManager.Instance;
            if (gridDataManager != null)
            {
                gridDataManager.FromJson(gridDataJson);
            }
            objectPlacer.FromJson(objectPlacerJson);
        }
        catch (Exception e)
        {
            Debug.LogError("Error occurred while loading data from the file: " + fullPath + "\n" + e);
        }
    }
}