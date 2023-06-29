using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStarter : MonoBehaviour
{
    public static LevelStarter _instance;
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

    public string[] levelNames;
    string nextLevel;

    private void Start()
    {
        //StartLevel("SampleScene 1");
        //StartLevel("Mission01");

    }

    public void SelectLevel(BaseQuest nextQuest)
    {
        QuestManager._instance.currentQuest = nextQuest;
        /*        switch (i)
        {
            case 1:
                QuestManager._instance.currentQuest = QuestManager._instance.mission01;
                break;
            case 2:
                QuestManager._instance.currentQuest = QuestManager._instance.mission02;
                break;
            case 3:
                QuestManager._instance.currentQuest = QuestManager._instance.mission03;
                break;
            default:
                break;
        }*/
        StartLevel();
    }
    void StartLevel()
    {
        QuestManager._instance.currentQuest.OnLevelSelect(LevelSelectCallBack);

    }

    void LevelSelectCallBack()
    {
        StartCoroutine(LoadYourAsyncScene(QuestManager._instance.currentQuest.sceneName));
    }

    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        /*
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        */
        SceneManager.LoadScene(sceneName);
        yield return new WaitForSeconds(0.2f);
        DeleventSystem.levelStart();
        GameDataManager._instance.SetupLevel();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
