using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript010 : BaseBodyScript
{
    public override void ModifyUnitStats(PlayerUnit _unit)
    {
        base.ModifyUnitStats(_unit);
        _unit.CurrMove += 1;
    }
}
