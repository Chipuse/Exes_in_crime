using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUnitUI : MonoBehaviour
{

    public PlayerUnit unit;
    public TMP_Text actionText;
    public RectTransform bar;
    float fullScale;
    // Start is called before the first frame update
    void Start()
    {
        fullScale = bar.sizeDelta.x;
        if (unit == null)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        float tempScale;
        if (unit.CurrAP == 0)
            tempScale = 0;
        else
            tempScale = (fullScale / unit.BaseAP) * unit.CurrAP;
        bar.sizeDelta = new Vector2(tempScale, bar.sizeDelta.y);
        actionText.text = unit.CurrAP.ToString();
    }
}
