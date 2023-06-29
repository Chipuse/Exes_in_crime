using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUnitHud : MonoBehaviour
{
    public TMP_Text characterName;
    public TMP_Text disguiseLvl;
    //public TMP_Text hpAp;
    //public TMP_Text stats;
    public TMP_Text statsInt;
    public TMP_Text statsPwr;
    public TMP_Text statsMov;
    public TMP_Text statsChr;
    //public TMP_Text equip;
    public TMP_Text inventory;

    public GameObject portraitVin;
    public GameObject portraitPhib;
    public GameObject portraitSam;
    public GameObject portraitKero;
    public GameObject portraitJeanne;

    public CounterDisplayBase hpCounter;
    public CounterDisplayBase apCounter;
    public CounterDisplayBase invCounter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerData();
    }

    void UpdatePlayerData()
    {
        if (GameManager._instance.activeUnit == null)
            return;
        PlayerUnit temp = GameManager._instance.activeUnit;
        CharacterData tempData = temp.ownData;

        //CharacterSpecific setup:
        portraitVin.SetActive(false);
        portraitPhib.SetActive(false);
        portraitSam.SetActive(false);
        portraitKero.SetActive(false);
        portraitJeanne.SetActive(false);
        switch (tempData.character)
        {
            case DialogChar.None:
                break;
            case DialogChar.Vin:
                portraitVin.SetActive(true);
                break;
            case DialogChar.Phib:
                portraitPhib.SetActive(true);
                break;
            case DialogChar.Kero:
                portraitKero.SetActive(true);
                break;
            case DialogChar.Sam:
                portraitSam.SetActive(true);
                break;
            case DialogChar.Jeanne:
                portraitJeanne.SetActive(true);
                break;
            case DialogChar.System:
                break;
            case DialogChar.Mary:
                break;
            default:
                break;
        }

        characterName.text = temp.CharacterName;
        disguiseLvl.text = temp.GetDisguiseLevel().ToString();
        //hpAp.text = "Health: " + temp.CurrHealth + "/" + temp.BaseHealth + " | AP: " + temp.CurrAP + " / " + temp.BaseAP;
        hpCounter.currNumber = temp.CurrHealth;
        hpCounter.maxNumber = temp.BaseHealth;
        //stats.text = "Int: " + temp.CurrInt + " | Atk: " + temp.CurrAtk + " | Mov: " + temp.CurrMove + " | Cha: " + temp.CurrCha;
        apCounter.currNumber = temp.CurrAP;
        apCounter.maxNumber = temp.BaseAP;

        statsInt.text = temp.CurrInt.ToString();
        statsPwr.text = temp.CurrAtk.ToString();
        statsMov.text = temp.CurrMove.ToString();
        statsChr.text = temp.CurrCha.ToString();
        // equips
        //equip.text = "";
        //if (temp.handSlot != null)
        //    equip.text += "Hand: " + temp.handSlot.data.Name;
        //else
        //    equip.text += "Hand: ---";
        //if (temp.bodySlot != null)
        //    equip.text += " | Body: " + temp.bodySlot.data.Name;
        //else
        //    equip.text += " | Body: ---";
        //// inv
        //if(temp.inventory.Count <= 0)
        //{
        //    inventory.text = "Inv: Empty";
        //}
        //else
        //{
        //    inventory.text = "Inv:";
        //    foreach (var item in temp.inventory)
        //    {
        //        inventory.text += ", " + item.data.Name;
        //    }
        //}
        if (temp.handSlot != null)
            invCounter.currNumber = temp.handSlot.data.ID;
        else
            invCounter.currNumber = -1;
        if (temp.bodySlot != null)
            invCounter.maxNumber = temp.bodySlot.data.ID;
        else
            invCounter.maxNumber = -1;
        invCounter.numberList = new List<int>();
        foreach (var item in temp.inventory)
        {
            invCounter.numberList.Add(item.data.ID);
        }
    }
}
