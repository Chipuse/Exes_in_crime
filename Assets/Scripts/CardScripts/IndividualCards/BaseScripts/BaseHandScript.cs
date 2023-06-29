using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHandScript : BaseCardScript
{
    public override void PlayFromHand()
    {
        //base.PlayFromHand();
        GameManager._instance.AddSaveState();
        GameManager._instance.activeUnit.CurrAP -= data.Cost;
        Debug.Log("equipped card: " + data.Name + " to " + GameManager._instance.activeUnit.CharacterName);

        int index = CardManager._instance.handCards.FindIndex(u => u.gameObject.GetInstanceID() == gameObject.GetInstanceID());
        CardManager._instance.handCards.RemoveAt(index);
        //CardManager._instance.handCards.Remove(this);

        if(GameManager._instance.activeUnit.handSlot != null)
        {
            GameManager._instance.activeUnit.inventory.Add(GameManager._instance.activeUnit.handSlot);
            //case if inventory is full lol ToDo
        }
        GameManager._instance.activeUnit.handSlot = this;
        CardVisHand._instance.UpdateHandCards();
        DeleventSystem.playerUnitUpdate();
    }

    public virtual SuspiciousLevel ModifySusLevel()
    {
        return SuspiciousLevel.Unsuspicious;
    }

    public virtual int ModifyDisguiseLevel(int _baseLevel)
    {
        return _baseLevel;
    }

    public virtual void ModifyUnitStats(PlayerUnit _unit)
    {

    }
}
