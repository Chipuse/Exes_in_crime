using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBodyScript : BaseCardScript
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

        if (GameManager._instance.activeUnit.bodySlot != null)
        {
            GameManager._instance.activeUnit.inventory.Add(GameManager._instance.activeUnit.bodySlot);
            //case if inventory is full lol ToDo
        }
        GameManager._instance.activeUnit.bodySlot = this;
        CardVisHand._instance.UpdateHandCards();
        DeleventSystem.enemyUnitUpdate();
        DeleventSystem.playerUnitUpdate();
    }

    public virtual SuspiciousLevel ModifySusLevel()
    {
        return SuspiciousLevel.Unsuspicious;
    }

    public virtual int ModifyDisguiseLevel(PlayerUnit unit)
    {
        return 0;
    }

    public virtual bool HandSlotExceptions(PlayerUnit unit)
    {
        if (unit.handSlot == null)
            return true;
        else
        {
            foreach (var type in unit.handSlot.data.Type)
            {
                if(type == "inconspicuous")
                {
                    return true;
                }
            }
        }
        return false;
    }

    public virtual bool ModifyPerformedAction(ActionType _action)
    {
        return false;
    }

    public virtual void ModifyUnitStats(PlayerUnit _unit)
    {

    }
}
