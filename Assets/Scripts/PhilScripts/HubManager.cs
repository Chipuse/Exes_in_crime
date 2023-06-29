using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HubManager : MonoBehaviour
{
    public static HubManager _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public GameObject MainButtons;
    public GameObject MissionButtons;
    public GameObject PillowButtons;
    public GameObject[] PBArray;
    private int[] PBData;
    public GameObject CreditsObject;

    public TMP_Text StartButtonText;
    public string startText;
    public string nextText;
    public GameObject PillowButton;
    public GameObject FakePillowButton;

    public bool notFirstTime;
    public int currentMission;
    public bool[] convoBools;

    // Start is called before the first frame update
    void Start()
    {
        InitiateHub();
    }

    private void InitiateHub()
    {
        MainButtons.SetActive(true);
        MissionButtons.SetActive(false);
        PillowButtons.SetActive(false);
        CreditsObject.SetActive(false);

        if (!notFirstTime)
        {
            SetData();
            notFirstTime = true;
        }

        SetButtons();
        UpdateConvoButtons();
    }

    
    private void SetData()
    {
        convoBools = new bool[6];
        currentMission = 0;
    }

    public void SetButtons()
    {
        StartButtonText.text = currentMission > 0 ? startText : nextText;
        if (isPillowTalk())
        {
            FakePillowButton.SetActive(false);
            PillowButton.SetActive(true);
        }
        else
        {
            FakePillowButton.SetActive(true);
            PillowButton.SetActive(false);
        }
    }

  

    public void MissionButton(int i)
    {
        if (i < 0)
        {
            StartMission(currentMission);
        }
        else
        {
            StartMission(i);
        }
    }

    private void StartMission(int missionNum)
    {
        //TODO for mark to fill :)
        switch (missionNum)
        {
            case 0:
                LevelStarter._instance.SelectLevel(QuestManager._instance.mission01);
                break;
            case 1:
                LevelStarter._instance.SelectLevel(QuestManager._instance.mission02);
                break;
            case 2:
                LevelStarter._instance.SelectLevel(QuestManager._instance.mission03);
                break;
            case 3:
                LevelStarter._instance.SelectLevel(QuestManager._instance.mission04);
                break;
            case 4:
                LevelStarter._instance.SelectLevel(QuestManager._instance.mission05);
                break;
            case 5:
                LevelStarter._instance.SelectLevel(QuestManager._instance.mission06);
                break;
            case 6:
                LevelStarter._instance.SelectLevel(QuestManager._instance.mission06);
                break;
            default:
                LevelStarter._instance.SelectLevel(QuestManager._instance.mission00);
                break;
        }
    }

    public void ShowSubMenu(GameObject subMenu)
    {
        subMenu.SetActive(true);
        MainButtons.SetActive(false);
        UpdateConvoButtons();
    }

    public void BackToMain(GameObject subMenu)
    {
        MainButtons.SetActive(true);
        SetButtons();
        subMenu.SetActive(false);
    }

    //for you mark to set the bool true for whatever conversation pair
    public void SetConvoBool(Convos c, bool b)
    {
        convoBools[(int) c] = b;
    }

    public void ConvoButton(int buttonNum)
    {
        Convos currentConvo = (Convos) PBData[buttonNum];
        StartConvo(currentConvo);
        SetConvoBool(currentConvo,false);
        UpdateConvoButtons();
    }

    private void StartConvo(Convos c)
    {
        //TODO for mark to fill :)
        PillowTalkManager._instance.StartNextPillowTalk(c);
    }

    public void UpdateConvoButtons()
    {
        PBData = new int[6];
        foreach (GameObject o in PBArray)
        {
            o.SetActive(false);
        }

        int k = 0;
        for (int i = 0; i < convoBools.Length; i++)
        {
            if (convoBools[i])
            {
                PBArray[k].SetActive(true);
                PBArray[k].transform.Find("Text").gameObject.GetComponent<TMP_Text>().text = ConvoName((Convos) i);
                PBData[k] = i;
                k++;
            }
        }
    }

    private string ConvoName(Convos c)
    {
        string s = "somethign went wrong";

        switch (c)
        {
            case Convos.VinPhib:
                s = "Vin + Phib";
                break;
            case Convos.VinSam:
                s = "Vin + Samantha";
                break;
            case Convos.VinKero:
                s = "Vin + Keroscene";
                break;
            case Convos.SamPhib:
                s = "Samantha + Phib";
                break;
            case Convos.SamKero:
                s = "Samantha + Keroscene";
                break;
            case Convos.PhibKero:
                s = "Phib + Keroscene";
                break;
        }

        return s;
    }

    private bool isPillowTalk()
    {
        foreach (bool b in convoBools)
        {
            if (b)
            {
                return true;
            }
        }

        return false;
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}

public enum Convos
{
    VinPhib,
    VinSam,
    VinKero,
    SamPhib,
    SamKero,
    PhibKero
}