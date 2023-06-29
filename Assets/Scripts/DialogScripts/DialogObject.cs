using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogObject", menuName = "ScriptableObjects/DialogObject", order = 1)]
public class DialogObject : ScriptableObject
{
    public string ConvoName;
    [SerializeField]
    public List<TextBox> Textboxen = new List<TextBox>();

}

[Serializable]
public class TextBox
{
    public DialogChar Speaker;
    public string VoiceLinePath;
    public AudioClip Voiceline;
    public string Text;
    public DialogChar ImageL;
    public bool FocusL;
    public DialogChar ImageR;
    public bool FocusR;
}

[Serializable]
public enum DialogChar
{
    None,
    Vin,
    Phib,
    Kero,
    Sam,
    Jeanne,
    System,
    Mary
}
