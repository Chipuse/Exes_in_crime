using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmUnit : BaseUnit, IHackable
{
    //Serialized Data
    public int BaseHackHealth = 10;
    public int CurrHackHealth = 10;
    public bool Diffused = false;
    public bool Tried = false;
    //[HideInInspector]
    public List<PositionKey> connectedCameraPos = new List<PositionKey>();
    //NonSerialized Data
    public List<CameraUnit> connectedCameras = new List<CameraUnit>();

    protected override void OnEnable()
    {
        base.OnEnable();
        DeleventSystem.levelInit += OnLevelInit;
        DeleventSystem.clickedOnTile += OnClickedOnTile;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        DeleventSystem.clickedOnTile -= OnClickedOnTile;
        DeleventSystem.levelInit -= OnLevelInit;
    }

    void OnLevelInit()
    {
        foreach (var item in connectedCameras)
        {
            PositionKey temp = InvalidKey.Key;
            temp = MapManager._instance.WorldPosToGroundGridPos(item.transform.position);
            if(temp != InvalidKey.Key)
                connectedCameraPos.Add(temp);
        }
    }

    public void OnClickedOnTile(PositionKey _pos)
    {
        if (position == _pos)
        {
            string status = "Alarm";
            GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.damageEffects);
            go.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(position), status + "\nHack: " + CurrHackHealth.ToString(), Color.black, 2);
            go.SetActive(true);
            foreach (var pos in connectedCameraPos)
            {
                GameObject tempGo = EffectPooler._instance.GetPooledObject(EffectPooler._instance.tempTargetEffects);
                tempGo.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(pos), "", Color.black, 2);
                tempGo.SetActive(true);
            }
        }
        
    }

    public bool Hackable()
    {
        if (CurrHackHealth > 0)
            return true;
        return false;
    }

    public void GetHacked(int damage)
    {
        CurrHackHealth -= damage;
        GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.damageEffects);
        go.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(position), "-" + damage.ToString(), Color.green, 2, 1);
        go.SetActive(true);
        if (CurrHackHealth <= 0)
        {
            Diffused = true;
            foreach (var pos in connectedCameraPos)
            {
                if (UnitManager._instance.units.ContainsKey(pos))
                {
                    foreach (var unit in UnitManager._instance.units[pos])
                    {
                        if(unit is CameraUnit)
                        {
                            CameraUnit temp = (CameraUnit)unit;
                            temp.Deactivate();
                        }
                    }
                }
            }
        }
        //Possible events?
    }

    public ReachType GetHackReachType()
    {
        return ReachType.sameTile;
    }

    public override SerializedDataContainer Serialize()
    {
        SerializedDataContainer container = base.Serialize();
        container.Serialize(BaseHackHealth);
        container.Serialize(CurrHackHealth);
        container.Serialize(Diffused);
        container.Serialize(Tried);
        SerializedDataContainer connectedCameras = new SerializedDataContainer();
        connectedCameras.posKeys = connectedCameraPos;
        container.Serialize(connectedCameras);
        return container;
    }

    public override void Deserialize(SerializedDataContainer input)
    {
        base.Deserialize(input);
        BaseHackHealth = input.GetFirstInt();
        CurrHackHealth = input.GetFirstInt();
        Diffused = input.GetFirstBool();
        Tried = input.GetFirstBool();
        connectedCameraPos = input.GetFirstMember().posKeys;
    }

    public override SerializableClasses GetSerializableType()
    {
        return SerializableClasses.baseUnit;
    }
}
