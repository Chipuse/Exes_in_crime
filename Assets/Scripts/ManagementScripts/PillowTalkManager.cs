using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillowTalkManager : MonoBehaviour
{
    public  static PillowTalkManager _instance;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    //needs to be serialized
    public DialogObject[] DialogsVinPhib;
    public int freeVinPhib;
    public int alreadyListenedVinPhib;

    public DialogObject[] DialogsVinSam;
    public int freeVinSam;
    public int alreadyListenedVinSam;

    public DialogObject[] DialogsVinKero;
    public int freeVinKero;
    public int alreadyListenedVinKero;

    public DialogObject[] DialogsSamPhib;
    public int freeSamPhib;
    public int alreadyListenedSamPhib;

    public DialogObject[] DialogsSamKero;
    public int freeSamKero;
    public int alreadyListenedSamKero;

    public DialogObject[] DialogsPhibKero;
    public int freePhibKero;
    public int alreadyListenedPhibKero;

    public GameObject hubMenuObject;

    private void OnEnable()
    {
        UnlockAllTalks();
    }

    private void OnDisable()
    {
        
    }

    private void Start()
    {
        RefreshPillowButtons();
    }

    void RefreshPillowButtons()
    {
        HubManager._instance.SetConvoBool(Convos.PhibKero, CheckForNextDialog(Convos.PhibKero));
        HubManager._instance.SetConvoBool(Convos.SamKero, CheckForNextDialog(Convos.SamKero));
        HubManager._instance.SetConvoBool(Convos.SamPhib, CheckForNextDialog(Convos.SamPhib));
        HubManager._instance.SetConvoBool(Convos.VinKero, CheckForNextDialog(Convos.VinKero));
        HubManager._instance.SetConvoBool(Convos.VinPhib, CheckForNextDialog(Convos.VinPhib));
        HubManager._instance.SetConvoBool(Convos.VinSam, CheckForNextDialog(Convos.VinSam));
        HubManager._instance.UpdateConvoButtons();
        HubManager._instance.SetButtons();
    }

    public bool CheckForNextDialog(Convos pair)
    {
        switch (pair)
        {
            case Convos.VinPhib:
                if(freeVinPhib > alreadyListenedVinPhib)
                {
                    if (DialogsVinPhib.Length > alreadyListenedVinPhib)
                    {
                        return true;
                    }
                }
                break;
            case Convos.VinSam:
                if (freeVinSam > alreadyListenedVinSam)
                {
                    if (DialogsVinSam.Length > alreadyListenedVinSam)
                    {
                        return true;
                    }
                }
                break;
            case Convos.VinKero:
                if (freeVinKero > alreadyListenedVinKero)
                {
                    if (DialogsVinKero.Length > alreadyListenedVinKero)
                    {
                        return true;
                    }
                }
                break;
            case Convos.SamPhib:
                if (freeSamPhib > alreadyListenedSamPhib)
                {
                    if (DialogsSamPhib.Length > alreadyListenedSamPhib)
                    {
                        return true;
                    }
                }
                break;
            case Convos.SamKero:
                if (freeSamKero > alreadyListenedSamKero)
                {
                    if (DialogsSamKero.Length > alreadyListenedSamKero)
                    {
                        return true;
                    }
                }
                break;
            case Convos.PhibKero:
                if (freePhibKero > alreadyListenedPhibKero)
                {
                    if (DialogsPhibKero.Length > alreadyListenedPhibKero)
                    {
                        return true;
                    }
                }
                break;
            default:
                break;
        }
        return false;
    }

    public void StartNextPillowTalk(Convos pair)
    {
        if(hubMenuObject != null)
        {
            hubMenuObject.SetActive(false);
        }
        switch (pair)
        {
            case Convos.VinPhib:
                if (freeVinPhib > alreadyListenedVinPhib)
                {
                    if (DialogsVinPhib.Length > alreadyListenedVinPhib)
                    {
                        ConversationManager._instance.StartConversation(DialogsVinPhib[alreadyListenedVinPhib], PillowTalkEndCallBack);
                        alreadyListenedVinPhib += 1;
                    }
                }
                break;
            case Convos.VinSam:
                if (freeVinSam > alreadyListenedVinSam)
                {
                    if (DialogsVinSam.Length > alreadyListenedVinSam)
                    {
                        ConversationManager._instance.StartConversation(DialogsVinSam[alreadyListenedVinSam], PillowTalkEndCallBack);
                        alreadyListenedVinSam += 1;
                    }
                }
                break;
            case Convos.VinKero:
                if (freeVinKero > alreadyListenedVinKero)
                {
                    if (DialogsVinKero.Length > alreadyListenedVinKero)
                    {
                        ConversationManager._instance.StartConversation(DialogsVinKero[alreadyListenedVinKero], PillowTalkEndCallBack);
                        alreadyListenedVinKero += 1;
                    }
                }
                break;
            case Convos.SamPhib:
                if (freeSamPhib > alreadyListenedSamPhib)
                {
                    if (DialogsSamPhib.Length > alreadyListenedSamPhib)
                    {
                        ConversationManager._instance.StartConversation(DialogsSamPhib[alreadyListenedSamPhib], PillowTalkEndCallBack);
                        alreadyListenedSamPhib += 1;
                    }
                }
                break;
            case Convos.SamKero:
                if (freeSamKero > alreadyListenedSamKero)
                {
                    if (DialogsSamKero.Length > alreadyListenedSamKero)
                    {
                        ConversationManager._instance.StartConversation(DialogsSamKero[alreadyListenedSamKero], PillowTalkEndCallBack);
                        alreadyListenedSamKero += 1;
                    }
                }
                break;
            case Convos.PhibKero:
                if (freePhibKero > alreadyListenedPhibKero)
                {
                    if (DialogsPhibKero.Length > alreadyListenedPhibKero)
                    {
                        ConversationManager._instance.StartConversation(DialogsPhibKero[alreadyListenedPhibKero], PillowTalkEndCallBack);
                        alreadyListenedPhibKero += 1;
                    }
                }
                break;
            default:
                break;
        }
    }

    void PillowTalkEndCallBack()
    {
        if(hubMenuObject != null)
        {
            hubMenuObject.SetActive(true);
            RefreshPillowButtons();
        }
    }

    public void UnlockAllTalks()
    {
        freeVinPhib = DialogsVinPhib.Length;
        freeVinSam = DialogsVinSam.Length;
        //freeVinKero = DialogsVinKero.Length;
        freeSamPhib = DialogsSamPhib.Length;
        //freeSamKero = DialogsSamKero.Length;
        //freePhibKero = DialogsPhibKero.Length;
    }
}
