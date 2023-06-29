using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapTargetSelector : MonoBehaviour
{
    public static MapTargetSelector _instance;
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

    bool PerformTargetSelection = false;

    SelectionResult ValidTargets = new SelectionResult { positionKeys = new List<PositionKey>(), baseUnits = new List<BaseUnit>(), wallUnits = new List<WallUnit>() };
    List<PositionKey> ValidPositions = new List<PositionKey>();
    SelectionResult ChosenTargets = new SelectionResult { positionKeys = new List<PositionKey>(), baseUnits = new List<BaseUnit>(), wallUnits = new List<WallUnit>() };
    public delegate void TargetEvent(SelectionResult targets);
    public TargetEvent targetEventResult;
    int targetNumber = 0;
    bool cancelAble = false;
    InputMode bufferedInputMode = InputMode.map;


    Dictionary<int, buttonSelectionMap> menuButtonMap;

    public bool MenuOpen = false;
    SelectionResult menuSelection = new SelectionResult { positionKeys = new List<PositionKey>(), baseUnits = new List<BaseUnit>(), wallUnits = new List<WallUnit>() };
    public GameObject buttonPrefab;
    public GameObject buttonParent;
    List<MapMenuButton> buttons = new List<MapMenuButton>();

    List<GameObject> ValidTargetsEffects = new List<GameObject>();
    List<GameObject> ChosenTargetsEffects = new List<GameObject>();

    public void StartTargetSelection(SelectionResult _validTargets, TargetEvent _callBack, int _targetNumber = 1, bool _cancelAble = false, InputMode _bufferedInputMode = InputMode.map)
    {
        InputManager._instance.SwitchInputMode(InputMode.mapTargetSelection);
        bufferedInputMode = _bufferedInputMode;
        targetEventResult = _callBack;
        ValidTargets = _validTargets;
        ChosenTargets = new SelectionResult { positionKeys = new List<PositionKey>(), baseUnits = new List<BaseUnit>(), wallUnits = new List<WallUnit>() };
        targetNumber = _targetNumber;
        cancelAble = _cancelAble;
        // hide show cancel button
        //start targeting routine
        PerformTargetSelection = true;
        ValidPositions = GetPositionsFromSelection(ValidTargets);
        UpdateVisualisation();
    }

    private void Update()
    {
        if (PerformTargetSelection)
        {
            if (!MenuOpen)
            {
                
            }
        }
    }

    public void MouseClick(PositionKey _pos) 
    {
        if (!MenuOpen)
        {
            menuSelection = GetAllFromPosition(_pos);
            if(menuSelection.baseUnits.Count != 0 || menuSelection.wallUnits.Count != 0 || menuSelection.positionKeys.Count != 0)
                OpenMenu();
        }
    }

    SelectionResult GetAllFromPosition(PositionKey _pos)
    {
        SelectionResult result = new SelectionResult { positionKeys = new List<PositionKey>(), baseUnits = new List<BaseUnit>(), wallUnits = new List<WallUnit>()};
        foreach (var item in ValidTargets.positionKeys)
        {
            if(item == _pos)
            {
                result.positionKeys.Add(item);
            }
        }

        foreach (var item in ValidTargets.baseUnits)
        {
            if(item.position == _pos)
            {
                result.baseUnits.Add(item);
            }
        }

        foreach (var item in ValidTargets.wallUnits)
        {
            foreach (var wallPos in MapManager._instance.GetAllTilesAroundWall(item.position))
            {
                if (wallPos == _pos)
                    result.wallUnits.Add(item);
            }
        }
        return result;
    }

    private void OpenMenu()
    {
        transform.position = MapManager._instance.GroundGridPosToWorldPos(InputManager._instance.cursor.mouseGridPos);
        buttonParent.SetActive(true);
        menuButtonMap = new Dictionary<int, buttonSelectionMap>();
        int idCounter = 0;
        for (int i = 0; i < menuSelection.positionKeys.Count; i++)
        {
            menuButtonMap.Add(idCounter, new buttonSelectionMap { type = MapTargetType.position, index = i });
            idCounter++;
        }

        for (int i = 0; i < menuSelection.baseUnits.Count; i++)
        {
            menuButtonMap.Add(idCounter, new buttonSelectionMap { type = MapTargetType.baseUnit, index = i });
            idCounter++;
        }

        for (int i = 0; i < menuSelection.wallUnits.Count; i++)
        {
            menuButtonMap.Add(idCounter, new buttonSelectionMap { type = MapTargetType.wallUnit, index = i });
            idCounter++;
        }

        foreach (var item in menuButtonMap)
        {
            CreateButton(item.Key);
        }
        MenuOpen = true;
        ValidPositions = GetPositionsFromSelection(ValidTargets);
        UpdateVisualisation();
    }

    private void CreateButton(int _id)
    {
        GameObject go = Instantiate<GameObject>(buttonPrefab, buttonParent.transform, false);
        go.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 200 * _id);
        go.GetComponent<MapMenuButton>().SetUpButton(_id, OnButtonCallback);
        TMP_Text buttonText = go.transform.GetChild(0).GetComponent<TMP_Text>();
        switch (menuButtonMap[_id].type)
        {
            case MapTargetType.position:
                buttonText.text = "Pos: " + menuSelection.positionKeys[menuButtonMap[_id].index].x + ":" + menuSelection.positionKeys[menuButtonMap[_id].index].y;
                break;
            case MapTargetType.baseUnit:
                buttonText.text = menuSelection.baseUnits[menuButtonMap[_id].index].prefabPath;
                break;
            case MapTargetType.wallUnit:
                buttonText.text = menuSelection.wallUnits[menuButtonMap[_id].index].description;
                break;
            default:
                break;
        }
    }

    private void CloseMenu()
    {
        foreach (Transform child in buttonParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        buttonParent.SetActive(false);
        MenuOpen = false;
        ValidPositions = GetPositionsFromSelection(ValidTargets);
        UpdateVisualisation();
    }

    private void OnButtonCallback(int _buttonId)
    {        
        if (menuButtonMap.ContainsKey(_buttonId))
        {
            switch (menuButtonMap[_buttonId].type)
            {
                case MapTargetType.position:
                    ChosenTargets.positionKeys.Add(menuSelection.positionKeys[menuButtonMap[_buttonId].index]);
                    //ValidTargets.positionKeys.RemoveAt(menuButtonMap[_buttonId].index); // this does not work yet!!!
                    break;
                case MapTargetType.baseUnit:
                    ChosenTargets.baseUnits.Add(menuSelection.baseUnits[menuButtonMap[_buttonId].index]);
                    //ValidTargets.baseUnits.RemoveAt(menuButtonMap[_buttonId].index);
                    break;
                case MapTargetType.wallUnit:
                    ChosenTargets.wallUnits.Add(menuSelection.wallUnits[menuButtonMap[_buttonId].index]);
                    //ValidTargets.wallUnits.RemoveAt(menuButtonMap[_buttonId].index);
                    break;
                default:
                    break;
            }
            targetNumber--;
            if(targetNumber <= 0)
            {
                TargetSelectionEnd();
            }
        }
        CloseMenu();
    }

    public void TargetSelectionEnd()
    {
        PerformTargetSelection = false;
        InputManager._instance.SwitchInputMode(bufferedInputMode);
        targetEventResult(ChosenTargets);
    }

    public void CancelTargetSelection()
    {
        PerformTargetSelection = false;
        //a cancelled or failed target selection return a selectionresult with no entries. the callback funstions should consider this case and cleanly go back to previous state... or at least dont break the game if possible
        targetEventResult(new SelectionResult { positionKeys = new List<PositionKey>(), baseUnits = new List<BaseUnit>(), wallUnits = new List<WallUnit>()});
        InputManager._instance.SwitchInputMode(bufferedInputMode);
    }

    public bool CheckMultiTarget(int _validTargetsCount, int _wantedTargets)
    {
        if(_wantedTargets > _validTargetsCount)
        {
            return false;
        }
        return true;
    }

    public void UpdateVisualisation()
    {
        foreach (var item in ValidTargetsEffects)
        {
            item.SetActive(false);
        }
        ValidTargetsEffects = new List<GameObject>();
        foreach (var item in ChosenTargetsEffects)
        {
            item.SetActive(false);
        }
        ChosenTargetsEffects = new List<GameObject>();
        if (PerformTargetSelection)
        {
            foreach (var item in ValidPositions)
            {
                GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.validTargetEffects);
                go.SetActive(true);
                go.transform.SetPositionAndRotation(MapManager._instance.GroundGridPosToWorldPos(item), go.transform.rotation);
                go.transform.Translate(Vector3.up * 0.53f, Space.World);
                ValidTargetsEffects.Add(go);
            }
        }
    }

    List<PositionKey> GetPositionsFromSelection(SelectionResult _input)
    {
        List<PositionKey> result = new List<PositionKey>();
        foreach (var item in _input.positionKeys)
        {
            if (!result.Contains(item))
            {
                result.Add(item);
            }
        }

        foreach (var item in _input.baseUnits)
        {
            if (!result.Contains(item.position))
            {
                result.Add(item.position);
            }
        }

        foreach (var item in ValidTargets.wallUnits)
        {
            foreach (var wallPos in MapManager._instance.GetAllTilesAroundWall(item.position))
            {
                if (!result.Contains(wallPos))
                    result.Add(wallPos);
            }
        }
        return result;
    }
}

public struct SelectionResult
{
    public List<PositionKey> positionKeys;
    public List<BaseUnit> baseUnits;
    public List<WallUnit> wallUnits;
}

public struct buttonSelectionMap
{
    public MapTargetType type;
    public int index;
}

public enum MapTargetType
{
    position,
    baseUnit,
    wallUnit
}
