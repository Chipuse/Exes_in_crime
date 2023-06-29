using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMenu : MonoBehaviour
{
    public static GameStateMenu _instance;
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(this);
        }
    }

    public GameObject buttonParent;
    public bool ChangedState = false;
    bool MenuOpen = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && MenuOpen)
        {
            CloseMenu();
        }
    }

    private void LateUpdate()
    {
        ChangedState = false;
    }

    public void OpenMenu()
    {
        if (ChangedState)
            return;
        InputManager._instance.SwitchInputMode(InputMode.menu);
        transform.position = MapManager._instance.GroundGridPosToWorldPos(InputManager._instance.cursor.mouseGridPos);
        buttonParent.SetActive(true);
        ChangedState = true;
        MenuOpen = true;
    }

    public void CloseMenu()
    {
        if (ChangedState)
            return;
        InputManager._instance.SwitchInputMode(InputMode.map);
        buttonParent.SetActive(false);
        ChangedState = true;
        MenuOpen = false;
    }

    public void NextCharacter()
    {
        CloseMenu();
        GameManager._instance.NextActiveUnit();
        DeleventSystem.handVisualsUpdate();
        DeleventSystem.playerUnitUpdate();
        DeleventSystem.enemyUnitUpdate();
    }

    public void EndTurn()
    {
        CloseMenu();
        GameManager._instance.AdvanceTurn();
        DeleventSystem.handVisualsUpdate();
        DeleventSystem.playerUnitUpdate();
        DeleventSystem.enemyUnitUpdate();

    }

    public void UndoMove()
    {
        CloseMenu();
        GameManager._instance.ReturnToLatestState();
        DeleventSystem.handVisualsUpdate();
        DeleventSystem.playerUnitUpdate();
        DeleventSystem.enemyUnitUpdate();
    }
}
