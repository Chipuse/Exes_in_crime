using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript002 : BaseEventScript
{
    public override bool CheckPlayCondition()
    {
        if (!base.CheckPlayCondition())
            return false;
        foreach (var unit in GameManager._instance.currentPlayerUnits)
        {
            if(unit != GameManager._instance.activeUnit && unit.alive && unit.CurrAP < unit.BaseAP)
            {
                return true;
            }
        }
        return false;
    }

    public override void EventEffect()
    {
        base.EventEffect();
        SelectionResult targetSelInput = new SelectionResult { baseUnits = new List<BaseUnit>(), positionKeys = new List<PositionKey>(), wallUnits = new List<WallUnit>() };
        //Check all hackable units if they are reached by this tile
        foreach (var unit in GameManager._instance.currentPlayerUnits)
        {
            if (unit != GameManager._instance.activeUnit && unit.alive && unit.CurrAP < unit.BaseAP)
            {
                targetSelInput.baseUnits.Add(unit);
            }
        }
        MapTargetSelector._instance.StartTargetSelection(targetSelInput, TargetCallback);
    }

    public void TargetCallback(SelectionResult targetSelOutput)
    {
        foreach (var unit in targetSelOutput.baseUnits)
        {
            if (unit is PlayerUnit)
            {
                PlayerUnit temp = (PlayerUnit)unit;
                temp.CurrAP += data.Variables[0];
                if (temp.CurrAP >= temp.BaseAP)
                    temp.CurrAP = temp.BaseAP;
            }
        }        

        DeleventSystem.playerUnitUpdate();
        DeleventSystem.enemyUnitUpdate();
        DeleventSystem.mapVisualsUpdate();
    }
}
