using System;

[Serializable]
public class GameData
{
    public Vector3IntPlacementDataDictionary placedObjects;

    public GameData()
    {
        placedObjects = new();
    }
}