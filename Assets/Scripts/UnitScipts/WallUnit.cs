using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WallUnit : MonoBehaviour, ISerializableUnit, IAttackable, IHackable
{
    public PositionKey position;
    string prefabPath = "Units/PlaceHolder/WallUnit"; //Prefab to create the desired unit

    public bool playerOpen = true;
    public bool enemyOpen = true;

    public int baseHackHealth = 5;
    public int baseAttackHealth = 5;
    public int currHackHealth = 5;
    public int currAttackHealth = 5;
    public SuspiciousLevel susLvl = SuspiciousLevel.Unsuspicious;
    public string description = "Door";

    // non serialized data
    public TMP_Text textDisplay;

    protected virtual void Start()
    {

    }

    protected virtual void OnEnable()
    {
        DeleventSystem.levelInit += SetPositionByTransform;
    }
    protected virtual void OnDisable()
    {
        DeleventSystem.levelInit -= SetPositionByTransform;
    }

    private void LateUpdate()
    {
        //transform.forward = Camera.main.transform.forward;
        string tempText = description + ": ";
        if (playerOpen)
        {
            tempText += "Unlocked";
        }
        else
        {
            tempText += "Locked";
            if (currHackHealth > 0)
                tempText += "\nHack: " + currHackHealth;
            if (currAttackHealth > 0)
                tempText += "\nAttack: " + currAttackHealth;
        }
        textDisplay.text = tempText;
    }
    public void SetPositionByTransform()
    {
        RegisterAtManager(MapManager._instance.WorldPosToWallGridPos(transform.position));
    }
    public void RegisterAtManager(PositionKey pos)
    {
        DeregisterAtManager();
        if (!UnitManager._instance.wallUnits.ContainsKey(pos))
            UnitManager._instance.wallUnits.Add(pos, this);
        else
        {
            Debug.LogError("double Dooring lol");
            Destroy(this);
        }
        position = pos;
        transform.position = new Vector3(
            MapManager._instance.WallGridPosToWorldPos(position).x,
            MapManager._instance.wallHeight,
            MapManager._instance.WallGridPosToWorldPos(position).z);
    }

    void DeregisterAtManager()
    {
        if (!UnitManager._instance.wallUnits.ContainsKey(position))
            return;
        UnitManager._instance.wallUnits.Remove(position);
    }

    public bool Attackable()
    {
        if(currAttackHealth > 0  && !playerOpen)
            return true;
        return false;
    }

    public void GetAttacked(int damage)
    {
        currAttackHealth -= damage;
        GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.damageEffects);
        go.GetComponent<DamageEffect>().StartEffect(MapManager._instance.WallGridPosToWorldPos(position), "-" + damage.ToString(), Color.red, 2, 1);
        go.SetActive(true);
        if (currAttackHealth <= 0)
        {
            playerOpen = true;
            enemyOpen = true;
            susLvl = SuspiciousLevel.Alarming;
        }
        //Possible events?
    }

    public ReachType GetAttackReachType()
    {
        return ReachType.wall;
    }

    public bool Hackable()
    {
        if (currHackHealth > 0 && !playerOpen)
            return true;
        return false;
    }

    public void GetHacked(int damage)
    {
        currHackHealth -= damage;
        GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.damageEffects);
        go.GetComponent<DamageEffect>().StartEffect(MapManager._instance.WallGridPosToWorldPos(position), "-" + damage.ToString(), Color.green, 2, 1);
        go.SetActive(true);
        if (currHackHealth <= 0)
        {
            playerOpen = true;
        }
        //Possible events?
    }

    public ReachType GetHackReachType()
    {
        return ReachType.wall;
    }

    public virtual SerializedDataContainer Serialize()
    {
        SerializedDataContainer container = new SerializedDataContainer();
        container.prefabPath = prefabPath;
        container.type = GetSerializableType();
        container.Serialize(position);
        container.Serialize(playerOpen);
        container.Serialize(enemyOpen);
        container.Serialize(baseHackHealth);
        container.Serialize(baseAttackHealth);
        container.Serialize(currHackHealth);
        container.Serialize(currAttackHealth);
        container.Serialize((int)susLvl);
        container.Serialize(description);
        return container;
    }

    public virtual void Deserialize(SerializedDataContainer input)
    {
        prefabPath = input.prefabPath;
        position = input.GetFirstPosKey();
        playerOpen = input.GetFirstBool();
        enemyOpen = input.GetFirstBool();
        baseHackHealth = input.GetFirstInt();
        baseAttackHealth = input.GetFirstInt();
        currHackHealth = input.GetFirstInt();
        currAttackHealth = input.GetFirstInt();
        susLvl = (SuspiciousLevel)input.GetFirstInt();
        description = input.GetFirstString();
        //initiate unit
        RegisterAtManager(position);
    }

    public virtual SerializableClasses GetSerializableType()
    {
        return SerializableClasses.wallUnit;
    }
}
