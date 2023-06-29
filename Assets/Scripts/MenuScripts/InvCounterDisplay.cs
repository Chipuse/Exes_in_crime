using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InvCounterDisplay : CounterDisplayBase
{
    public TMP_Text handText;
    public TMP_Text bodyText;
    public TMP_Text inv1Text;
    public TMP_Text inv2Text;
    public TMP_Text inv3Text;

    public Image handImage;
    public Image bodyImage;
    public Image inv1Image;
    public Image inv2Image;
    public Image inv3Image;

    public List<int> lastList;

    // Start is called before the first frame update
    void Start()
    {
        numberList = new List<int>();
        lastList = new List<int>();
    }

    // Update is called once per frame
    public override void UpdateUI()
    {
        handImage.gameObject.SetActive(true);
        bodyImage.gameObject.SetActive(true);
        inv1Image.gameObject.SetActive(true);
        inv2Image.gameObject.SetActive(true);
        inv3Image.gameObject.SetActive(true);

        if (currNumber == -1)
        {
            handText.text = "---";
            handImage.gameObject.SetActive(false);
        }
        else
        {
            if(handText.text != CardTester.GetCardByID(currNumber).Name)
            {
                handText.text = "" + CardTester.GetCardByID(currNumber).Name;
                handImage.sprite = CardTester.GetIlluByID(currNumber);
                // background to color of card
            }
        }

        if (maxNumber == -1)
        {
            bodyText.text = "---";
            bodyImage.gameObject.SetActive(false);
        }
        else
        {
            if(bodyText.text != CardTester.GetCardByID(maxNumber).Name)
            {
                bodyText.text = "" + CardTester.GetCardByID(maxNumber).Name;
                bodyImage.sprite = CardTester.GetIlluByID(maxNumber);
            }
        }


        for (int i = 0; i < numberList.Count; i++)
        {
            if(i >= lastList.Count)
            {
                // something new need to be added
                CardData tempData = CardTester.GetCardByID(numberList[i]);
                switch (i)
                {
                    case 0:
                        // hand
                        inv1Text.text = tempData.Name;
                        //TODO: get image via cardData
                        inv1Image.sprite = CardTester.GetIlluByName(tempData.IlluName);
                        break;
                    case 1:
                        // body
                        inv2Text.text = tempData.Name;
                        inv2Image.sprite = CardTester.GetIlluByName(tempData.IlluName);
                        break;
                    case 2:
                        // inv 1
                        inv3Text.text = tempData.Name;
                        inv3Image.sprite = CardTester.GetIlluByName(tempData.IlluName);
                        break;
                    default:
                        break;
                }
            }
            else if (numberList[i] == lastList[i])
            {
                //nothing changed
                continue;
            }
            else
            {
                CardData tempData = CardTester.GetCardByID(numberList[i]);
                switch (i)
                {
                    case 0:
                        // hand
                        inv1Text.text = tempData.Name;
                        inv1Image.sprite = CardTester.GetIlluByName(tempData.IlluName);
                        //TODO: get image via cardData
                        break;
                    case 1:
                        // body
                        inv2Text.text = tempData.Name;
                        inv2Image.sprite = CardTester.GetIlluByName(tempData.IlluName);
                        break;
                    case 2:
                        // inv 1
                        inv3Text.text = tempData.Name;
                        inv3Image.sprite = CardTester.GetIlluByName(tempData.IlluName);
                        break;
                    default:
                        break;
                }
            }
        }
        if(numberList.Count < 3)
        {
            inv3Text.text = "---";
            inv3Image.gameObject.SetActive(false);

        }
        if (numberList.Count < 2)
        {
            inv2Text.text = "---";
            inv2Image.gameObject.SetActive(false);
        }
        if (numberList.Count < 1)
        {
            inv1Text.text = "---";
            inv1Image.gameObject.SetActive(false);
        }
        lastList = numberList;
    }
}
