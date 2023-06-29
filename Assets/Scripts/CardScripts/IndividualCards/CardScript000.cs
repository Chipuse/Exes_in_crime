using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript000 : BaseEventScript
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
                        if(unit is PlayerUnit && unit != GameManager._instance.activeUnit)
                        {
                            if (UnitManager._instance.CheckHack(unit.position))
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

        //hack that for me

        //1. choose associate in range
        SelectionResult targetSelInput = new SelectionResult { baseUnits = new List<BaseUnit>(), positionKeys = new List<PositionKey>(), wallUnits = new List<WallUnit>() };

        foreach (var tile in Pathfinder._instance.SimplePathFindCast(GameManager._instance.activeUnit.position, data.Variables[0]))
        {
            if (UnitManager._instance.units.ContainsKey(tile))
            {
                foreach (var unit in UnitManager._instance.units[tile])
                {
                    if (unit is PlayerUnit && unit != GameManager._instance.activeUnit)
                    {
                        if (UnitManager._instance.CheckHack(unit.position))
                        {
                            targetSelInput.baseUnits.Add(unit);
                        }
                    }
                }
            }
        }

        MapTargetSelector._instance.StartTargetSelection(targetSelInput, TargetAssociateCallback);
        //2. choose hacking target for associate

        //hack the thing
    }

    public void TargetAssociateCallback(SelectionResult targetSelOutput)
    {
        SelectionResult targetSelInput = new SelectionResult { baseUnits = new List<BaseUnit>(), positionKeys = new List<PositionKey>(), wallUnits = new List<WallUnit>() };
        chosenAssociate = (PlayerUnit)targetSelOutput.baseUnits[0];
        foreach (var tile in UnitManager._instance.units)
        {
            foreach (var unit in tile.Value)
            {
                if (unit is IHackable)
                {
                    IHackable temp = (IHackable)unit;
                    if (temp.Hackable() && UnitManager._instance.CheckReachType(temp.GetHackReachType(), unit.position, chosenAssociate.position))
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
                if (temp.Hackable() && UnitManager._instance.CheckReachType(temp.GetHackReachType(), wall.Value.position, chosenAssociate.position))
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
                    temp.GetHacked(chosenAssociate.CurrInt + GameManager._instance.activeUnit.CurrCha);
            }
        }
        foreach (var unit in targetSelOutput.wallUnits)
        {
            if (unit is IHackable)
            {
                IHackable temp = (IHackable)unit;
                if (temp.Hackable())
                    temp.GetHacked(chosenAssociate.CurrInt + GameManager._instance.activeUnit.CurrCha);
            }
        }

        chosenAssociate.PerformAction(ActionType.hack);
        DeleventSystem.playerUnitUpdate();
        DeleventSystem.enemyUnitUpdate();
        DeleventSystem.mapVisualsUpdate();
    }
}
