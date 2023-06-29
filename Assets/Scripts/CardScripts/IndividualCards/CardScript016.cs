using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript016 : BaseEventScript
{
    public override void EventEffect()
    {
        SelectionResult targetSelInput = new SelectionResult { baseUnits = new List<BaseUnit>(), positionKeys = new List<PositionKey>(), wallUnits = new List<WallUnit>() };
        targetSelInput.positionKeys = Pathfinder._instance.GeneralPathFindingCast(GameManager._instance.activeUnit.position, GameManager._instance.activeUnit.CurrMove + data.Variables[0], true);
        MapTargetSelector._instance.StartTargetSelection(targetSelInput, TargetCallback);
    }
    public void TargetCallback(SelectionResult targetSelOutput)
    {
        if (targetSelOutput.positionKeys.Count > 0)
        {
            GameManager._instance.activeUnit.MoveUnit(Pathfinder._instance.GetPath(GameManager._instance.activeUnit.position, targetSelOutput.positionKeys[0], true));
        }
        DeleventSystem.playerUnitUpdate();
        DeleventSystem.enemyUnitUpdate();
        DeleventSystem.mapVisualsUpdate();
    }
}
