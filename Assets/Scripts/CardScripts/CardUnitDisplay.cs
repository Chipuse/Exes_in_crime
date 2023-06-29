using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardUnitDisplay : MonoBehaviour
{
    public Color magenta;
    public BaseCardScript data;
    public TMP_Text nameText;
    public TMP_Text costText;
    public TMP_Text slotText;
    public TMP_Text requirementText;
    public TMP_Text typeText; //not needed anymore
    public TMP_Text cardTextText;
    public Image highlight;
    public Image artwork;

    public int id;

    public bool highlighted = false;

    //new things:

    //frame dependened on cardPool
    public GameObject FrameKeroJeanne;
    public GameObject FrameKeroSam;
    public GameObject FramePhibKero;
    public GameObject FramePhibSam;
    public GameObject FramePhibJeanne;
    public GameObject FrameSamJeanne;
    public GameObject FrameVinJeanne;
    public GameObject FrameVinKero;
    public GameObject FrameVinPhib;
    public GameObject FrameVinSam;
    public GameObject FrameVin;
    public GameObject FrameJeanne;
    public GameObject FramePhib;
    public GameObject FrameSam;
    public GameObject FrameKero;
    public GameObject FrameGeneric;

    //slots
    public GameObject SlotFrame;

    public GameObject SlotHand;
    public GameObject SlotBody;
    public GameObject SlotCollectable;
    public GameObject SlotGadget;

    //requirements
    public GameObject RequInt;
    public GameObject RequChr;
    public GameObject RequSpd;
    public GameObject RequPwr;

    // Start is called before the first frame update
    bool Setup = false;
    void Start()
    {
        Setup = false;
    }

    // Update is called once per frame
    void Update()
    {
        highlight.enabled = highlighted;
        if(data == null ||Setup == true)
        {
            return;
        }
        Setup = true;

        nameText.text = data.data.Name;
        costText.text = data.data.Cost.ToString();
        //slotText.text = GenerateSlotText();
        //requirementText.text = GenerateRequText();
        //typeText.text = GenerateTypeText();
        cardTextText.text = GenerateCardText();

        SetFrame(); // maybe not in update...
        SetRequ();
        SetSlot();
        SetIllu();
    }

    void SetFrame()
    {
        FrameKeroJeanne.SetActive(false);
        FrameKeroSam.SetActive(false);
        FramePhibKero.SetActive(false);
        FramePhibSam.SetActive(false);
        FramePhibJeanne.SetActive(false);
        FrameSamJeanne.SetActive(false);
        FrameVinJeanne.SetActive(false);
        FrameVinKero.SetActive(false);
        FrameVinPhib.SetActive(false);
        FrameVinSam.SetActive(false);
        FrameVin.SetActive(false);
        FrameJeanne.SetActive(false);
        FramePhib.SetActive(false);
        FrameSam.SetActive(false);
        FrameKero.SetActive(false);
        FrameGeneric.SetActive(false);

        switch (data.data.CardPool1)
        {
            case CardPools.Neutral:
                FrameGeneric.SetActive(true);
                break;
            case CardPools.Vin:
                switch (data.data.CardPool2)
                {
                    case CardPools.Neutral:
                        FrameVin.SetActive(true);
                        break;
                    case CardPools.Vin:
                        FrameVin.SetActive(true);
                        break;
                    case CardPools.Phib:
                        FrameVinPhib.SetActive(true);
                        break;
                    case CardPools.Sam:
                        FrameVinSam.SetActive(true);
                        break;
                    case CardPools.Kero:
                        FrameVinKero.SetActive(true);
                        break;
                    case CardPools.Jeanne:
                        FrameVinJeanne.SetActive(true);
                        break;
                    case CardPools.Invalid:
                        FrameVin.SetActive(true);
                        break;
                    default:
                        break;
                }
                break;
            case CardPools.Phib:
                switch (data.data.CardPool2)
                {
                    case CardPools.Neutral:
                        FramePhib.SetActive(true);
                        break;
                    case CardPools.Vin:
                        FrameVinPhib.SetActive(true);
                        break;
                    case CardPools.Phib:
                        FramePhib.SetActive(true);
                        break;
                    case CardPools.Sam:
                        FramePhibSam.SetActive(true);
                        break;
                    case CardPools.Kero:
                        FramePhibKero.SetActive(true);
                        break;
                    case CardPools.Jeanne:
                        FramePhibJeanne.SetActive(true);
                        break;
                    case CardPools.Invalid:
                        FramePhib.SetActive(true);
                        break;
                    default:
                        break;
                }
                break;
            case CardPools.Sam:
                switch (data.data.CardPool2)
                {
                    case CardPools.Neutral:
                        FrameSam.SetActive(true);
                        break;
                    case CardPools.Vin:
                        FrameVinSam.SetActive(true);
                        break;
                    case CardPools.Phib:
                        FramePhibSam.SetActive(true);
                        break;
                    case CardPools.Sam:
                        FrameSam.SetActive(true);
                        break;
                    case CardPools.Kero:
                        FrameKeroSam.SetActive(true);
                        break;
                    case CardPools.Jeanne:
                        FrameSamJeanne.SetActive(true);
                        break;
                    case CardPools.Invalid:
                        FrameSam.SetActive(true);
                        break;
                    default:
                        break;
                }
                break;
            case CardPools.Kero:
                switch (data.data.CardPool2)
                {
                    case CardPools.Neutral:
                        FrameKero.SetActive(true);
                        break;
                    case CardPools.Vin:
                        FrameVinKero.SetActive(true);
                        break;
                    case CardPools.Phib:
                        FramePhibKero.SetActive(true);
                        break;
                    case CardPools.Sam:
                        FrameKeroSam.SetActive(true);
                        break;
                    case CardPools.Kero:
                        FrameKero.SetActive(true);
                        break;
                    case CardPools.Jeanne:
                        FrameKeroJeanne.SetActive(true);
                        break;
                    case CardPools.Invalid:
                        FrameKero.SetActive(true);
                        break;
                    default:
                        break;
                }
                break;
            case CardPools.Jeanne:
                switch (data.data.CardPool2)
                {
                    case CardPools.Neutral:
                        FrameJeanne.SetActive(true);
                        break;
                    case CardPools.Vin:
                        FrameVinJeanne.SetActive(true);
                        break;
                    case CardPools.Phib:
                        FramePhibJeanne.SetActive(true);
                        break;
                    case CardPools.Sam:
                        FrameSamJeanne.SetActive(true);
                        break;
                    case CardPools.Kero:
                        FrameKeroJeanne.SetActive(true);
                        break;
                    case CardPools.Jeanne:
                        FrameJeanne.SetActive(true);
                        break;
                    case CardPools.Invalid:
                        FrameJeanne.SetActive(true);
                        break;
                    default:
                        break;
                }
                break;
            case CardPools.Invalid:
                FrameGeneric.SetActive(true);
                break;
            default:
                break;
        }
    }

    void SetSlot()
    {
        SlotFrame.SetActive(true);
        SlotHand.SetActive(false);
        SlotBody.SetActive(false);
        SlotCollectable.SetActive(false);
        SlotGadget.SetActive(false);

        switch (data.data.Slot)
        {
            case Slots.Event:
                SlotFrame.SetActive(false);
                break;
            case Slots.Hand:
                SlotHand.SetActive(true);
                break;
            case Slots.Body:
                SlotBody.SetActive(true);
                break;
            case Slots.Companion:
                SlotGadget.SetActive(true);
                break;
            case Slots.Collectable:
                SlotCollectable.SetActive(true);
                break;
            case Slots.Invalid:
                SlotFrame.SetActive(false);
                break;
            default:
                SlotFrame.SetActive(false);
                break;
        }
    }

    void SetRequ()
    {
        RequInt.SetActive(false);
        RequChr.SetActive(false);
        RequSpd.SetActive(false);
        RequPwr.SetActive(false);

        if (data.data.RequAmount <= 0)
        {
            requirementText.gameObject.SetActive(false);
            return;
        }
        requirementText.gameObject.SetActive(false);
        requirementText.text = data.data.RequAmount.ToString();

        switch (data.data.RequType)
        {
            case RequTypes.Int:
                RequInt.SetActive(true);
                break;
            case RequTypes.Pwr:
                RequPwr.SetActive(true);
                break;
            case RequTypes.Chr:
                RequChr.SetActive(true);
                break;
            case RequTypes.Spd:
                RequSpd.SetActive(true);
                break;
            case RequTypes.Hp:
                break;
            case RequTypes.Ap:
                break;
            case RequTypes.Invalid:
                break;
            default:
                break;
        }
    }

    void SetIllu()
    {
        artwork.sprite = CardTester.GetIlluByName(data.data.IlluName);
    }

    public void GetCardDataByID(int _id)
    {
        //!!! DO NOT USE!!!
        id = _id;
        data = CardTester.CreateCardObj(_id);
    }
    public void SetCardData(BaseCardScript _data)
    {
        id = _data.data.ID;
        data = _data;
    }

    string GenerateSlotText()
    {
        string result = "";
        result += CardTester.ParseSlotType(data.data.Slot);
        return result;
    }

    string GenerateRequText()
    {
        string result = "";
        if (data.data.RequAmount <= 0)
            return result;
        result += CardTester.ParseRequType(data.data.RequType);
        result += ": ";
        result += data.data.RequAmount;
        return result;
    }

    string GenerateTypeText()
    {
        string result = "";
        for (int i = 0; i < data.data.Type.Length; i++)
        {
            result += data.data.Type[i];
            if(i != data.data.Type.Length - 1)
            {
                result += " | ";
            }
        }
        return result;
    }

    string GenerateCardText()
    {
        string result = data.data.DescriptionText;
        if(data.data.Variables.Length >= 1)
            result = result.Replace("var1", data.data.Variables[0].ToString());
        if (data.data.Variables.Length >= 2)
            result = result.Replace("var2", data.data.Variables[1].ToString());
        if (data.data.Variables.Length >= 3)
            result = result.Replace("var3", data.data.Variables[2].ToString());
        if (data.data.Variables.Length >= 4)
            result = result.Replace("var4", data.data.Variables[3].ToString());
        if (data.data.Variables.Length >= 5)
            result = result.Replace("var5", data.data.Variables[4].ToString());
        if (data.data.Variables.Length >= 6)
            result = result.Replace("var6", data.data.Variables[5].ToString());
        if (data.data.Variables.Length >= 7)
            result = result.Replace("var7", data.data.Variables[6].ToString());
        if (data.data.Variables.Length >= 7)
            result = result.Replace("var8", data.data.Variables[7].ToString());
        if (data.data.Variables.Length >= 8)
            result = result.Replace("var9", data.data.Variables[8].ToString());
        return result;
    }
}
