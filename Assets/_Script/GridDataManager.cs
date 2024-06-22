using UnityEngine;

public class GridDataManager : MonoBehaviour
{
    public static GridDataManager Instance { get; private set; }

    public GridData gridData = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int id, int placedObjectIndex)
    {
        gridData.AddObjectAt(gridPosition, objectSize, id, placedObjectIndex);
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        return gridData.CanPlaceObjectAt(gridPosition, objectSize);
    }

    public int GetRepresentationIndex(Vector3Int gridPosition)
    {
        return gridData.GetRepresentationIndex(gridPosition);
    }

    public void RemoveObjectAt(Vector3Int gridPosition)
    {
        gridData.RemoveObjectAt(gridPosition);
    }

    public string ToJson()
    {
        return gridData.ToJson();
    }

    public void FromJson(string json)
    {
        gridData.FromJson(json);
    }
}
