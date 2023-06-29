using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDatabase", menuName = "ScriptableObjects/CardData", order = 1)]
public class CardDatabase : ScriptableObject
{
    [SerializeField]
    public List<CardData> cards;

    [SerializeField]
    public List<CardObj> cardObjs;

    public void ReloadCardData()
    {
        cards = CardTester.FetchCardListStatic();
        cardObjs = new List<CardObj>();
        foreach (CardData card  in cards)
        {
            CardObj tempObj = new CardObj { data = card };
            tempObj.data = card;
            tempObj.prefab = CardTester.GetPrefabByData(card);
            cardObjs.Add(tempObj);
        }
    }
}



[Serializable]
public struct CardData
{
    public int ID;
    public string Name;
    public string IlluName;
    //maybe image
    public RequTypes RequType;
    public int RequAmount;
    public int Cost;
    public string[] Type;
    public Slots Slot;
    public string DescriptionText;
    public string FlavorText;
    public CardPools CardPool1;
    public CardPools CardPool2;
    public int Unlock;
    public int Shop;
    public int[] Variables;
}

[Serializable]
public struct CardObj
{
    public CardData data;
    public GameObject prefab;
}
