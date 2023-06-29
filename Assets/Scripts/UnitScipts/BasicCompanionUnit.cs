using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCompanionUnit : BaseUnit
{
    public override void PerformTurn()
    {
        StartCoroutine(TurnBehaviour());
    }
    IEnumerator TurnBehaviour()
    {
        yield return new WaitForSeconds(.3f);
        foreach (var card in inventory)
        {
            if(card is BaseCompanionScript)
            {
                Debug.Log("Perform comapnionAction");
                //perform companion turn routine (this) as argument
            }
        }
        finishedTurn = true;
    }
}
