using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKeys, TValues> : Dictionary<TKeys, TValues>, ISerializationCallbackReceiver
{
    [SerializeField]
    public List<TKeys> keys = new();

    [SerializeField]
    public List<TValues> values = new();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach (var pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        Clear();

        if (keys.Count != values.Count)
            throw new Exception($"There are {keys.Count} positions and {values.Count} datas after deserialization. Make sure that both key and value types are serializable.");

        for (int i = 0; i < keys.Count; i++)
        {
            Add(keys[i], values[i]);
        }
    }
}

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