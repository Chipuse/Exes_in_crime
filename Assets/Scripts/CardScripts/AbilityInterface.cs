using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbility
{
    bool CheckAbilityCondition();
    void TriggerAbility();
}
