using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TPosition, TData> : Dictionary<TPosition, TData>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TPosition> positions = new();

    [SerializeField]
    private List<TData> datas = new();

    public void OnBeforeSerialize()
    {
        positions.Clear();
        datas.Clear();

        foreach (var pair in this)
        {
            positions.Add(pair.Key);
            datas.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        Clear();

        if (positions.Count != datas.Count)
            throw new System.Exception($"There are {positions.Count} positions and {datas.Count} datas after deserialization. Make sure that both key and value types are serializable.");

        for (int i = 0; i < positions.Count; i++)
        {
            Add(positions[i], datas[i]);
        }
    }
}

[Serializable]
public class Vector3IntPlacementDataDictionary : SerializableDictionary<Vector3Int, PlacementData> { }