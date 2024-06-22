using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedGameObjects = new();

    // Lista de datos serializables
    private List<PlacedObjectData> placedObjectDataList = new();


    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;
        placedGameObjects.Add(newObject);

        // Guardar los datos del objeto colocado
        placedObjectDataList.Add(new PlacedObjectData(prefab.name, position));

        return placedGameObjects.Count - 1;
    }

    internal void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null)
            return;

        Destroy(placedGameObjects[gameObjectIndex]);
        placedGameObjects[gameObjectIndex] = null;

        // También remover los datos
        placedObjectDataList.RemoveAt(gameObjectIndex);
    }

    // Método para convertir los datos a JSON
    public string ToJson()
    {
        Debug.Log("Saving " + placedObjectDataList.Count + " objects");
        return JsonUtility.ToJson(new PlacedObjectDataListWrapper { placedObjectData = placedObjectDataList }, true);
    }

    // Método para cargar los datos desde JSON
    public void FromJson(string json)
    {
        PlacedObjectDataListWrapper wrapper = JsonUtility.FromJson<PlacedObjectDataListWrapper>(json);
        placedObjectDataList = wrapper.placedObjectData;

        // Limpiar los objetos existentes antes de recrear
        foreach (var go in placedGameObjects)
        {
            if (go != null)
                Destroy(go);
        }
        placedGameObjects.Clear();

        // Recrear los objetos colocados desde los datos cargados
        foreach (var data in placedObjectDataList)
        {
            GameObject prefab = Resources.Load<GameObject>(data.prefabName);
            if (prefab != null)
            {
                GameObject newObject = Instantiate(prefab);
                newObject.transform.position = data.position;
                placedGameObjects.Add(newObject);
            }
            else
            {
                Debug.LogError("Could not find prefab: " + data.prefabName);
            }
        }
    }

    [Serializable]
    private class PlacedObjectDataListWrapper
    {
        public List<PlacedObjectData> placedObjectData;
    }
}
