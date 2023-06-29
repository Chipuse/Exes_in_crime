using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : BaseUnit , IAttackable, ILootable
{
    public string UnitName = "Enemy";

    public int BaseHealth = 5;
    public int BaseMove = 5;
    public int BaseVision = 5;
    public int AlarmedVision = 6;
    public int BaseShoutRange = 5;
    public int BaseAttack = 1;

    public int CurrHealth = 5;
    public int CurrMove = 5;
    public int CurrVision = 5;

    public bool SeenAlly = false;
    public PositionKey AllyPosition = InvalidKey.Key;

    public EnemyState state = EnemyState.Unsuspicious;
    [HideInInspector]
    public PositionKey originPos = InvalidKey.Key;
    public List<PositionKey> route = new List<PositionKey>(); //Save by extra serialized datacontainer
    public PositionKey currentTarget = InvalidKey.Key;

    //non serialized Data
    public PlayerUnit targetUnit;
    public List<PositionKey> watchedTiles = new List<PositionKey>();
    public PositionKey nextPosition = InvalidKey.Key;
    public List<PositionKey> nextWatchedTiles = new List<PositionKey>(); //TODO including tiles between current pos and next position

    protected override void OnEnable()
    {
        base.OnEnable();
        DeleventSystem.enemyPathUpdate += UpdatePaths;
        DeleventSystem.enemyUnitUpdate += UpdateWatchedTiles;
        DeleventSystem.enemyUnitUpdate += CheckForStateChange;
        DeleventSystem.enemyUnitUpdate += CheckForAlly;
        DeleventSystem.enemyTurn += OnEnemyTurn;
        DeleventSystem.clickedOnTile += OnClickedOnTile;
        DeleventSystem.illegalAction += OnIllegalAction;
        //Check for state change
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        DeleventSystem.enemyPathUpdate -= UpdatePaths;        
        DeleventSystem.enemyUnitUpdate -= UpdateWatchedTiles;
        DeleventSystem.enemyUnitUpdate -= CheckForStateChange;
        DeleventSystem.enemyUnitUpdate -= CheckForAlly;
        DeleventSystem.enemyTurn -= OnEnemyTurn;
        DeleventSystem.clickedOnTile -= OnClickedOnTile;
        DeleventSystem.illegalAction -= OnIllegalAction;
    }

    public void OnEnemyTurn()
    {

    }

    public void OnIllegalAction(PositionKey tile)
    {
        if (watchedTiles.Contains(tile))
        {
            ChangeState(EnemyState.Alarmed);
            UpdateWatchedTiles();
            CheckForAlly();
        }
    }

    public void OnClickedOnTile(PositionKey _pos)
    {        
        if(position == _pos)
        {
            UpdatePaths();
            string status = UnitName + "\n";
            switch (state)
            {
                case EnemyState.Unsuspicious:
                    status += "Unsuspicious";
                    break;
                case EnemyState.Suspicious:
                    status += "Suspicious";
                    break;
                case EnemyState.Alarmed:
                    status += "Alarmed!";
                    break;
                case EnemyState.Dead:
                    status += "Dead\n";
                    break;
                default:
                    break;
            }
            if(state != EnemyState.Dead)
            {
                if (state == EnemyState.Unsuspicious)
                {
                    if (route.Count > 1)
                    {
                        foreach (var pos in route)
                        {
                            GameObject tempGo = EffectPooler._instance.GetPooledObject(EffectPooler._instance.tempEnemyPathEffects);
                            tempGo.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(pos), "", Color.black, 2);
                            tempGo.SetActive(true);
                        }
                        if (nextPosition != InvalidKey.Key)
                        {
                            GameObject tempGo = EffectPooler._instance.GetPooledObject(EffectPooler._instance.tempTargetEffects);
                            tempGo.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(nextPosition), "", Color.black, 2);
                            tempGo.SetActive(true);
                        }
                    }
                    else
                    {
                        foreach (var pos in Pathfinder._instance.GetPath(position, nextPosition, false))
                        {
                            GameObject tempGo = EffectPooler._instance.GetPooledObject(EffectPooler._instance.tempEnemyPathEffects);
                            tempGo.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(pos), "", Color.black, 2);
                            tempGo.SetActive(true);
                        }
                        if (nextPosition != InvalidKey.Key)
                        {
                            GameObject tempGo = EffectPooler._instance.GetPooledObject(EffectPooler._instance.tempTargetEffects);
                            tempGo.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(nextPosition), "", Color.black, 2);
                            tempGo.SetActive(true);
                        }
                    }
                }
                else if(state == EnemyState.Suspicious)
                {
                    foreach (var pos in Pathfinder._instance.GetPath(position, nextPosition, false))
                    {
                        GameObject tempGo = EffectPooler._instance.GetPooledObject(EffectPooler._instance.tempEnemyPathEffects);
                        tempGo.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(pos), "", Color.black, 2);
                        tempGo.SetActive(true);
                    }
                    if (nextPosition != InvalidKey.Key)
                    {
                        GameObject tempGo = EffectPooler._instance.GetPooledObject(EffectPooler._instance.tempTargetEffects);
                        tempGo.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(nextPosition), "", Color.black, 2);
                        tempGo.SetActive(true);
                    }
                    if (currentTarget != InvalidKey.Key)
                    {
                        GameObject tempGo = EffectPooler._instance.GetPooledObject(EffectPooler._instance.tempTargetEffects);
                        tempGo.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(currentTarget), "", Color.black, 2);
                        tempGo.SetActive(true);
                    }
                }
                else if(state == EnemyState.Alarmed)
                {
                    foreach (var pos in Pathfinder._instance.GetPath(position, nextPosition, false))
                    {
                        GameObject tempGo = EffectPooler._instance.GetPooledObject(EffectPooler._instance.tempEnemyPathEffects);
                        tempGo.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(pos), "", Color.black, 2);
                        tempGo.SetActive(true);
                    }
                    if (nextPosition != InvalidKey.Key)
                    {
                        GameObject tempGo = EffectPooler._instance.GetPooledObject(EffectPooler._instance.tempTargetEffects);
                        tempGo.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(nextPosition), "", Color.black, 2);
                        tempGo.SetActive(true);
                    }
                    if (currentTarget != InvalidKey.Key)
                    {
                        GameObject tempGo = EffectPooler._instance.GetPooledObject(EffectPooler._instance.tempTargetEffects);
                        tempGo.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(currentTarget), "", Color.black, 2);
                        tempGo.SetActive(true);
                    }
                }
            }
            else
            {
                GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.damageEffects);
                foreach (var item in inventory)
                {
                    status += item.data.Name;
                    status += "\n";
                }
                go.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(position), status, Color.black, 2);
                go.SetActive(true);
            }
        }
    }

    public void CheckForAlly()
    {
        foreach (var item in watchedTiles)
        {
            if (UnitManager._instance.units.ContainsKey(item))
            {
                foreach (var unit in UnitManager._instance.units[item])
                {
                    if(unit is PlayerUnit)
                    {
                        SeenAlly = true;
                        AllyPosition = unit.position;
                    }
                }
            }
        }
    }

    public void GetSuspicous(PositionKey susTile)
    {
        if(state == EnemyState.Unsuspicious|| state == EnemyState.Suspicious)
        {
            currentTarget = susTile;
            ChangeState(EnemyState.Suspicious);
        }
    }

    public override void MoveUnit(PositionKey pos, bool updateUnits = true)
    {
        if(state == EnemyState.Alarmed)
        {
            //try to attack player -> right now the enemyprobably would not attack the player when walking the max range
            PlayerUnit temp = null;
            foreach (var tile in watchedTiles)
            {
                if (UnitManager._instance.units.ContainsKey(tile))
                {
                    foreach (var unit in UnitManager._instance.units[tile])
                    {
                        if(unit is PlayerUnit)
                        {
                            temp = (PlayerUnit)unit;
                            if (!temp.alive)
                                temp = null;
                            break;
                        }
                    }
                }
                if (temp != null)
                    break;
            }
            if(temp != null && BaseAttack > 0)
            {
                // break move coroutine!
                GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.damageEffects);
                go.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(temp.position), "-" + BaseAttack.ToString(), Color.red, 2, 1);
                go.SetActive(true);
                Moving = false;
                StepsLeft = -1;
                temp.GetDamage(BaseAttack);
                return;
            }
        }
        base.MoveUnit(pos);
    }

    public override void SetPositionByTransform()
    {
        base.SetPositionByTransform();
        originPos = position;
    }

    public override void PerformTurn()
    {
        if(state != EnemyState.Dead)
        {
            finishedTurn = false;
            StartCoroutine(TurnBehaviour());
        }
        else
        {
            finishedTurn = true;
        }
    }

    IEnumerator TurnBehaviour()
    {
        UpdatePaths();
        switch (state)
        {
            case EnemyState.Unsuspicious:
                if(position != currentTarget && currentTarget != InvalidKey.Key)
                {
                    MoveUnit(Pathfinder._instance.GetPath(position, currentTarget, false), CurrMove);
                    while (!ContinueTurn)
                    {
                        yield return null;
                    }
                }
                break;
            case EnemyState.Suspicious:
                MoveUnit(Pathfinder._instance.GetPath(position, currentTarget, false), CurrMove);
                while (!ContinueTurn)
                {
                    yield return null;
                }
                break;
            case EnemyState.Alarmed:
                if (SeenAlly && BaseAttack > 0)
                {
                    EmittedSound.StartNoiseEventOnGo(gameObject, position, BaseShoutRange, 0.01f);
                    MoveUnit(Pathfinder._instance.GetPath(position, AllyPosition, false), CurrMove);
                    while (!ContinueTurn)
                    {
                        yield return null;
                    }
                }
                else
                {
                    if (!GameManager._instance.alarmSetOff)
                    {
                        MoveUnit(Pathfinder._instance.GetPath(position, currentTarget, false), CurrMove);
                        while (!ContinueTurn)
                        {
                            yield return null;
                        }
                        if (position == currentTarget)
                        {
                            //try to set off alarm
                            if (UnitManager._instance.units.ContainsKey(currentTarget))
                            {
                                foreach (var unit in UnitManager._instance.units[currentTarget])
                                {
                                    if (unit is AlarmUnit)
                                    {
                                        AlarmUnit temp = (AlarmUnit)unit;
                                        if (!temp.Tried && !temp.Diffused)
                                        {
                                            DeleventSystem.fireAlarm();
                                            GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.damageEffects);
                                            go.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(temp.position), "ALARM!", Color.black, 2, 1);
                                            go.SetActive(true);
                                        }
                                        else
                                        {
                                            GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.damageEffects);
                                            go.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(temp.position), "The alarm is diffused?!", Color.black, 2, 1);
                                            go.SetActive(true);
                                        }
                                        temp.Tried = true;
                                    }
                                }
                            }
                        }
                    }
                    SeenAlly = false;
                }

                break;
            case EnemyState.Dead:
                break;
            default:
                break;
        }
        //yield return new  WaitForSeconds(5.0f);
        finishedTurn = true;
    }

    void UpdateWatchedTiles()
    {
        if (state == EnemyState.Dead)
            return;
        watchedTiles = Pathfinder._instance.GeneralVisionCast(position, CurrVision);
        if(nextPosition != InvalidKey.Key && nextPosition != position)
        {
            //nextWatchedTiles = Pathfinder._instance.GeneralVisionCast(nextPosition, CurrVision);
        }
    }

    void CheckForStateChange()
    {
        if(state != EnemyState.Alarmed)
        {
            foreach (var item in watchedTiles)
            {
                switch (UnitManager._instance.CheckSusLevel(item))
                {
                    case SuspiciousLevel.Unsuspicious:
                        break;
                    case SuspiciousLevel.Alarming:
                        if(state != EnemyState.Alarmed)
                        {
                            ChangeState(EnemyState.Alarmed);
                            //Debug.LogError("Found Something Alarming!");
                            // break move coroutine!
                            Moving = false;
                            StepsLeft = -1;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {

        }
    }

    void UpdatePaths()
    {
        if (GameManager._instance.alarmSetOff && state != EnemyState.Dead)
        {
            if (state != EnemyState.Alarmed)
                ChangeState(EnemyState.Alarmed);
            if(targetUnit == null || !targetUnit.alive)
            {
                targetUnit = Pathfinder._instance.GetClosestUnit<PlayerUnit>(position, false, 30);
                if (targetUnit == null)
                    targetUnit = UnitManager._instance.GetFirstUnitOfType<PlayerUnit>();
            }
            if (targetUnit != null)
            {
                SeenAlly = true;
                AllyPosition = targetUnit.position;
            }
        }
        switch (state)
        {
            case EnemyState.Unsuspicious:
                if(route.Count > 0)
                {
                    // walk the routy route, patrouling
                    if (!route.Contains(currentTarget))
                    {
                        currentTarget = route[0];
                    }
                    if(position == currentTarget)
                    {
                        currentTarget = route[0];
                        route.RemoveAt(0);
                        route.Add(currentTarget);
                    }
                }
                else if(position != originPos)
                {
                    // walk back to idle pos
                    currentTarget = originPos;
                }
                break;
            case EnemyState.Suspicious:
                if(position == currentTarget || currentTarget == InvalidKey.Key /*|| watchedTiles.Contains(currentTarget)*/)
                {
                    ChangeState(EnemyState.Unsuspicious);
                    UpdatePaths();
                    return;
                }
                else
                {

                }
                break;
            case EnemyState.Alarmed:
                if (!GameManager._instance.alarmSetOff)
                {
                    PositionKey temp = Pathfinder._instance.GetClosestUnitPosition<AlarmUnit>(position);
                    if (temp != InvalidKey.Key)
                    {
                        currentTarget = temp;
                    }
                }
                break;
            default:
                break;
        }
        if (AllyPosition != InvalidKey.Key && AllyPosition != position && SeenAlly && BaseAttack > 0)
        {
            List<PositionKey> tempPath = Pathfinder._instance.GetPath(position, AllyPosition, false);
            if (tempPath.Count > CurrMove)
            {
                nextPosition = tempPath[CurrMove];
            }
            else
            {
                nextPosition = tempPath[tempPath.Count - 1];
            }
        }
        else if(currentTarget != InvalidKey.Key && currentTarget != position)
        {
            List<PositionKey> tempPath = Pathfinder._instance.GetPath(position, currentTarget, false);
            if(tempPath.Count > CurrMove)
            {
                nextPosition = tempPath[CurrMove];
            }
            else
            {
                nextPosition = tempPath[tempPath.Count - 1];
            }
        }
    }

    void ChangeState(EnemyState newState)
    {
        state = newState;
        playerOpen = false;
        occupying = true;
        string temp = "";
        switch (newState)
        {
            case EnemyState.Unsuspicious:
                temp = "Chill";
                susLvl = SuspiciousLevel.Unsuspicious;
                CurrVision = BaseVision;
                break;
            case EnemyState.Suspicious:
                temp = "Sus...";
                susLvl = SuspiciousLevel.Unsuspicious;
                CurrVision = BaseVision;
                break;
            case EnemyState.Alarmed:
                temp = "!\nAlarmed";                
                susLvl = SuspiciousLevel.Alarming;
                CurrVision = AlarmedVision;
                break;
            case EnemyState.Dead:
                temp = "Dead";
                susLvl = SuspiciousLevel.Alarming;
                watchedTiles = new List<PositionKey>();
                nextWatchedTiles = new List<PositionKey>();
                currentTarget = InvalidKey.Key;
                nextPosition = InvalidKey.Key;
                playerOpen = true;
                occupying = false;
                break;
            default:
                break;
        }
        GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.damageEffects);
        go.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(position), temp, Color.red, 2, 1);
        go.SetActive(true);
    }

    public bool Attackable()
    {
        if (CurrHealth > 0)
            return true;
        return false;
    }

    public void GetAttacked(int damage)
    {
        CurrHealth -= damage;
        GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.damageEffects);
        go.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(position), "-" + damage.ToString(), Color.red, 2, 1);
        go.SetActive(true);
        if (CurrHealth <= 0)
        {
            playerOpen = true;
            enemyOpen = true;
            ChangeState(EnemyState.Dead);
        }
        //Possible events?
    }

    public ReachType GetAttackReachType()
    {
        return ReachType.adjacentTiles;
    }

    public bool Lootable()
    {
        if (inventory.Count > 0 && state == EnemyState.Dead)
            return true;
        return false;
    }
    public BaseCardScript GetLooted()
    {
        BaseCardScript tempRes;
        if (inventory.Count > 0)
        {
            tempRes = inventory[0];
            inventory.RemoveAt(0);
            return tempRes;
        }

        return null;
    }
    public ReachType GetLootReachType()
    {
        return ReachType.sameAndAdjacentTiles;
    }

    public override SerializedDataContainer Serialize()
    {
        SerializedDataContainer container = base.Serialize();
        container.Serialize(UnitName);
        container.Serialize(BaseHealth);
        container.Serialize(BaseMove);
        container.Serialize(BaseVision);
        container.Serialize(AlarmedVision);
        container.Serialize(BaseShoutRange);
        container.Serialize(BaseAttack);
        container.Serialize(CurrHealth);
        container.Serialize(CurrMove);
        container.Serialize(CurrVision);
        container.Serialize(SeenAlly);
        container.Serialize(AllyPosition);
        container.Serialize((int)state);
        container.Serialize(originPos);
        SerializedDataContainer routeContainer = new SerializedDataContainer();
        routeContainer.posKeys = route;
        container.Serialize(routeContainer);
        container.Serialize(currentTarget);
        return container;
    }

    public override void Deserialize(SerializedDataContainer input)
    {
        base.Deserialize(input);
        UnitName = input.GetFirstString();
        BaseHealth = input.GetFirstInt();
        BaseMove = input.GetFirstInt();
        BaseVision = input.GetFirstInt();
        AlarmedVision = input.GetFirstInt();
        BaseShoutRange = input.GetFirstInt();
        BaseAttack = input.GetFirstInt();
        CurrHealth = input.GetFirstInt();
        CurrMove = input.GetFirstInt();
        CurrVision = input.GetFirstInt();
        SeenAlly = input.GetFirstBool();
        AllyPosition = input.GetFirstPosKey();
        state = (EnemyState)input.GetFirstInt();
        originPos = input.GetFirstPosKey();
        route = input.GetFirstMember().posKeys;
        currentTarget = input.GetFirstPosKey();
    }

    public override SerializableClasses GetSerializableType()
    {
        return SerializableClasses.enemyUnit;
    }
}

public enum EnemyState
{
    Unsuspicious,
    Suspicious,
    Alarmed,
    Dead
}
