using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript001 : BaseEventScript
{
    public override bool CheckPlayCondition()
    {
        if (!base.CheckPlayCondition())
            return false;
        foreach (var tile in UnitManager._instance.units)
        {
            foreach (var unit in tile.Value)
            {
                if (unit is IAttackable && !(unit is EnemyUnit))
                {
                    IAttackable temp = (IAttackable)unit;
                    if (temp.Attackable() && UnitManager._instance.CheckReachType(temp.GetAttackReachType(), unit.position, GameManager._instance.activeUnit.position))
                        return true;
                }
            }
        }

        foreach (var wall in UnitManager._instance.wallUnits)
        {
            if (wall.Value is IAttackable)
            {
                IAttackable temp = (IAttackable)wall.Value;
                if (temp.Attackable() && UnitManager._instance.CheckReachType(temp.GetAttackReachType(), wall.Value.position, GameManager._instance.activeUnit.position))
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
        foreach (var tile in UnitManager._instance.units)
        {
            foreach (var unit in tile.Value)
            {
                if (unit is IAttackable && !(unit is EnemyUnit))
                {
                    IAttackable temp = (IAttackable)unit;
                    if (temp.Attackable() && UnitManager._instance.CheckReachType(temp.GetAttackReachType(), unit.position, GameManager._instance.activeUnit.position))
                    {
                        targetSelInput.baseUnits.Add(unit);
                    }
                    //temp.GetAttacked(GameManager._instance.activeUnit.CurrAtk);
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
                //temp.GetAttacked(GameManager._instance.activeUnit.CurrAtk);
            }
        }
        MapTargetSelector._instance.StartTargetSelection(targetSelInput, TargetCallback);
    }

    public void TargetCallback(SelectionResult targetSelOutput)
    {
        foreach (var unit in targetSelOutput.baseUnits)
        {
            if (unit is IAttackable)
            {
                IAttackable temp = (IAttackable)unit;
                if (temp.Attackable())
                {
                    temp.GetAttacked(GameManager._instance.activeUnit.CurrAtk + data.Variables[0]);
                    EmittedSound.NoiseEventOnUnit(unit, data.Variables[1]);
                }
            }
        }
        foreach (var unit in targetSelOutput.wallUnits)
        {
            if (unit is IAttackable)
            {
                IAttackable temp = (IAttackable)unit;
                if (temp.Attackable())
                {
                    temp.GetAttacked(GameManager._instance.activeUnit.CurrAtk + data.Variables[0]);
                    EmittedSound.NoiseEventOnUnit(unit, data.Variables[1]);
                }
            }
        }
        GameManager._instance.activeUnit.PerformAction(ActionType.attack);

        DeleventSystem.playerUnitUpdate();
        DeleventSystem.enemyUnitUpdate();
        DeleventSystem.mapVisualsUpdate();
    }
}
