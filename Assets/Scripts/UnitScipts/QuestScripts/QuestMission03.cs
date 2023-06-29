using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMission03 : BaseQuest
{
    //0 gotophibs room
    //1 entered phibs room
    public QuestUnit phib;
    public List<PositionKey> targetTiles;

    public DialogObject missionStart;
    public DialogObject enterVenue;
    public DialogObject backroom;

    bool venueConvo = false;
    bool backroomConvo = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        DeleventSystem.lateLevelInit += OnLateLevelInit;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        DeleventSystem.lateLevelInit -= OnLateLevelInit;
    }

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
        if (!venueConvo)
        {
            foreach (var unit in GameManager._instance.currentPlayerUnits)
            {
                if(MapManager._instance.GetSecurityLevel(unit.position) > 0)
                {
                    QuestStepIndex = 1;
                }
            }
        }
        if (!backroomConvo)
        {
            foreach (var unit in GameManager._instance.currentPlayerUnits)
            {
                foreach (var tile in targetTiles)
                {
                    if(unit.position == tile)
                    {
                        QuestStepIndex = 2;
                    }
                }
            }
        }
        if (backroomConvo)
        {
            foreach (var unit in GameManager._instance.currentPlayerUnits)
            {
                foreach (var tile in targetTiles)
                {
                    if (unit.position == tile)
                    {
                        QuestStepIndex = 3;
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
                //win the fucking game
                if (!venueConvo)
                {
                    venueConvo = true;
                    GameManager._instance.ClearSaveStates();
                    ConversationManager._instance.StartConversation(enterVenue, null);
                }
                break;
            case 2:
                //win the fucking game
                if (!backroomConvo)
                {
                    backroomConvo = true;
                    GameManager._instance.ClearSaveStates();
                    ConversationManager._instance.StartConversation(backroom, null);
                }
                break;
            case 3:
                OnLevelFinished(LevelStarter._instance.BackToMainMenu);
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
        venueConvo = false;
        backroomConvo = false;
        ConversationManager._instance.StartConversation(missionStart, _func);
    }

    public void OnLateLevelInit()
    {
        if(QuestManager._instance.currentQuest == this)
        {
            foreach (var unit in GameManager._instance.currentPlayerUnits)
            {
                unit.bodySlot = CardTester.CreateCardObj(56);
            }
        }
    }
}
