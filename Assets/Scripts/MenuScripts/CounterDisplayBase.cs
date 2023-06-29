using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CounterDisplayBase : MonoBehaviour
{
    public int currNumber;
    public int maxNumber;
    public int minNumber;

    public string textToDisplay;

    public List<int> numberList;

    //can have much more
    public TMP_Text textDisplay;

    private void OnEnable()
    {
        //DeleventSystem.playerUnitUpdate += UpdateUI;
    }

    private void OnDisable()
    {
        //DeleventSystem.playerUnitUpdate -= UpdateUI;
    }

    private void Update()
    {
        UpdateUI();
    }


    public virtual void UpdateUI()
    {

    }
}
