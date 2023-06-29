using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
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

    //Data to serialize
    public int turn = 0;
    public bool alarmSetOff = false;

    //non serialized data
    public GameState gameState = GameState.playerTurn;
    public List<PlayerUnit> currentPlayerUnits;
    public PlayerUnit activeUnit;

    public static bool instancedRendering = true;

    private void OnEnable()
    {
        DeleventSystem.enemyTurn += OnEnemyTurn;
        DeleventSystem.allyTurn += OnAllyTurn;
        DeleventSystem.playerTurn += OnplayerTurn;
        DeleventSystem.preDeserialization += OnPreDeserialization;
        DeleventSystem.postDeserialization += OnPostDeserialization;
        DeleventSystem.fireAlarm += OnAlarmSetOff;
        //DeleventSystem.levelInit += InitGameManager;
    }

    private void OnDisable()
    {
        DeleventSystem.enemyTurn -= OnEnemyTurn;
        DeleventSystem.allyTurn -= OnAllyTurn;
        DeleventSystem.playerTurn -= OnplayerTurn;
        DeleventSystem.preDeserialization -= OnPreDeserialization;
        DeleventSystem.postDeserialization -= OnPostDeserialization;
        DeleventSystem.fireAlarm -= OnAlarmSetOff;
        //DeleventSystem.levelInit -= InitGameManager;
    }

    void OnAlarmSetOff()
    {
        alarmSetOff = true;
    }

    void InitGameManager()
    {
        turn = 0;
        alarmSetOff = false;
        gameState = GameState.playerTurn;
        currentPlayerUnits = new List<PlayerUnit>();
    }
    public void NextActiveUnit()
    {
        bool someAlive = false;
        foreach (var unit in currentPlayerUnits)
        {
            if (unit.alive)
            {
                someAlive = true;
                break;
            }
        }
        if (!someAlive)
            return;
        if (currentPlayerUnits.Count == 0)
            return;
        int nextIndex = 0;
        for (int i = 0; i < currentPlayerUnits.Count; i++)
        {
            if(currentPlayerUnits[i] == activeUnit)
            {
                nextIndex = i + 1; 
            }
        }
        if(nextIndex >= currentPlayerUnits.Count)
        {
            nextIndex = 0;
        }
        activeUnit = currentPlayerUnits[nextIndex];
        if (!activeUnit.alive)
            NextActiveUnit();
        CameraMover._instance.MoveCamera(activeUnit.position);
        DeleventSystem.handVisualsUpdate();
    }

    bool Continue = true;
    public bool GetPauseStatus()
    {
        return Continue;
    }
    public void UnpauseGame()
    {
        Continue = true;
        InputManager._instance.SwitchInputMode(InputMode.map);
    }

    public void PauseGame()
    {
        InputManager._instance.SwitchInputMode(InputMode.standby);
        Continue = false;
    }

    public List<SerializedDataContainer> lastGameStates = new List<SerializedDataContainer>();
    public void AddSaveState()
    {
        lastGameStates.Add(Serialize());
    }

    public void ReturnToLatestState()
    {
        if(lastGameStates.Count > 0 && InputManager._instance.currentMode == InputMode.map)
        {
            DeleventSystem.preDeserialization();
            Deserialize(lastGameStates[lastGameStates.Count-1]); //lastdata gets practically destroyed when deserializing because of my (stupid) approach
            lastGameStates.RemoveAt(lastGameStates.Count - 1);
            DeleventSystem.postDeserialization();
            DeleventSystem.handVisualsUpdate();
        }
    }

    public void ClearSaveStates()
    {
        lastGameStates = new List<SerializedDataContainer>();
    }

    void Update()
    {
        switch (gameState)
        {
            case GameState.playerTurn:
                break;
            case GameState.enemyTurn:
                break;
            case GameState.allyTurn:
                break;
            default:
                break;
        }
        if(InputManager._instance.currentMode == InputMode.hand)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (InputManager._instance.currentMode == InputMode.hand)
                {
                    InputManager._instance.SwitchInputMode(InputMode.map);
                }
                else
                {
                    InputManager._instance.SwitchInputMode(InputMode.hand);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
            instancedRendering = !instancedRendering;

        if (InputManager._instance.currentMode != InputMode.map)
            return;
        if (Input.GetKeyDown(KeyCode.B))
            NextActiveUnit();
        if (Input.GetKeyDown(KeyCode.S))
            AddSaveState();
        if (Input.GetKeyDown(KeyCode.L))
        {
            InitGameManager();
            ReturnToLatestState();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartLevel();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            AdvanceTurn();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            CardVisHand._instance.UpdateHandCards();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            if(InputManager._instance.currentMode == InputMode.hand)
            {
                InputManager._instance.SwitchInputMode(InputMode.map);
            }
            else
            {
                InputManager._instance.SwitchInputMode(InputMode.hand);
            }
        }

    }

    public void StartLevel()
    {
        InitGameManager();
        DeleventSystem.levelInit();
        if(DeleventSystem.lateLevelInit != null)
            DeleventSystem.lateLevelInit();
        DeleventSystem.playerUnitUpdate();
        if(DeleventSystem.enemyUnitUpdate != null)
            DeleventSystem.enemyUnitUpdate();
        if(DeleventSystem.enemyPathUpdate != null)
            DeleventSystem.enemyPathUpdate();
        DeleventSystem.mapVisualsUpdate();
    }

    void OnPreDeserialization()
    {
        currentPlayerUnits = new List<PlayerUnit>();
    }

    void OnPostDeserialization()
    {
        DeleventSystem.playerUnitUpdate();
        DeleventSystem.enemyUnitUpdate();
        DeleventSystem.enemyPathUpdate();
        DeleventSystem.mapVisualsUpdate();
    }

    void OnplayerTurn()
    {
        bool someAlive = false;
        foreach (var unit in currentPlayerUnits)
        {
            if (unit.alive)
            {
                someAlive = true;
                break;
            }
        }
        if (!someAlive)
        {
            GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.damageEffects);
            go.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos( activeUnit.position), "Game Over", Color.black, 10);
            go.SetActive(true);
        }
        CardManager._instance.DrawTopCard();
        NextActiveUnit();
    }

    void OnEnemyTurn()
    {
        foreach (var tile in UnitManager._instance.units)
        {
            for (int i = 0; i < tile.Value.Count; i++)
            {
                BaseUnit unit = tile.Value[i];
                if (unit is EnemyUnit)
                {
                    unit.finishedTurn = false;
                }
            }
        }
    }

    void OnAllyTurn()
    {
        foreach (var tile in UnitManager._instance.units)
        {
            for (int i = 0; i < tile.Value.Count; i++)
            {
                BaseUnit unit = tile.Value[i];
                if (unit is BaseUnit)
                {
                    if (unit is BasicCompanionUnit)
                    {
                        unit.finishedTurn = false;
                    }
                }
            }
        }
    }

    public void AdvanceTurn()
    {
        switch (gameState)
        {
            case GameState.playerTurn:
                ClearSaveStates();
                gameState = GameState.allyTurn;
                DeleventSystem.allyTurn(); ;
                StartCoroutine(AllyTurnRoutine());
                break;
            case GameState.allyTurn:
                gameState = GameState.enemyTurn;
                DeleventSystem.enemyTurn();
                StartCoroutine(EnemyTurnRoutine());
                break;
            case GameState.enemyTurn:
                DeleventSystem.playerTurn();
                gameState = GameState.playerTurn;
                break;
            default:
                break;
        }
        turn++;
    }

    IEnumerator EnemyTurnRoutine()
    {
        bool NoUnitLeft = false;
        bool UnitFound;

        //only check for alarmed units
        while (!NoUnitLeft)
        {
            UnitFound = false;
            foreach (var tile in UnitManager._instance.units)
            {
                for (int i = 0; i < tile.Value.Count; i++)
                {
                    BaseUnit unit = tile.Value[i];
                    if (unit is EnemyUnit && !unit.finishedTurn)
                    {
                        EnemyUnit tempUnit = (EnemyUnit)unit;
                        if(tempUnit.state == EnemyState.Alarmed)
                        {
                            if (tempUnit.state == EnemyState.Dead || (tempUnit.state == EnemyState.Unsuspicious && tempUnit.route.Count <= 0) || (alarmSetOff && tempUnit.BaseAttack <= 0))
                            {

                            }
                            else
                            {
                                CameraMover._instance.MoveCamera(unit.position);
                                while (CameraMover._instance.auto)
                                {
                                    yield return null;
                                }
                            }
                            UnitFound = true;
                            unit.PerformTurn();
                            while (!unit.finishedTurn)
                            {
                                yield return null;
                            }
                        }
                    }
                    if (UnitFound)
                        break;
                }
                if (UnitFound)
                    break;
            }
            if (!UnitFound)
                break;
        }
        NoUnitLeft = false;
        //all other units
        while (!NoUnitLeft)
        {
            UnitFound = false;
            foreach (var tile in UnitManager._instance.units)
            {
                for (int i = 0; i < tile.Value.Count; i++)
                {
                    BaseUnit unit = tile.Value[i];
                    if (unit is EnemyUnit && !unit.finishedTurn)
                    {
                        EnemyUnit tempUnit = (EnemyUnit)unit;
                        if (tempUnit.state == EnemyState.Dead || (tempUnit.state == EnemyState.Unsuspicious && tempUnit.route.Count <= 0) || (alarmSetOff && tempUnit.BaseAttack <= 0))
                        {

                        }
                        else
                        {
                            CameraMover._instance.MoveCamera(unit.position);
                            while (CameraMover._instance.auto)
                            {
                                yield return null;
                            }
                        }
                        UnitFound = true;
                        unit.PerformTurn();
                        while (!unit.finishedTurn)
                        {
                            yield return null;
                        }

                    }
                    if (UnitFound)
                        break;
                }
                if (UnitFound)
                    break;
            }
            if (!UnitFound)
                break;
        }
        AdvanceTurn();
    }
    IEnumerator AllyTurnRoutine()
    {
        bool NoUnitLeft = true; //right now ally units are not consiedered
        bool UnitFound = false;
        while (!NoUnitLeft)
        {
            UnitFound = false;
            foreach (var tile in UnitManager._instance.units)
            {
                for (int i = 0; i < tile.Value.Count; i++)
                {
                    BaseUnit unit = tile.Value[i];
                    if (unit is BasicCompanionUnit && !unit.finishedTurn)
                    {
                        UnitFound = true;
                        CameraMover._instance.MoveCamera(unit.position);
                        while (CameraMover._instance.auto)
                        {
                            yield return null;
                        }
                        unit.PerformTurn();
                        while (!unit.finishedTurn)
                        {
                            yield return null;
                        }
                    }
                    if (UnitFound)
                        break;
                }
                if (UnitFound)
                    break;
            }
            if (!UnitFound)
                break;
        }
        CameraMover._instance.MoveCamera(activeUnit.position);
        while (CameraMover._instance.auto)
        {
            yield return null;
        }
        AdvanceTurn();
    }

    public SerializedDataContainer Serialize()
    {
        SerializedDataContainer result = new SerializedDataContainer();
        result.Serialize(turn);
        result.Serialize(alarmSetOff);
        result.Serialize(UnitManager._instance.Serialize());
        result.Serialize(CardManager._instance.Serialize());
        return result;
    }

    public void Deserialize(SerializedDataContainer input)
    {
        turn = input.GetFirstInt();
        alarmSetOff = input.GetFirstBool();
        UnitManager._instance.Deserialize(input.GetFirstMember()); //lastdata gets practically destroyed when deserializing because of my (stupid) approach
        CardManager._instance.Deserialize(input.GetFirstMember()); //lastdata gets practically destroyed when deserializing because of my (stupid) approach
    }
}

public enum GameState
{
    playerTurn,
    enemyTurn,
    allyTurn,
    preperation
}
