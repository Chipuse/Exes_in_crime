using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class QuestUnitUI : MonoBehaviour
{
    public QuestUnit unit;

    public TMP_Text nameText;
    public TMP_Text healthText;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }
    public void UpdateUI()
    {
        nameText.text = unit.QuestUnitName;
        healthText.text = unit.QuestUnitDescription;
    }
}
