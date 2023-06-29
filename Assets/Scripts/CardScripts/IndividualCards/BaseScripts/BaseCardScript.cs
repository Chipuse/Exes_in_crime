using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCardScript : MonoBehaviour
{
    public static int UniqueIDCounter = 0;
    public CardData data;
    public int UniqueID;

    public virtual bool CheckPlayCondition()
    {
        if(GameManager._instance.activeUnit.CurrAP >= data.Cost && CardTester.CheckRequForUnit(data, GameManager._instance.activeUnit))
        {
            return true;
        }
        return false;
    }

    public virtual void PlayFromHand()
    {
        GameManager._instance.AddSaveState();
        GameManager._instance.activeUnit.CurrAP -= data.Cost;
        Debug.Log("played card: " + data.Name);

        int index = CardManager._instance.handCards.FindIndex(u => u.gameObject.GetInstanceID() == gameObject.GetInstanceID());
        CardManager._instance.handCards.RemoveAt(index);
        //CardManager._instance.handCards.Remove(this);
        CardManager._instance.discardCards.Add(this);
        CardVisHand._instance.UpdateHandCards();
    }
}
