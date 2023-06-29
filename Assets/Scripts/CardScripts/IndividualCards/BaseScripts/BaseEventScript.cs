using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEventScript : BaseCardScript
{    public override void PlayFromHand()
    {
        GameManager._instance.AddSaveState();
        GameManager._instance.activeUnit.CurrAP -= data.Cost;        
        EventEffect();
        int index = CardManager._instance.handCards.FindIndex(u => u.gameObject.GetInstanceID() == gameObject.GetInstanceID());
        CardManager._instance.handCards.RemoveAt(index);
        CardManager._instance.discardCards.Add(this);
        CardVisHand._instance.UpdateHandCards();
    }

    public virtual void EventEffect()
    {
        Debug.Log("played event card: " + data.Name);
    }
}
