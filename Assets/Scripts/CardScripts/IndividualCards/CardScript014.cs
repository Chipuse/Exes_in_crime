using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript014 : BaseEventScript
{
    public override bool CheckPlayCondition()
    {
        if (base.CheckPlayCondition())
        {
            if (UnitManager._instance.CheckAttack(GameManager._instance.activeUnit.position))
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
                if (unit is IAttackable)
                {
                    IAttackable temp = (IAttackable)unit;
                    if (temp.Attackable() && UnitManager._instance.CheckReachType(temp.GetAttackReachType(), unit.position, GameManager._instance.activeUnit.position))
                    {
                        targetSelInput.baseUnits.Add(unit);
                    }
                    //temp.GetHacked(GameManager._instance.activeUnit.CurrInt);
                }
            }
        }

        foreach (var wall in UnitManager._instance.wallUnits)
        {
            if (wall.Value is IAttackable)
            {
                IAttackable temp = (IAttackable)wall.Value;
                if (temp.Attackable() && UnitManager._instance.CheckReachType(temp.GetAttackReachType(), wall.Value.position, GameManager._instance.activeUnit.position))
                {
                    targetSelInput.wallUnits.Add(wall.Value);
                }
                //temp.GetHacked(GameManager._instance.activeUnit.CurrInt);
            }
        }
        MapTargetSelector._instance.StartTargetSelection(targetSelInput, TargetAttackCallback);
    }
    public void TargetAttackCallback(SelectionResult targetSelOutput)
    {
        foreach (var unit in targetSelOutput.baseUnits)
        {
            if (unit is IAttackable)
            {
                IAttackable temp = (IAttackable)unit;
                if (temp.Attackable())
                    temp.GetAttacked(GameManager._instance.activeUnit.CurrAtk + data.Variables[0]);
            }
        }
        foreach (var unit in targetSelOutput.wallUnits)
        {
            if (unit is IAttackable)
            {
                IAttackable temp = (IAttackable)unit;
                if (temp.Attackable())
                    temp.GetAttacked(GameManager._instance.activeUnit.CurrAtk + data.Variables[0]);
            }
        }

        GameManager._instance.activeUnit.PerformAction(ActionType.attack);
        DeleventSystem.playerUnitUpdate();
        DeleventSystem.enemyUnitUpdate();
        DeleventSystem.mapVisualsUpdate();
    }
}
