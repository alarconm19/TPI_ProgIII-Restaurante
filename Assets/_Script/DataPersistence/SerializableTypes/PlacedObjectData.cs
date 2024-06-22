using System;
using UnityEngine;

[Serializable]
public class PlacedObjectData
{
    public string prefabName;
    public Vector3 position;

    public PlacedObjectData(string prefabName, Vector3 position)
    {
        this.prefabName = prefabName;
        this.position = position;
    }
}