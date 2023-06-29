using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript007 : BaseEventScript
{
    public override bool CheckPlayCondition()
    {
        if (base.CheckPlayCondition())
        {
            if (UnitManager._instance.CheckHack(GameManager._instance.activeUnit.position))
            {
                return true;
            }
        }
        return false;
    }

    public override void EventEffect()
    {
        SelectionResult targetSelInput = new SelectionResult { baseUnits = new List<BaseUnit>(), positionKeys = new List<PositionKey>(), wallUnits = new List<WallUnit>() };

        foreach (var tile in UnitManager._instance.units)
        {
            foreach (var unit in tile.Value)
            {
                if (unit is IHackable)
                {
                    IHackable temp = (IHackable)unit;
                    if (temp.Hackable() && UnitManager._instance.CheckReachType(temp.GetHackReachType(), unit.position, GameManager._instance.activeUnit.position))
                    {
                        targetSelInput.baseUnits.Add(unit);
                    }
                    //temp.GetHacked(GameManager._instance.activeUnit.CurrInt);
                }
            }
        }

        foreach (var wall in UnitManager._instance.wallUnits)
        {
            if (wall.Value is IHackable)
            {
                IHackable temp = (IHackable)wall.Value;
                if (temp.Hackable() && UnitManager._instance.CheckReachType(temp.GetHackReachType(), wall.Value.position, GameManager._instance.activeUnit.position))
                {
                    targetSelInput.wallUnits.Add(wall.Value);
                }
                //temp.GetHacked(GameManager._instance.activeUnit.CurrInt);
            }
        }
        MapTargetSelector._instance.StartTargetSelection(targetSelInput, TargetHackCallback);
    }
    public void TargetHackCallback(SelectionResult targetSelOutput)
    {
        foreach (var unit in targetSelOutput.baseUnits)
        {
            if (unit is IHackable)
            {
                IHackable temp = (IHackable)unit;
                if (temp.Hackable())
                    temp.GetHacked(GameManager._instance.activeUnit.CurrInt + data.Variables[0]);
            }
        }
        foreach (var unit in targetSelOutput.wallUnits)
        {
            if (unit is IHackable)
            {
                IHackable temp = (IHackable)unit;
                if (temp.Hackable())
                    temp.GetHacked(GameManager._instance.activeUnit.CurrInt + data.Variables[0]);
            }
        }

        GameManager._instance.activeUnit.PerformAction(ActionType.hack);
        DeleventSystem.playerUnitUpdate();
        DeleventSystem.enemyUnitUpdate();
        DeleventSystem.mapVisualsUpdate();
    }
}
