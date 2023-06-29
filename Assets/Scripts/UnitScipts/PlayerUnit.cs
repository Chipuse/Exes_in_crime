using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : BaseUnit
{
    public string CharacterName = "sexy criminal";
    public int BaseHealth = 5;
    public int BaseMove = 5;
    public int BaseInt = 5;
    public int BaseAtk = 5;
    public int BaseCha = 5;
    public int BaseAP = 3;
    
    public int CurrHealth = 5;
    public int CurrMove = 5;
    public int CurrInt = 5;
    public int CurrAtk = 5;
    public int CurrCha = 5;
    public int CurrAP = 3;

    public bool alive = true;

    public BaseCardScript handSlot;
    public BaseCardScript bodySlot;

    //non serialized data
    public CharacterData ownData;
    public int GetDisguiseLevel()
    {
        int result = 0;
        if(bodySlot != null)
        {
            BaseBodyScript tempBody = (BaseBodyScript)bodySlot;
            result = tempBody.ModifyDisguiseLevel(this);
        }
        else
        {
            result = 0;
        }
        return result;
    }

    public List<PositionKey> reachableTiles = new List<PositionKey>();
    protected override void Start()
    {
        base.Start();
        //RefreshReachableTiles();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        DeleventSystem.clickedOnTile += ShowPath;
        DeleventSystem.clickedOnTile += OnClickedOnTile;
        DeleventSystem.playerUnitUpdate += RefreshReachableTiles;
        DeleventSystem.playerUnitUpdate += UpdateSusLvl;
        DeleventSystem.playerUnitUpdate += UpdateUnitStats;
        DeleventSystem.playerTurn += OnPlayerTurn;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        DeleventSystem.clickedOnTile -= ShowPath;
        DeleventSystem.clickedOnTile -= OnClickedOnTile;
        DeleventSystem.playerUnitUpdate -= RefreshReachableTiles;
        DeleventSystem.playerUnitUpdate -= UpdateSusLvl;
        DeleventSystem.playerUnitUpdate -= UpdateUnitStats;
        DeleventSystem.playerTurn -= OnPlayerTurn;
    }

    public void UpdateUnitStats()
    {
        CurrInt = BaseInt;
        CurrAtk = BaseAtk;
        CurrCha = BaseCha;
        CurrMove = BaseMove;
        if(handSlot != null && handSlot is BaseHandScript)
        {
            BaseHandScript tempHand = (BaseHandScript)handSlot;
            tempHand.ModifyUnitStats(this);
        }
        if (bodySlot != null && bodySlot is BaseBodyScript)
        {
            BaseBodyScript tempHand = (BaseBodyScript)bodySlot;
            tempHand.ModifyUnitStats(this);
        }
    }

    public void OnClickedOnTile(PositionKey _pos)
    {
        if (position == _pos)
        {
            //string status = CharacterName;
            //GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.damageEffects);
            //go.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(position), status + "\nHP:" + CurrHealth.ToString() + "\nAP:" + CurrAP.ToString(), Color.black, 2);
            //go.SetActive(true);
        }
    }

    void OnPlayerTurn()
    {
        CurrAP = BaseAP;
    }

    public override void RegisterAtManager(PositionKey pos)
    {
        base.RegisterAtManager(pos);
        if (!GameManager._instance.currentPlayerUnits.Contains(this))
        {
            GameManager._instance.currentPlayerUnits.Add(this);
            GameManager._instance.activeUnit = this;
        }
    }

    public void ShowPath(PositionKey target)
    {
        RefreshReachableTiles();
        if (GameManager._instance.activeUnit != this)
            return;
        Pathfinder._instance.ExampleMethod(position, CurrMove);
        if (reachableTiles.Contains(target) && target != position)
        {
            Pathfinder._instance.ExampleMethodTwo(position, target, CurrMove);
        }
    }
    public void RefreshReachableTiles()
    {
        reachableTiles = Pathfinder._instance.GeneralPathFindingCast(position, CurrMove, true);
    }

    void UpdateSusLvl()
    {
        // counting in equipment, char, illegal stuff and security level of tile
        if (MapManager._instance.GetSecurityLevel(position) > GetDisguiseLevel())
            susLvl = SuspiciousLevel.Alarming;
        else
            susLvl = SuspiciousLevel.Unsuspicious;
    }

    public void PerformAction(ActionType action)
    {
        UpdateSusLvl();
        if(bodySlot != null)
        {
            BaseBodyScript tempBody = (BaseBodyScript)bodySlot;
            if (tempBody.ModifyPerformedAction(action))
            {
                return;
            }
        }
        switch (action)
        {
            case ActionType.attack:
                DeleventSystem.illegalAction(position);
                break;
            case ActionType.hack:
                DeleventSystem.illegalAction(position);
                break;
            case ActionType.move:
                break;
            case ActionType.take:
                break;
            default:
                break;
        }
    }

    public void GetDamage(int damage)
    {
        CurrHealth -= damage;
        if(CurrHealth <= 0)
        {
            alive = false;
            GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.damageEffects);
            go.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(position), CharacterName + " has fallen", Color.black, 2, 1);
            go.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override SerializedDataContainer Serialize()
    {
        SerializedDataContainer container = base.Serialize();
        container.Serialize(CharacterName);
        container.Serialize(CurrHealth);
        container.Serialize(CurrMove);
        container.Serialize(CurrInt);
        container.Serialize(CurrAtk);
        container.Serialize(CurrCha);
        container.Serialize(CurrAP);
        container.Serialize(alive);

        if (handSlot != null)
            container.Serialize(handSlot.data.ID);
        else
            container.Serialize(-1);
        if (bodySlot != null)
            container.Serialize(bodySlot.data.ID);
        else
            container.Serialize(-1);
        return container;
    }

    public override void Deserialize(SerializedDataContainer input)
    {
        base.Deserialize(input); //check if input already loses all the used fields!!! it does
        CharacterName = input.GetFirstString();
        CurrHealth = input.GetFirstInt();
        CurrMove = input.GetFirstInt();
        CurrInt = input.GetFirstInt();
        CurrAtk = input.GetFirstInt();
        CurrCha = input.GetFirstInt();
        CurrAP = input.GetFirstInt();
        alive = input.GetFirstBool();

        int tempID = input.GetFirstInt();
        if (tempID == -1)
            handSlot = null;
        else
            handSlot = CardTester.CreateCardObj(tempID);
        tempID = input.GetFirstInt();
        if (tempID == -1)
            bodySlot = null;
        else
            bodySlot = CardTester.CreateCardObj(tempID);
        //initiate unit
    }

    public override SerializableClasses GetSerializableType()
    {
        return SerializableClasses.playerUnit;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (handSlot != null)
            Destroy(handSlot.gameObject);
        if (bodySlot != null)
            Destroy(bodySlot.gameObject);
    }
}

public enum ActionType
{
    attack, //alarms exceptions by cards
    hack, //alarms exceptions by cards
    move, //
    take //like move
}
