using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Configuration")]
    [SerializeField] private string fileName = "data.game";

    private GameData gameData;

    private static List<IDataPersistence> dataPersistenceObjects;

    private FileDataHandler dataHanlder;

    public static DataPersistenceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more that one Data Persistence Manager in the scene.");
        }
        Instance = this;
    }

    private void Start()
    {
        dataHanlder = new FileDataHandler(Application.persistentDataPath, fileName);
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public void NewGame()
    {
        gameData = new();
    }

    public void LoadGame()
    {
        gameData = dataHanlder.Load();

        if (gameData == null)
        {
            Debug.Log("No data was found. Initializing data to default.");
            NewGame();
        }

        foreach (var dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach (var dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        dataHanlder.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}