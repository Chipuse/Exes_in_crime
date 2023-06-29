using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript028 : BaseHandScript, IAbility
{
    public bool CheckAbilityCondition()
    {
        if (GameManager._instance.activeUnit.CurrAP >= data.Variables[0])
        {
            foreach (var tile in UnitManager._instance.units)
            {
                foreach (var unit in tile.Value)
                {
                    if (unit is IHackable)
                    {
                        IHackable temp = (IHackable)unit;
                        if (temp.Hackable() && UnitManager._instance.CheckReachType(temp.GetHackReachType(), unit.position, GameManager._instance.activeUnit.position))
                        {
                            return true;
                        }
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
                        return true;
                    }
                }
            }

        }
        return false;
    }

    public void TriggerAbility()
    {
        GameManager._instance.AddSaveState();
        GameManager._instance.activeUnit.CurrAP -= data.Variables[0];

        SelectionResult targetSelInput = new SelectionResult { baseUnits = new List<BaseUnit>(), positionKeys = new List<PositionKey>(), wallUnits = new List<WallUnit>() };
        //Check all hackable units if they are reached by this tile
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
                    //temp.GetAttacked(GameManager._instance.activeUnit.CurrAtk);
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

        MapTargetSelector._instance.StartTargetSelection(targetSelInput, OnAbilityTargetCallBack);
    }

    public void OnAbilityTargetCallBack(SelectionResult targetSelOutput)
    {

        foreach (var unit in targetSelOutput.baseUnits)
        {
            if (unit is IHackable)
            {
                IHackable temp = (IHackable)unit;
                if (temp.Hackable())
                {
                    temp.GetHacked(GameManager._instance.activeUnit.CurrInt + data.Variables[1]);
                }
            }
        }

        foreach (var unit in targetSelOutput.wallUnits)
        {
            if (unit is IHackable)
            {
                IHackable temp = (IHackable)unit;
                if (temp.Hackable())
                    temp.GetHacked(GameManager._instance.activeUnit.CurrInt + data.Variables[1]);
            }
        }

        GameManager._instance.activeUnit.PerformAction(ActionType.hack);
        DeleventSystem.playerUnitUpdate();
        DeleventSystem.enemyUnitUpdate();
        DeleventSystem.mapVisualsUpdate();
    }
}
