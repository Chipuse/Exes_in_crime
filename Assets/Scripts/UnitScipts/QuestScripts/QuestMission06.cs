using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMission06 : BaseQuest
{
    //0 gotophibs room
    //1 entered phibs room
    public QuestUnit phib;
    public List<PositionKey> targetTiles;

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

        if(phib == null)
        {
            var questobjs = FindObjectsOfType<QuestUnit>();
            if (questobjs.Length > 0)
                phib = questobjs[0];
        }
        if (phib == null)
            return;
        if(targetTiles == null || targetTiles.Count == 0)
        {
            targetTiles = new List<PositionKey>();
            targetTiles = Pathfinder._instance.GeneralNoiseFindingCast(phib.position, 10, true);
        }
        

        //check if egg is in possession of playerunits
        foreach (var unit in GameManager._instance.currentPlayerUnits)
        {
            foreach (var tile in targetTiles)
            {
                if(unit.position == tile)
                {
                    //reached his room!
                    QuestStepIndex = 1;
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
                //win the fucking game
                OnLevelFinished(InputManager._instance.QuitGame);
                break;
            default:
                break;
        }
        if (nooneAlive)
            QuestStepIndex = -1;
        base.UpdateQuest();
    }

    public override void OnLevelEnter(DeleventSystem.SimpleEvent _func)
    {
        ConversationManager._instance.StartConversation(missionStart, _func);
    }

    public override void OnLevelFinished(DeleventSystem.SimpleEvent _func)
    {
        ConversationManager._instance.StartConversation(missionEnd, _func);
    }
}
