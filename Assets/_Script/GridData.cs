using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridData
{
    public Dictionary<Vector3Int, PlacementData> PlacedObjects = new();

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