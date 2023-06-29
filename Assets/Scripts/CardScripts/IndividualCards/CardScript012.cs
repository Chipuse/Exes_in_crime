using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript012 : BaseHandScript, IAbility
{
    public bool CheckAbilityCondition()
    {
        if(GameManager._instance.activeUnit.CurrAP >= data.Variables[0])
        {
            foreach (var tile in UnitManager._instance.units)
            {
                foreach (var unit in tile.Value)
                {
                    if (unit is IAttackable && unit is EnemyUnit)
                    {
                        IAttackable temp = (IAttackable)unit;
                        if (temp.Attackable() && UnitManager._instance.CheckReachType(temp.GetAttackReachType(), unit.position, GameManager._instance.activeUnit.position))
                        {
                            return true;
                        }
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
                if (unit is IAttackable && unit is EnemyUnit)
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

        MapTargetSelector._instance.StartTargetSelection(targetSelInput, OnAbilityTargetCallBack);
    }

    public void OnAbilityTargetCallBack(SelectionResult targetSelOutput)
    {

        foreach (var unit in targetSelOutput.baseUnits)
        {
            if (unit is IAttackable && unit is EnemyUnit)
            {
                IAttackable temp = (IAttackable)unit;
                if (temp.Attackable())
                {
                    temp.GetAttacked(GameManager._instance.activeUnit.CurrAtk + data.Variables[1]);
                }
            }
        }
        
        GameManager._instance.activeUnit.PerformAction(ActionType.attack);
        DeleventSystem.playerUnitUpdate();
        DeleventSystem.enemyUnitUpdate();
        DeleventSystem.mapVisualsUpdate();
    }
}
