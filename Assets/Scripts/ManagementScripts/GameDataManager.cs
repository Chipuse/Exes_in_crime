using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager _instance;
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(this);
        }
    }

    public List<int> currentDeckCards = new List<int>();

    public List<CharacterUsability> characters = new List<CharacterUsability>();
    public CharacterUsability characterToDeploy;
    public bool deployed = false;
    public static CharacterData CDNone;
    public static CharacterData CDVin;
    public static CharacterData CDPhib;
    public static CharacterData CDSam;
    public static CharacterData CDKero;
    public static CharacterData CDJeanne;
    public static CharacterData CDSystem;
    public static CharacterData CDMary;

    private void Update()
    {

    }

    void StartLevel()
    {
        StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("SampleScene");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        for(int i = 0; i < characters.Count; i++)
        {
            CharacterUsability temp = characters[i];
            temp.Deployed = false;
            characters[i] = temp;
        }

        //deploy all units
        InputManager._instance.SwitchInputMode(InputMode.deployUnits);
        GameManager._instance.gameState = GameState.preperation;
        for (int i = 0; i < characters.Count; i++)
        {
            if(!characters[i].Deployed && characters[i].Usable && characters[i].ChosenForNextMission)
            {
                characterToDeploy = characters[i];
                deployed = false;
                while (!deployed)
                {
                    yield return null;
                }
            }
        }
        GameManager._instance.gameState = GameState.playerTurn;
        InputManager._instance.SwitchInputMode(InputMode.map);
        GameManager._instance.StartLevel();
    }

    public void SetupLevel()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            CharacterUsability temp = characters[i];
            temp.Deployed = false;
            characters[i] = temp;
        }

        //questevent: OnLevelEnter
        QuestManager._instance.currentQuest.OnLevelEnter(OnLevelEnterCallBack);
        
    }

    void OnLevelEnterCallBack()
    {
        StartCoroutine(LevelSetupRoutine());
    }

    IEnumerator LevelSetupRoutine()
    {
        //deploy all units
        InputManager._instance.SwitchInputMode(InputMode.deployUnits);
        GameManager._instance.gameState = GameState.preperation;
        /*
        for (int i = 0; i < characters.Count; i++)
        {
            if (!characters[i].Deployed && characters[i].Usable && characters[i].ChosenForNextMission)
            {
                characterToDeploy = characters[i];
                deployed = false;
                while (!deployed)
                {
                    yield return null;
                }
            }
        }
        */
        foreach (var item in MapManager._instance._mapData.secLvl)
        {
            if (item.level < 0)
            {
                QuestManager._instance.currentQuest.unitPlacementFields.Add(item.positionKey);
            }
        }
        QuestManager._instance.currentQuest.ShowPlacmentFields();
        foreach (var character in QuestManager._instance.currentQuest.usableCharacters)
        {
            characterToDeploy = new CharacterUsability { data = GetCharacterData(character), Deployed = false, Usable = true };
            deployed = false;
            while (!deployed)
                yield return null; 
        }
        OnCharacterDeployed();
    }

    void OnCharacterDeployed()
    {
        if (currentDeckCards != null && currentDeckCards.Count > 0)
            CardManager._instance.SetupSessionCards(currentDeckCards, new List<int>(), new List<int>());
        else
            CardManager._instance.SetupSessionCards(CardManager._instance.startDeckCards, CardManager._instance.startHandCards, CardManager._instance.startDiscardCards);

        QuestManager._instance.currentQuest.HidePlacmentFields();
        //questEvent: OnCharactersPlaced
        QuestManager._instance.currentQuest.OnUnitPlacement(CharacterPlacedCallback);
    }

    void CharacterPlacedCallback()
    {
        GameManager._instance.gameState = GameState.playerTurn;
        InputManager._instance.SwitchInputMode(InputMode.map);
        GameManager._instance.StartLevel();
    }

    

    public static CharacterData GetCharacterData(DialogChar _char)
    {
        switch (_char)
        {
            case DialogChar.None:
                if (CDNone == null)
                {
                    CDNone = Resources.Load<CharacterData>("Units/PlayerUnits/NoneCharacterData");
                }
                return CDNone;
            case DialogChar.Vin:
                if (CDVin == null)
                {
                    CDVin = Resources.Load<CharacterData>("Units/PlayerUnits/VinCharacterData");
                }
                return CDVin;
            case DialogChar.Phib:
                if (CDPhib == null)
                {
                    CDPhib = Resources.Load<CharacterData>("Units/PlayerUnits/PhibCharacterData");
                }
                return CDPhib;
            case DialogChar.Kero:
                if (CDKero == null)
                {
                    CDKero = Resources.Load<CharacterData>("Units/PlayerUnits/KeroCharacterData");
                }
                return CDKero;
            case DialogChar.Sam:
                if (CDSam == null)
                {
                    CDSam = Resources.Load<CharacterData>("Units/PlayerUnits/SamCharacterData");
                }
                return CDSam;
            case DialogChar.Jeanne:
                if (CDJeanne == null)
                {
                    CDJeanne = Resources.Load<CharacterData>("Units/PlayerUnits/JeanneCharacterData");
                }
                return CDJeanne;
            case DialogChar.System:
                if (CDSystem == null)
                {
                    CDSystem = Resources.Load<CharacterData>("Units/PlayerUnits/SystemCharacterData");
                }
                return CDSystem;
            case DialogChar.Mary:
                if (CDMary == null)
                {
                    CDMary = Resources.Load<CharacterData>("Units/PlayerUnits/MaryCharacterData");
                }
                return CDMary;
            default:
                Debug.LogError("No valid character selected");
                return null;
        }
    }
}

[Serializable]
public struct CharacterUsability
{
    public CharacterData data;
    public bool Usable;
    public bool ChosenForNextMission;
    public bool Deployed;
}
