using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager _instance;
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
        cursor = GameObject.Instantiate(CursorPrefab).GetComponent<Cursor>();
    }

    private void OnEnable()
    {
        DeleventSystem.newInputMode += EnterMode;
        DeleventSystem.oldInputMode += ExitMode;
        currentMode = InputMode.standby;
    }

    private void OnDisable()
    {
        DeleventSystem.newInputMode -= EnterMode;
        DeleventSystem.oldInputMode -= ExitMode;
    }

    public InputMode currentMode;
    public GameObject CursorPrefab;
    public Cursor cursor;

    public GameObject HandHighlighter;

    public void SwitchInputMode(InputMode newInputMode)
    {
        if (!GameManager._instance.GetPauseStatus())
            return;
        DeleventSystem.oldInputMode(currentMode);
        currentMode = newInputMode;
        DeleventSystem.newInputMode(newInputMode);
        DeleventSystem.handVisualsUpdate();
    }

    private void Update()
    {
        if (ConversationManager._instance != null && ConversationManager._instance.ConversationIsPlaying)
            return;
        HandHighlighter.SetActive(false);
        switch (currentMode)
        {
            case InputMode.map:
                if(Input.mousePosition.y <= Screen.height / 5)
                {
                    HandHighlighter.SetActive(true);
                    if (Input.GetMouseButtonDown(0))
                    {
                        //showhand
                        InputManager._instance.SwitchInputMode(InputMode.hand);
                    }
                }
                else
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        DeleventSystem.clickedOnTile(cursor.mouseGridPos);
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        GameStateMenu._instance.OpenMenu();
                    }
                }
                break;
            case InputMode.hand:
                if (Input.GetMouseButtonDown(1))
                {
                    //hide hand
                    InputManager._instance.SwitchInputMode(InputMode.map);
                }
                break;
            case InputMode.menu:
                break;
            case InputMode.standby:
                break;
            case InputMode.targetSelect:
                if (Input.GetMouseButtonDown(0))
                    TargetSelect._instance.MouseClick(cursor.mouseGridPos);
                break;
            case InputMode.deployUnits:
                if (Input.GetMouseButtonDown(0) && MapManager._instance.GetSecurityLevel( cursor.mouseGridPos) < 0 && !GameDataManager._instance.deployed)
                {
                    GameObject go = Instantiate(GameDataManager._instance.characterToDeploy.data.characterUnitPrefab);
                    go.transform.position = MapManager._instance.GroundGridPosToWorldPos(cursor.mouseGridPos) + Vector3.up * MapManager._instance.unitHeight;
                    GameDataManager._instance.deployed = true;
                }
                break;
            case InputMode.mapTargetSelection:
                if(!MapTargetSelector._instance.MenuOpen)
                    cursor.gameObject.SetActive(true);
                else 
                    cursor.gameObject.SetActive(false);
                if (Input.GetMouseButtonDown(0) && !MapTargetSelector._instance.MenuOpen)
                    MapTargetSelector._instance.MouseClick(cursor.mouseGridPos);
                break;
            default:
                break;
        }
    }

    void EnterMode(InputMode newInputMode)
    {
        switch (newInputMode)
        {
            case InputMode.map:
                cursor.gameObject.SetActive(true);
                break;
            case InputMode.hand:
                break;
            case InputMode.menu:
                break;
            case InputMode.standby:
                break;
            case InputMode.targetSelect:
                cursor.gameObject.SetActive(true);
                break;
            case InputMode.deployUnits:
                cursor.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    void ExitMode(InputMode oldInputMode)
    {
        cursor.gameObject.SetActive(false);
        switch (oldInputMode)
        {
            case InputMode.map:
                break;
            case InputMode.hand:
                break;
            case InputMode.menu:
                break;
            case InputMode.standby:
                break;
            case InputMode.targetSelect:
                break;
            case InputMode.deployUnits:
                break;
            default:
                break;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
public enum InputMode
{
    map, //default mode during gameplay to interact with map and playerunits and their actions
    hand, // mode to browse the players hand. accessed from map mode when clicked on the bottom part of screen where cards live. Gets exited automatically as soon as clicked outside of hand -> map mode
    menu, // mode when a menu is taking over the input - e.g. map action menu, settings menu, card browse menus
    standby, // mode when the game waits until a certain action is finished like animationmanagaer stuff
    targetSelect, // maybe solve that over menu thingy
    deployUnits, //same thing
    mapTargetSelection //deployUnits and to use when target on map should get selected
    // usw.
}