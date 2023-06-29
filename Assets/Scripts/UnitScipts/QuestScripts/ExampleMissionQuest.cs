using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleMissionQuest : BaseQuest
{
    public int CardIDToSteal = 43;
    
    //0 when needing to get egg into possession
    //1 when needing to exfiltrate with egg
    //2 egg is inside extraction zone

    public override void UpdateQuest()
    {
        bool nooneAlive = true;
        foreach (var unit in GameManager._instance.currentPlayerUnits)
        {
            if (unit.alive)
                nooneAlive = false;
        }

        //check if egg is in possession of playerunits
        foreach (var unit in GameManager._instance.currentPlayerUnits)
        {
            foreach (var item in unit.inventory)
            {
                if(item.data.ID == CardIDToSteal)
                {
                    QuestStepIndex = 1;
                    if(MapManager._instance.GetSecurityLevel(unit.position) < 0)
                    {
                        QuestStepIndex = 2;
                    }
                }
            }
        }
        switch (QuestStepIndex)
        {
            case -1:
                break;
            case 0:
                break;
            case 1:
                break;
            case 2:
                // win the fucking game
                break;
            default:
                break;
        }
        if (nooneAlive)
            QuestStepIndex = -1;
        base.UpdateQuest();
    }


    public override void UpdateObjectiveText()
    {
        if(QuestStepIndex < 0)
        {
            QuestManager._instance.objectiveText.text = "Game Over";
        }
        else if(objectiveTexts.Length > QuestStepIndex)
        {
            QuestManager._instance.objectiveText.text = objectiveTexts[QuestStepIndex];
        }
        else
        {
            QuestManager._instance.objectiveText.text = "Missing text for QuestIndex " + QuestStepIndex;
        }
    }
}
