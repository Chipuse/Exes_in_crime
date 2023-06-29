using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using KoganeUnityLib;
using TMPro;
using UnityEngine.UI;

[ExecuteAlways]
public class ConversationManager : MonoBehaviour
{
    public static ConversationManager _instance;
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            if(Application.isPlaying)
                DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseButtonDown();
        }
    }
    public bool ConversationIsPlaying = false;
    public GameObject background;
    public Color focusColor;
    public Color unfocusColor;
    public Color noneColor;

    public DialogObject currentDialog;
    public int currentTextBox = 0;
    bool autoMode = false;
    bool textBoxRunning;
    public static DeleventSystem.SimpleEvent onConvoFinished;

    public GameObject conversationObject;

    public AudioSource voiceLinePlayer;

    public Image conImageL;
    public Image conImageR;

    public TMP_Text nameDisplay;
    public TMP_Text textDisplay;
    string FullText = "";
    string CurrText = "";
    public float waitTime = 0.01f;

    bool textBoxFinished = false;
    IEnumerator TypewriterRoutine()
    {
        textBoxFinished = false;
        CurrText = "";
        for (int i = 0; i <= FullText.Length; i++)
        {
            CurrText = FullText.Substring(0, i);
            textDisplay.text = CurrText;
            if (textBoxFinished)
            {
                textDisplay.text = FullText;
                break;
            }
            yield return new WaitForSeconds(waitTime);
        }
        // finished typewriter
        textBoxFinished = true;
    }

    public void OnMouseButtonDown()
    {
        if (textBoxFinished)
        {
            ContinueConversation();
        }
        else
        {
            SkipTypeWriter();
        }
    }

    void SkipTypeWriter()
    {
        textBoxFinished = true;
    }

    public void SetNewDialog(DialogObject _newDialog)
    {
        currentDialog = _newDialog;
        currentTextBox = 0;
    }

    void FillDialogUI()
    {
        

        conImageL.sprite = GameDataManager.GetCharacterData(currentDialog.Textboxen[currentTextBox].ImageL).conImageL;
        if (currentDialog.Textboxen[currentTextBox].FocusL)
            conImageL.color = focusColor;
        else
            conImageL.color = unfocusColor;
        switch (currentDialog.Textboxen[currentTextBox].ImageL)
        {
            case DialogChar.None:
                conImageL.color = noneColor;
                break;
            case DialogChar.System:
                conImageL.color = noneColor;
                break;
            case DialogChar.Mary:
                conImageL.color = noneColor;
                break;
        }
        if (currentDialog.Textboxen[currentTextBox].FocusR)
            conImageR.color = focusColor;
        else
            conImageR.color = unfocusColor;
        switch (currentDialog.Textboxen[currentTextBox].ImageR)
        {
            case DialogChar.None:
                conImageR.color = noneColor;
                break;
            case DialogChar.System:
                conImageR.color = noneColor;
                break;
            case DialogChar.Mary:
                conImageR.color = noneColor;
                break;
        }
        conImageR.sprite = GameDataManager.GetCharacterData(currentDialog.Textboxen[currentTextBox].ImageR).conImageR;
        FullText = currentDialog.Textboxen[currentTextBox].Text;
        nameDisplay.text = GameDataManager.GetCharacterData(currentDialog.Textboxen[currentTextBox].Speaker).CharacterName;

        if(currentDialog.Textboxen[currentTextBox].Voiceline != null)
        {
            voiceLinePlayer.Stop();
            voiceLinePlayer.PlayOneShot(currentDialog.Textboxen[currentTextBox].Voiceline);
        }
    }

    public void StartConversation(DialogObject _newDialog, DeleventSystem.SimpleEvent _func = null)
    {
        Debug.Log("started convo");
        ConversationIsPlaying = true;
        conversationObject.SetActive(true);
        if (GameManager._instance != null)
            background.SetActive(false);
        else
            background.SetActive(true);
        //enable overlay
        //check if game is running and if disable background

        // setnew dialog

        // switch input to dialog
        // "pause" game (in current mode -> map, enemy turn, overworld ... )
        // -> conect with animationmanager?

        // animate beginning oif convo ( text box appears, first sprites)
        currentDialog = _newDialog;
        currentTextBox = 0;
        if(currentDialog.Textboxen.Count > currentTextBox)
        {
            conversationObject.SetActive(true);
            FillDialogUI();
            StartCoroutine(TypewriterRoutine());
        }
        if (_func != null)
        {
            onConvoFinished += _func;
        }
    }

    public void ContinueConversation()
    {
        //triggered by clicking when textbox is finished unless conversationmode is auto
        //if auto it gets triggered after a set period of time

        // if there is a next textbox engage next textbox like in startConvo (-> if necessary change images and textbox)
        currentTextBox += 1;
        if (currentDialog.Textboxen.Count > currentTextBox && ConversationIsPlaying)
        {
            conversationObject.SetActive(true);
            FillDialogUI();
            StartCoroutine(TypewriterRoutine());
        }
        else
        {
            EndConversation();
        }
    }

    public void FinishTextBox()
    {
        //gets triggered when clicked while textbox is not finished yet
        //completes typewriter if not happened yet
        //sets currenttextbox to finished

        //maybe cut off voiceover
    }

    public void EndConversation()
    {
        ConversationIsPlaying = false;
        conversationObject.SetActive(false);
        //disable overlay
        conversationObject.SetActive(false);
        //hide images and textbox
        //get back into the game
        //let animationamanager continue
        if(onConvoFinished != null)
        {
            onConvoFinished();
            onConvoFinished = null;
        }
    }
}
