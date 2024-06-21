using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    void LoadData(GameData gridData);
    void SaveData(ref GameData gridData);
}
