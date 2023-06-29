using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHolderScript : BaseUnit, ILootable, IAttackable, IHackable
{
    

    public int baseHackHealth = 5;
    public int baseAttackHealth = 5;
    public int currHackHealth = 5;
    public int currAttackHealth = 5;

    protected override void OnEnable()
    {
        base.OnEnable();
        DeleventSystem.clickedOnTile += OnClickedOnTile;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        DeleventSystem.clickedOnTile -= OnClickedOnTile;
    }

    public override void SetPositionByTransform()
    {
        base.SetPositionByTransform();
        transform.position = new Vector3(
            MapManager._instance.GroundGridPosToWorldPos(position).x,
            MapManager._instance.unitHeight,
            MapManager._instance.GroundGridPosToWorldPos(position).z
            );
    }

    public void OnClickedOnTile(PositionKey _pos)
    {
        if (position == _pos)
        {
            //string status = "";
            //if(currAttackHealth > 0)
            //{
            //    status += "Attack: "+ currAttackHealth + "\n";
            //}
            //if (currHackHealth > 0)
            //{
            //    status += "Hack: " + currHackHealth + "\n";
            //}
            //status += "Contains: \n";
            //foreach (var item in inventory)
            //{
            //    status += item.data.Name;
            //    status += "\n";
            //}
            //GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.damageEffects);
            //go.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(position), status, Color.black, 2);
            //go.SetActive(true);
        }
    }

    public bool Lootable()
    {
        if(inventory.Count > 0 && currAttackHealth <= 0 && currHackHealth <= 0)
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

    public bool Attackable()
    {
        if (currAttackHealth > 0 && !playerOpen)
            return true;
        return false;
    }

    public void GetAttacked(int damage)
    {
        currAttackHealth -= damage;
        GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.damageEffects);
        go.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(position), "-" + damage.ToString(), Color.red, 2, 1);
        go.SetActive(true);
        if (currAttackHealth <= 0)
        {
            susLvl = SuspiciousLevel.Alarming;
        }
        //Possible events?
    }

    public ReachType GetAttackReachType()
    {
        return ReachType.sameAndAdjacentTiles;
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
        go.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(position), "-" + damage.ToString(), Color.green, 2, 1);
        go.SetActive(true);
        if (currHackHealth <= 0)
        {

        }
        //Possible events?
    }

    public ReachType GetHackReachType()
    {
        return ReachType.sameAndAdjacentTiles;
    }

    public override SerializedDataContainer Serialize()
    {
        SerializedDataContainer container = base.Serialize();
        container.Serialize(baseAttackHealth);
        container.Serialize(currAttackHealth);
        container.Serialize(baseHackHealth);
        container.Serialize(currHackHealth);
        return container;
    }

    public override void Deserialize(SerializedDataContainer input)
    {
        base.Deserialize(input);
        baseAttackHealth = input.GetFirstInt();
        currAttackHealth = input.GetFirstInt();
        baseHackHealth = input.GetFirstInt();
        currHackHealth = input.GetFirstInt();
    }
}
