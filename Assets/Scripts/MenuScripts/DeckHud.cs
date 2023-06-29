using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeckHud : MonoBehaviour
{
    public TMP_Text deckText;
    public TMP_Text discardText;
    public TMP_Text turnCounterText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        deckText.text = CardManager._instance.deckCards.Count.ToString();
        discardText.text = CardManager._instance.discardCards.Count.ToString();
    }

    public void OnDeckButton()
    {
        if(GameManager._instance.activeUnit.CurrAP >= 2 && CardManager._instance.deckCards.Count > 0)
        {
            GameManager._instance.AddSaveState();
            GameManager._instance.activeUnit.CurrAP -= 2;
            CardManager._instance.DrawTopCard();
        }
    }
    public void OnDiscardButton()
    {

    }
    public void OnRedoButton()
    {
        GameManager._instance.ReturnToLatestState();
    }
}
