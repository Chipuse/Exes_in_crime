using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISerializableUnit
{
    SerializedDataContainer Serialize();
    void Deserialize(SerializedDataContainer input);

    SerializableClasses GetSerializableType();
}

public enum SerializableClasses
{    
    undef = -1,
    unitManager = 0,
    baseUnit = 1,
    playerUnit = 2,
    enemyUnit = 3,
    wallUnit = 4,
    gameState = 5,
    cardManager = 6
    //etc
}
