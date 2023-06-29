using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript046 : BaseBodyScript
{
    //Museum guard uniform
    public override bool HandSlotExceptions(PlayerUnit unit)
    {
        if (base.HandSlotExceptions(unit))
        {
            return true;
        }
        foreach (var type in unit.handSlot.data.Type)
        {
            if (type == "legal")
            {
                foreach (var typeTwo in unit.handSlot.data.Type)
                {
                    if (typeTwo == "Weapon")
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
}
