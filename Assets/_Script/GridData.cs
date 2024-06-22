using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridData
{
    public Dictionary<Vector3Int, PlacementData> PlacedObjects = new();

    // public string ToJson()
    // {
    //     var serializableDict = ToSerializable();
    //     return JsonUtility.ToJson(serializableDict, true);
    // }

    // public Vector3IntPlacementDataDictionary ToSerializable()
    // {
    //     var serializableDict = new Vector3IntPlacementDataDictionary();
    //     Debug.Log(PlacedObjects.Count);
    //     foreach (var kvp in PlacedObjects)
    //     {
    //         serializableDict.keys.Add(kvp.Key);
    //         serializableDict.values.Add(kvp.Value);
    //     }
    //     return serializableDict;
    // }

    // public void FromJson(string json)
    // {
    //     var serializableDict = JsonUtility.FromJson<Vector3IntPlacementDataDictionary>(json);
    //     FromSerializable(serializableDict);
    // }

    // public void FromSerializable(Vector3IntPlacementDataDictionary serializableDict)
    // {
    //     PlacedObjects.Clear();
    //     for (int i = 0; i < serializableDict.keys.Count; i++)
    //     {
    //         PlacedObjects.Add(serializableDict.keys[i], serializableDict.values[i]);
    //     }
    // }

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int id, int placedObjectIndex)
    {
        var positionToOccupy = CalculeOccupiedPositions(gridPosition, objectSize);
        PlacementData data = new(positionToOccupy, id, placedObjectIndex);

        foreach (var pos in positionToOccupy.Where(pos => !PlacedObjects.TryAdd(pos, data)))
        {
            throw new InvalidOperationException($"Position already occupied. {pos}");
        }
    }

    private List<Vector3Int> CalculeOccupiedPositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();

        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        var positionToOccupy = CalculeOccupiedPositions(gridPosition, objectSize);

        return positionToOccupy.All(pos => !PlacedObjects.ContainsKey(pos));
    }

    internal int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if(PlacedObjects.ContainsKey(gridPosition) == false)
            return -1;

        return PlacedObjects[gridPosition].PlacedObjectIndex;
    }

    internal void RemoveObjectAt(Vector3Int gridPosition)
    {
        foreach (var pos in PlacedObjects[gridPosition].OccupiedPositions)
        {
            PlacedObjects.Remove(pos);
        }
    }

    public string ToJson()
    {
        var wrapper = new DictionaryWrapper
        {
            Keys = PlacedObjects.Keys.ToList(),
            Values = PlacedObjects.Values.ToList()

        };
        Debug.Log(PlacedObjects.Count);

        return JsonUtility.ToJson(wrapper, true);
    }

    public void FromJson(string json)
    {
        var wrapper = JsonUtility.FromJson<DictionaryWrapper>(json);
        PlacedObjects = new Dictionary<Vector3Int, PlacementData>();
        for (int i = 0; i < wrapper.Keys.Count; i++)
        {
            PlacedObjects[wrapper.Keys[i]] = wrapper.Values[i];
        }
    }

    [Serializable]
    private class DictionaryWrapper
    {
        public List<Vector3Int> Keys;
        public List<PlacementData> Values;
    }
}

[Serializable]
public class PlacementData
{
    public List<Vector3Int> OccupiedPositions;

    public int Id { get; private set; }

    public int PlacedObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int id, int placedObjectIndex)
    {
        OccupiedPositions = occupiedPositions;
        Id = id;
        PlacedObjectIndex = placedObjectIndex;
    }

    public override string ToString()
    {
        return $"ID: {Id}, PlacedObjectIndex: {PlacedObjectIndex} at {OccupiedPositions[0]}";
    }
}