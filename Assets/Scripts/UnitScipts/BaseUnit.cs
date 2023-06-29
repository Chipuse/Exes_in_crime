using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour, ISerializableUnit
{
    public PositionKey position;
    public bool playerOpen = true;
    public bool enemyOpen = true;
    public string prefabPath = "Units/PlaceHolder/ExampleBaseUnit"; //Prefab to create the desired unit
    public bool seeThrough = true;
    public bool occupying = true;
    public SuspiciousLevel susLvl = SuspiciousLevel.Unsuspicious;

    [HideInInspector]
    public List<BaseCardScript> inventory = new List<BaseCardScript>();

    //nonserialized
    public List<int> cardsToHoldIDs;
    protected bool ContinueTurn = true;
    protected bool Moving = false;
    protected int StepsLeft = -1;
    protected List<PositionKey> MovePath = new List<PositionKey>();

    protected virtual void Start()
    {
        
    }

    protected virtual void OnEnable()
    {
        DeleventSystem.levelInit += SetPositionByTransform;
        DeleventSystem.levelInit += OnLevelInit;
    }
    protected virtual void OnDisable()
    {
        DeleventSystem.levelInit -= SetPositionByTransform;
        DeleventSystem.levelInit -= OnLevelInit;
    }

    public virtual void SetPositionByTransform()
    {
        RegisterAtManager(MapManager._instance.WorldPosToGroundGridPos(transform.position));        
    }

    void OnLevelInit()
    {
        inventory = new List<BaseCardScript>();
        foreach (var cardID in cardsToHoldIDs)
        {
            inventory.Add(CardTester.CreateCardObj(cardID));
        }
    }

    public virtual void RegisterAtManager(PositionKey pos)
    {
        DeregisterAtManager();
        if (!UnitManager._instance.units.ContainsKey(pos))
            UnitManager._instance.units.Add(pos, new List<BaseUnit>());
        UnitManager._instance.units[pos].Add(this);
        position = pos;
    }

    public virtual void MoveUnit(List<PositionKey> path, int maxRange = 100, bool updateUnits = true)
    {
        MovePath = path;
        StepsLeft = maxRange;
        StartCoroutine(MoveOnPath(updateUnits));
    }

    IEnumerator MoveOnPath(bool updateUnits = true)
    {
        ContinueTurn = false;
        if(MovePath.Count >= 1)
            CameraMover._instance.MoveCamera(MovePath[MovePath.Count-1]);
        while (MovePath.Count > 0 && StepsLeft >= 0)
        {
            MoveUnit(MovePath[0], updateUnits);
            while (Moving)
            {
                yield return null;
            }
            MovePath.RemoveAt(0);
            StepsLeft--;
        }
        ContinueTurn = true;
    }

    public virtual void MoveUnit(PositionKey pos, bool updateUnits = true)
    {
        if (updateUnits)
        {
            AnimationManager._instance.StartWaitTranslate(transform, new Vector3(
    MapManager._instance.GroundGridPosToWorldPos(pos).x,
    MapManager._instance.unitHeight,
    MapManager._instance.GroundGridPosToWorldPos(pos).z
    ), 5, OnTranslateEventUnitUpdate);
        }
        else
        {
            AnimationManager._instance.StartWaitTranslate(transform, new Vector3(
    MapManager._instance.GroundGridPosToWorldPos(pos).x,
    MapManager._instance.unitHeight,
    MapManager._instance.GroundGridPosToWorldPos(pos).z
    ), 5, OnTranslateEvent);
        }
        Moving = true;
    }
    public virtual void OnTranslateEvent()
    {
        RegisterAtManager(MapManager._instance.WorldPosToGroundGridPos(transform.position));
        transform.position = new Vector3(
            MapManager._instance.GroundGridPosToWorldPos(position).x,
            MapManager._instance.unitHeight,
            MapManager._instance.GroundGridPosToWorldPos(position).z
            );
        Moving = false;
    }

    public virtual void OnTranslateEventUnitUpdate()
    {
        RegisterAtManager(MapManager._instance.WorldPosToGroundGridPos(transform.position));
        transform.position = new Vector3(
            MapManager._instance.GroundGridPosToWorldPos(position).x,
            MapManager._instance.unitHeight,
            MapManager._instance.GroundGridPosToWorldPos(position).z
            );
        Moving = false;
        DeleventSystem.playerUnitUpdate();
        DeleventSystem.enemyUnitUpdate();
        DeleventSystem.mapVisualsUpdate();
    }

    void DeregisterAtManager()
    {
        if (!UnitManager._instance.units.ContainsKey(position))
            return;
        if (UnitManager._instance.units[position].Contains(this))
        {
            UnitManager._instance.units[position].Remove(this);
        }
    }
    [HideInInspector]
    public bool finishedTurn = false; //does not need to be serialized
    public virtual void PerformTurn()
    {
        finishedTurn = true;
    }

    public virtual SerializedDataContainer Serialize()
    {
        SerializedDataContainer container = new SerializedDataContainer();
        container.prefabPath = prefabPath;
        container.type = GetSerializableType();
        container.Serialize(position);
        container.Serialize(playerOpen);
        container.Serialize(enemyOpen);
        container.Serialize(seeThrough);
        container.Serialize(occupying);
        container.Serialize((int)susLvl);

        SerializedDataContainer inventoryContainer = new SerializedDataContainer();
        foreach (var item in inventory)
        {
            inventoryContainer.Serialize(item.data.ID);
        }
        container.Serialize(inventoryContainer);

        return container;
    }

    public virtual void Deserialize(SerializedDataContainer input)
    {
        prefabPath = input.prefabPath;
        position = input.GetFirstPosKey();
        playerOpen = input.GetFirstBool();
        enemyOpen = input.GetFirstBool();
        seeThrough = input.GetFirstBool();
        occupying = input.GetFirstBool();
        susLvl = (SuspiciousLevel)input.GetFirstInt();

        inventory = new List<BaseCardScript>();
        foreach (var cardID in input.GetFirstMember().ints)
        {
            inventory.Add(CardTester.CreateCardObj(cardID));
        }

        //initiate unit
        RegisterAtManager(position);
        transform.position = new Vector3(
            MapManager._instance.GroundGridPosToWorldPos(position).x, 
            MapManager._instance.unitHeight,
            MapManager._instance.GroundGridPosToWorldPos(position).z
            );
    }

    public virtual SerializableClasses GetSerializableType()
    {
        return SerializableClasses.baseUnit;
    }

    protected virtual void OnDestroy()
    {
        foreach (var item in inventory)
        {
            if(item != null && item.gameObject != null)
                Destroy(item.gameObject);
        }
    }
}

public enum SuspiciousLevel
{
    Unsuspicious,
    Suspicious,
    Alarming
}
