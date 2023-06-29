using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    public string CharacterName;
    public GameObject characterUnitPrefab;
    public Sprite conImageL;
    public Sprite conImageR;
    public DialogChar character;
}
