using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUnit : BaseUnit, IAttackable
{
    public int BaseHealth = 5;
    public int BaseVision = 5;
    public int CurrHealth = 5;
    [HideInInspector]
    public bool Active = true;


    //non serialized data

    public List<PositionKey> watchedTiles = new List<PositionKey>();

    protected override void OnEnable()
    {
        base.OnEnable();
        DeleventSystem.clickedOnTile += OnClickedOnTile;
        DeleventSystem.enemyUnitUpdate += UpdateWatchedTiles;
        DeleventSystem.enemyUnitUpdate += CheckForStateChange;
        DeleventSystem.fireAlarm += Deactivate;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        DeleventSystem.clickedOnTile -= OnClickedOnTile;
        DeleventSystem.enemyUnitUpdate -= UpdateWatchedTiles;
        DeleventSystem.enemyUnitUpdate -= CheckForStateChange;
        DeleventSystem.fireAlarm -= Deactivate;
    }

    public void OnClickedOnTile(PositionKey _pos)
    {
        if (position == _pos)
        {
            string status = "Camera";
            GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.damageEffects);
            go.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(position), status + "\nHP: " + CurrHealth.ToString(), Color.black, 2);
            go.SetActive(true);
            foreach (var pos in watchedTiles)
            {
                GameObject tempGo = EffectPooler._instance.GetPooledObject(EffectPooler._instance.tempTargetEffects);
                tempGo.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(pos), "", Color.black, 2);
                tempGo.SetActive(true);
            }
        }
        
    }

    void UpdateWatchedTiles()
    {
        if (!Active)
            return;
        watchedTiles = Pathfinder._instance.GeneralVisionCast(position, BaseVision);
    }

    public void Deactivate()
    {
        Active = false;
        DeleventSystem.mapVisualsUpdate();
    }

    void CheckForStateChange()
    {
        if (!Active)
            return;
         foreach (var item in watchedTiles)
         {
             switch (UnitManager._instance.CheckSusLevel(item))
             {
                 case SuspiciousLevel.Unsuspicious:
                     break;
                 case SuspiciousLevel.Alarming:
                    if (UnitManager._instance.units.ContainsKey(item))
                    {
                        foreach (var unit in UnitManager._instance.units[item])
                        {
                            if(unit is EnemyUnit)
                            {
                                
                            }
                            else
                            {
                                if(unit.susLvl == SuspiciousLevel.Alarming)
                                {
                                    DeleventSystem.fireAlarm();
                                    GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.damageEffects);
                                    go.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(position), "!", Color.red, 2, 1);
                                    go.SetActive(true);
                                    return;
                                }
                            }
                        }
                    }
                    break;
                 default:
                     break;
             }
         }
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
            Deactivate();
            susLvl = SuspiciousLevel.Alarming;
        }
        //Possible events?
    }

    public ReachType GetAttackReachType()
    {
        return ReachType.sameTile;
    }

    public override SerializedDataContainer Serialize()
    {
        SerializedDataContainer container = base.Serialize();
        container.Serialize(BaseHealth);
        container.Serialize(BaseVision);
        container.Serialize(CurrHealth);
        container.Serialize(Active);
        return container;
    }

    public override void Deserialize(SerializedDataContainer input)
    {
        base.Deserialize(input);
        BaseHealth = input.GetFirstInt();
        BaseVision = input.GetFirstInt();
        CurrHealth = input.GetFirstInt();
        Active = input.GetFirstBool();
    }

    public override SerializableClasses GetSerializableType()
    {
        return SerializableClasses.enemyUnit;
    }
}
