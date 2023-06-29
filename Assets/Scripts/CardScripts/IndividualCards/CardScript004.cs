using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript004 : BaseEventScript
{
    //local vars
    PlayerUnit chosenAssociate;

    public override bool CheckPlayCondition()
    {
        if (base.CheckPlayCondition())
        {
            //check if associate is in range + does this associate has something to hack
            foreach (var tile in Pathfinder._instance.SimplePathFindCast(GameManager._instance.activeUnit.position, data.Variables[0]))
            {
                if (UnitManager._instance.units.ContainsKey(tile))
                {
                    foreach (var unit in UnitManager._instance.units[tile])
                    {
                        if (unit is PlayerUnit && unit != GameManager._instance.activeUnit)
                        {
                            if (UnitManager._instance.CheckAttack(unit.position))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    public override void EventEffect()
    {
        base.EventEffect();

        SelectionResult targetSelInput = new SelectionResult { baseUnits = new List<BaseUnit>(), positionKeys = new List<PositionKey>(), wallUnits = new List<WallUnit>() };

        foreach (var tile in Pathfinder._instance.SimplePathFindCast(GameManager._instance.activeUnit.position, data.Variables[0]))
        {
            if (UnitManager._instance.units.ContainsKey(tile))
            {
                foreach (var unit in UnitManager._instance.units[tile])
                {
                    if (unit is PlayerUnit && unit != GameManager._instance.activeUnit)
                    {
                        if (UnitManager._instance.CheckAttack(unit.position))
                        {
                            targetSelInput.baseUnits.Add(unit);
                        }
                    }
                }
            }
        }

        MapTargetSelector._instance.StartTargetSelection(targetSelInput, TargetAssociateCallback);

    }

    public void TargetAssociateCallback(SelectionResult targetSelOutput)
    {
        SelectionResult targetSelInput = new SelectionResult { baseUnits = new List<BaseUnit>(), positionKeys = new List<PositionKey>(), wallUnits = new List<WallUnit>() };
        chosenAssociate = (PlayerUnit)targetSelOutput.baseUnits[0];
        foreach (var tile in UnitManager._instance.units)
        {
            foreach (var unit in tile.Value)
            {
                if (unit is IAttackable)
                {
                    IAttackable temp = (IAttackable)unit;
                    if (temp.Attackable() && UnitManager._instance.CheckReachType(temp.GetAttackReachType(), unit.position, chosenAssociate.position))
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
                if (temp.Attackable() && UnitManager._instance.CheckReachType(temp.GetAttackReachType(), wall.Value.position, chosenAssociate.position))
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
            if (unit is IAttackable)
            {
                IAttackable temp = (IAttackable)unit;
                if (temp.Attackable())
                    temp.GetAttacked(chosenAssociate.CurrAtk + GameManager._instance.activeUnit.CurrCha);
            }
        }
        foreach (var unit in targetSelOutput.wallUnits)
        {
            if (unit is IAttackable)
            {
                IAttackable temp = (IAttackable)unit;
                if (temp.Attackable())
                    temp.GetAttacked(chosenAssociate.CurrAtk + GameManager._instance.activeUnit.CurrCha);
            }
        }

        chosenAssociate.PerformAction(ActionType.hack);
        DeleventSystem.playerUnitUpdate();
        DeleventSystem.enemyUnitUpdate();
        DeleventSystem.mapVisualsUpdate();
    }
}
