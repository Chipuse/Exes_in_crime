using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript047 : BaseBodyScript
{
    //Mechanic Uniform

    public override bool HandSlotExceptions(PlayerUnit unit)
    {
        if (base.HandSlotExceptions(unit))
        {
            return true;
        }
        foreach (var type in unit.handSlot.data.Type)
        {
            if(type == "smart")
            {
                foreach (var typeTwo in unit.handSlot.data.Type)
                {
                    if (typeTwo == "Device")
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public override int ModifyDisguiseLevel(PlayerUnit unit)
    {
        if (HandSlotExceptions(unit))
            return data.Variables[0];
        return 0;
    }
    public override bool ModifyPerformedAction(ActionType _action)
    {
        if (_action == ActionType.hack)
            return true;
        return false;
    }
}
