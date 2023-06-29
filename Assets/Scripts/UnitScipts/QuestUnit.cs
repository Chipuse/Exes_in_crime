using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUnit : BaseUnit
{
    public string QuestUnitName = "QuestUnit";
    public string QuestUnitDescription = "Reach the Unit";

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
    }

    public void OnClickedOnTile(PositionKey _pos)
    {
        if (position == _pos)
        {
            string status = "";
            status += QuestUnitName + "\n";
            status += QuestUnitDescription;
            GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.damageEffects);
            go.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(position), status, Color.black, 2);
            go.SetActive(true);
        }
    }

    public override SerializedDataContainer Serialize()
    {
        SerializedDataContainer container = base.Serialize();
        container.Serialize(QuestUnitName);
        container.Serialize(QuestUnitDescription);
        return container;
    }

    public override void Deserialize(SerializedDataContainer input)
    {
        base.Deserialize(input);
        QuestUnitName = input.GetFirstString();
        QuestUnitDescription = input.GetFirstString();
    }
}
