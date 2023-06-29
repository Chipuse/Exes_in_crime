using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript056 : BaseBodyScript
{
    public override int ModifyDisguiseLevel(PlayerUnit unit)
    {
        if (HandSlotExceptions(unit))
            return data.Variables[0];
        return 0;
    }
}
