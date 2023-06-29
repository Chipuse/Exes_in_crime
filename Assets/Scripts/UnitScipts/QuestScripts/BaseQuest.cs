using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaseQuest : MonoBehaviour
{
    public static BaseQuest currentQuest;
    void Awake()
    {
        
    }

    public string sceneName;
    public string MissionDescription;
    public string[] objectiveTexts;
    protected int QuestStepIndex = 0;

    public int numberCharacters = 1;
    public List<DialogChar> usableCharacters;

    [HideInInspector]
    public List<PositionKey> unitPlacementFields = new List<PositionKey>();


    [HideInInspector]
    public List<GameObject> placementFieldEffects = new List<GameObject>();
    //GameDataManager.GetCharacterData(usableCharacters[0]);

    protected virtual void OnEnable()
    {
        
    }

    protected virtual void OnDisable()
    {
        
    }

    public virtual void ShowPlacmentFields()
    {
        foreach (var item in placementFieldEffects)
        {
            item.SetActive(false);
        }
        placementFieldEffects = new List<GameObject>();
        foreach (var item in unitPlacementFields)
        {
            GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.placementEffects);
            go.SetActive(true);
            go.transform.SetPositionAndRotation(MapManager._instance.GroundGridPosToWorldPos(item), go.transform.rotation);
            go.transform.Translate(Vector3.up * 0.53f, Space.World);
            placementFieldEffects.Add(go);
        }
    }

    public virtual void HidePlacmentFields()
    {
        foreach (var item in placementFieldEffects)
        {
            item.SetActive(false);
        }
        placementFieldEffects = new List<GameObject>();
    }

    public virtual void OnLevelSelect(DeleventSystem.SimpleEvent _func)
    {
        if (_func != null)
            _func();
    }

    public virtual void OnLevelEnter(DeleventSystem.SimpleEvent _func)
    {
        if(_func != null)
            _func();
    }

    public virtual void OnUnitPlacement(DeleventSystem.SimpleEvent _func)
    {
        if (_func != null)
            _func();
    }

    public virtual void OnLevelFinished(DeleventSystem.SimpleEvent _func)
    {
        if (_func != null)
            _func();
    }

    public virtual void OnLevelExit(DeleventSystem.SimpleEvent _func)
    {
        if (_func != null)
            _func();
    }

    public virtual void UpdateObjectiveText()
    {
        if (QuestStepIndex < 0)
        {
            QuestManager._instance.objectiveText.text = "Game Over";
        }
        else if (objectiveTexts.Length > QuestStepIndex)
        {
            QuestManager._instance.objectiveText.text = objectiveTexts[QuestStepIndex];
        }
        else
        {
            QuestManager._instance.objectiveText.text = "Missing text for QuestIndex " + QuestStepIndex;
        }
    }

    public virtual void QuestMilestone(int milestoneIndex)
    {

    }

    public virtual void UpdateQuest()
    {
        UpdateObjectiveText();
    }
}