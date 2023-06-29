using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapActionMenu : MonoBehaviour
{
    public GameObject buttonParent;
    public Button moveButton;
    public Button attackButton;
    public Button hackButton;
    public Button lootButton;
    public Button inventoryButton;
    public Button closeButton;
    private PositionKey lastClickedTile;
    // Start is called before the first frame update
    private void OnEnable()
    {
        DeleventSystem.clickedOnTile += OnTileClickedEvent;

    }

    private void OnDisable()
    {
        DeleventSystem.clickedOnTile -= OnTileClickedEvent;

    }

    void OnTileClickedEvent(PositionKey posKey)
    {
        if(InputManager._instance.currentMode == InputMode.map)
        {
            lastClickedTile = posKey;
            buttonParent.SetActive(false);
            if (posKey == GameManager._instance.activeUnit.position)
            {
                buttonParent.SetActive(true);
                moveButton.interactable = CheckForMove();
                attackButton.interactable = CheckForAttack();
                hackButton.interactable = CheckForHack();
                lootButton.interactable = CheckForLoot();
                closeButton.interactable = true;
                InputManager._instance.SwitchInputMode(InputMode.menu);
            }
            else
            {
                foreach (var unit in GameManager._instance.currentPlayerUnits)
                {
                    if (unit.position == posKey && unit.alive)
                        GameManager._instance.activeUnit = unit;
                }
                DeleventSystem.handVisualsUpdate();
            }
        }
    }

    public void OnMoveButton()
    {
        SelectionResult targetSelInput = new SelectionResult { baseUnits = new List<BaseUnit>(), positionKeys = new List<PositionKey>(), wallUnits = new List<WallUnit>() };
        foreach (var tile in GameManager._instance.activeUnit.reachableTiles)
        {
            if(tile != GameManager._instance.activeUnit.position)
                targetSelInput.positionKeys.Add(tile);
        }
        MapTargetSelector._instance.StartTargetSelection(targetSelInput, OnMoveTargetCallback);
        buttonParent.SetActive(false);

        
    }

    public void OnMoveTargetCallback(SelectionResult targetSelOutput)
    {
        GameManager._instance.AddSaveState();
        if(targetSelOutput.positionKeys.Count > 0)
        {
            GameManager._instance.activeUnit.MoveUnit(Pathfinder._instance.GetPath(GameManager._instance.activeUnit.position, targetSelOutput.positionKeys[0], true), GameManager._instance.activeUnit.CurrMove);
        }

        //GameManager._instance.activeUnit.MoveUnit(Pathfinder._instance.GetPath(GameManager._instance.activeUnit.position, lastClickedTile, true), GameManager._instance.activeUnit.CurrMove);
        GameManager._instance.activeUnit.CurrAP -= 1;
        //InputManager._instance.SwitchInputMode(InputMode.map);
        //buttonParent.SetActive(false);
        DeleventSystem.playerUnitUpdate();
        DeleventSystem.enemyUnitUpdate();
        DeleventSystem.mapVisualsUpdate();
    }

    public void OnAttackButton()
    {
        SelectionResult targetSelInput = new SelectionResult { baseUnits = new List<BaseUnit>(), positionKeys = new List<PositionKey>(), wallUnits = new List<WallUnit>() };  
        //Check all hackable units if they are reached by this tile
        foreach (var tile in UnitManager._instance.units)
        {
            foreach (var unit in tile.Value)
            {
                if (unit is IAttackable)
                {
                    IAttackable temp = (IAttackable)unit;
                    if (temp.Attackable() && UnitManager._instance.CheckReachType(temp.GetAttackReachType(), unit.position, lastClickedTile))
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
                if (temp.Attackable() && UnitManager._instance.CheckReachType(temp.GetAttackReachType(), wall.Value.position, lastClickedTile))
                {
                    targetSelInput.wallUnits.Add(wall.Value);
                }
                //temp.GetAttacked(GameManager._instance.activeUnit.CurrAtk);
            }
        }
        MapTargetSelector._instance.StartTargetSelection(targetSelInput, OnAttackTargetCallback);
        buttonParent.SetActive(false);
    }

    public void OnAttackTargetCallback(SelectionResult targetSelOutput)
    {
        GameManager._instance.AddSaveState();

        foreach (var unit in targetSelOutput.baseUnits)
        {
            if (unit is IAttackable)
            {
                IAttackable temp = (IAttackable)unit;
                if (temp.Attackable())
                {
                    temp.GetAttacked(GameManager._instance.activeUnit.CurrAtk);
                    if(!(unit is EnemyUnit))
                        EmittedSound.NoiseEventOnUnit(unit, 12, 0.01f);
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
                    temp.GetAttacked(GameManager._instance.activeUnit.CurrAtk);
                    EmittedSound.NoiseEventOnUnit(unit, 12, 0.01f);
                }
            }
        }
        
        GameManager._instance.activeUnit.PerformAction(ActionType.attack);

        GameManager._instance.activeUnit.CurrAP -= 1;
        GameManager._instance.activeUnit.ShowPath(lastClickedTile);
        DeleventSystem.playerUnitUpdate();
        DeleventSystem.enemyUnitUpdate();
        DeleventSystem.mapVisualsUpdate();
    }

    public void OnHackButton()
    {
        SelectionResult targetSelInput = new SelectionResult { baseUnits = new List<BaseUnit>(), positionKeys = new List<PositionKey>(), wallUnits = new List<WallUnit>() };
        
        foreach (var tile in UnitManager._instance.units)
        {
            foreach (var unit in tile.Value)
            {
                if (unit is IHackable)
                {
                    IHackable temp = (IHackable)unit;
                    if (temp.Hackable() && UnitManager._instance.CheckReachType(temp.GetHackReachType(), unit.position, lastClickedTile))
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
                if (temp.Hackable() && UnitManager._instance.CheckReachType(temp.GetHackReachType(), wall.Value.position, lastClickedTile))
                {
                    targetSelInput.wallUnits.Add(wall.Value);
                }
                    //temp.GetHacked(GameManager._instance.activeUnit.CurrInt);
            }
        }
        MapTargetSelector._instance.StartTargetSelection(targetSelInput, OnHackTargetCallback);
        buttonParent.SetActive(false);
        
    }

    public void OnHackTargetCallback(SelectionResult targetSelOutput)
    {
        GameManager._instance.AddSaveState();

        foreach (var unit in targetSelOutput.baseUnits)
        {
            if (unit is IHackable)
            {
                IHackable temp = (IHackable)unit;
                if (temp.Hackable())
                    temp.GetHacked(GameManager._instance.activeUnit.CurrInt);
            }
        }
        foreach (var unit in targetSelOutput.wallUnits)
        {
            if (unit is IHackable)
            {
                IHackable temp = (IHackable)unit;
                if (temp.Hackable())
                    temp.GetHacked(GameManager._instance.activeUnit.CurrInt);
            }
        }

        GameManager._instance.activeUnit.PerformAction(ActionType.hack);
        GameManager._instance.activeUnit.CurrAP -= 1;
        DeleventSystem.playerUnitUpdate();
        DeleventSystem.enemyUnitUpdate();
        DeleventSystem.mapVisualsUpdate();
    }

    public void OnLootButton()
    {
        SelectionResult targetSelInput = new SelectionResult { baseUnits = new List<BaseUnit>(), positionKeys = new List<PositionKey>(), wallUnits = new List<WallUnit>() };

        foreach (var tile in UnitManager._instance.units)
        {
            foreach (var unit in tile.Value)
            {
                if (unit is ILootable)
                {
                    ILootable temp = (ILootable)unit;
                    if (temp.Lootable() && UnitManager._instance.CheckReachType(temp.GetLootReachType(), unit.position, lastClickedTile))
                    {
                        targetSelInput.baseUnits.Add(unit);
                    }
                }
            }
        }

        foreach (var wall in UnitManager._instance.wallUnits)
        {
            if (wall.Value is ILootable)
            {
                ILootable temp = (ILootable)wall.Value;
                if (temp.Lootable() && UnitManager._instance.CheckReachType(temp.GetLootReachType(), wall.Value.position, lastClickedTile))
                {
                    targetSelInput.wallUnits.Add(wall.Value);
                }
            }
        }
        MapTargetSelector._instance.StartTargetSelection(targetSelInput, OnLootTargetCallback);
        buttonParent.SetActive(false);

    }

    public void OnLootTargetCallback(SelectionResult targetSelOutput)
    {
        GameManager._instance.AddSaveState();

        foreach (var unit in targetSelOutput.baseUnits)
        {
            if (unit is ILootable)
            {
                ILootable temp = (ILootable)unit;
                if (temp.Lootable())
                    GameManager._instance.activeUnit.inventory.Add(temp.GetLooted());
            }
        }
        foreach (var unit in targetSelOutput.wallUnits)
        {
            if (unit is ILootable)
            {
                ILootable temp = (ILootable)unit;
                if (temp.Lootable())
                    GameManager._instance.activeUnit.inventory.Add(temp.GetLooted());
            }
        }

        GameManager._instance.activeUnit.PerformAction(ActionType.take);
        GameManager._instance.activeUnit.CurrAP -= 1;
        DeleventSystem.playerUnitUpdate();
        DeleventSystem.enemyUnitUpdate();
        DeleventSystem.mapVisualsUpdate();
    }

    public void OnInventoryButton()
    {
        InventoryMenu._instance.OpenMenu();
        buttonParent.SetActive(false);
    }

    public void OnCloseButton()
    {
        InputManager._instance.SwitchInputMode(InputMode.map);
        buttonParent.SetActive(false);
    }

    bool CheckForMove()
    {
        //if(lastClickedTile != GameManager._instance.activeUnit.position && GameManager._instance.activeUnit.reachableTiles.Contains(lastClickedTile) && !UnitManager._instance.CheckTileOccupied(lastClickedTile) && GameManager._instance.activeUnit.CurrAP > 0)
        if(lastClickedTile == GameManager._instance.activeUnit.position && GameManager._instance.activeUnit.CurrAP > 0)
        {
            return true;
        }
        return false;
    }
    bool CheckForAttack()
    {
        if(lastClickedTile == GameManager._instance.activeUnit.position && UnitManager._instance.CheckAttack(lastClickedTile) && GameManager._instance.activeUnit.CurrAP > 0)
        {
            return true;
        }
        return false;
    }
    bool CheckForHack()
    {
        if (lastClickedTile == GameManager._instance.activeUnit.position && UnitManager._instance.CheckHack(lastClickedTile) && GameManager._instance.activeUnit.CurrAP > 0)
        {
            return true;
        }
        return false;
    }

    bool CheckForLoot()
    {
        if (lastClickedTile == GameManager._instance.activeUnit.position && UnitManager._instance.CheckLoot(lastClickedTile) && GameManager._instance.activeUnit.CurrAP > 0)
        {
            return true;
        }
        return false;
    }
}
