using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript003 : BaseEventScript
{
    public override void EventEffect()
    {
        base.EventEffect();
        EmittedSound.NoiseEventOnUnit(GameManager._instance.activeUnit, data.Variables[0], .01f);
    }
}
