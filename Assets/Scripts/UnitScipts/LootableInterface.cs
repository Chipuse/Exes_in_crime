using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILootable 
{ 
    // Start is called before the first frame update
    bool Lootable();
    BaseCardScript GetLooted();
    ReachType GetLootReachType();
}
