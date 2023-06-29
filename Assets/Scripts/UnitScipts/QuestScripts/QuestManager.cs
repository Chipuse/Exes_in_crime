using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class QuestManager : MonoBehaviour
{
    public static QuestManager _instance;
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnEnable()
    {
        DeleventSystem.playerUnitUpdate += UpdateCurrentQuest;
    }

    private void OnDisable()
    {
        DeleventSystem.playerUnitUpdate -= UpdateCurrentQuest;

    }

    void UpdateCurrentQuest()
    {
        if (currentQuest != null)
            currentQuest.UpdateQuest();
    }

    public BaseQuest mission00;
    public BaseQuest mission01;
    public BaseQuest mission02;
    public BaseQuest mission03;
    public BaseQuest mission04;
    public BaseQuest mission05;
    public BaseQuest mission06;
    public BaseQuest mission07;

    public BaseQuest currentQuest;

    public TMP_Text objectiveText;
}
