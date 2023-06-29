using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMission02 : BaseQuest
{
    //0 gotophibs room
    //1 entered phibs room
    public int CardIDToSteal = 43;

    public DialogObject missionSelect;
    public DialogObject missionStart;
    public DialogObject missionEnd;

    public override void UpdateQuest()
    {
        bool nooneAlive = true;
        foreach (var unit in GameManager._instance.currentPlayerUnits)
        {
            if (unit.alive)
                nooneAlive = false;
        }

        foreach (var unit in GameManager._instance.currentPlayerUnits)
        {
            foreach (var item in unit.inventory)
            {
                if (item.data.ID == CardIDToSteal)
                {
                    QuestStepIndex = 1;
                    if (MapManager._instance.GetSecurityLevel(unit.position) < 0)
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
                OnLevelFinished(LevelStarter._instance.BackToMainMenu);
                break;
            default:
                break;
        }
        if (nooneAlive)
            QuestStepIndex = -1;
        base.UpdateQuest();
    }

    public override void OnLevelSelect(DeleventSystem.SimpleEvent _func)
    {
        ConversationManager._instance.StartConversation(missionSelect, _func);
    }

    public override void OnUnitPlacement(DeleventSystem.SimpleEvent _func)
    {
        ConversationManager._instance.StartConversation(missionStart, _func);
    }

    public override void OnLevelFinished(DeleventSystem.SimpleEvent _func)
    {
        ConversationManager._instance.StartConversation(missionEnd, _func);
    }
}
