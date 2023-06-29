using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class EnemyUnitUI : MonoBehaviour
{
    public EnemyUnit unit;

    public TMP_Text nameText;
    public TMP_Text healthText;
    public RectTransform bar;
    float fullScale;

    public GameObject alarmedSymbol;
    public GameObject suspicousSymbol;

    private void Start()
    {
        fullScale = bar.sizeDelta.x;
        nameText.text = unit.UnitName;
        if(unit == null)
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
        if (unit.CurrHealth == 0)
            tempScale = 0;
        else
            tempScale = (fullScale / unit.BaseHealth) * unit.CurrHealth;
        bar.sizeDelta = new Vector2(tempScale, bar.sizeDelta.y);
        healthText.text = unit.CurrHealth.ToString();

        alarmedSymbol.SetActive(false);
        suspicousSymbol.SetActive(false);
        switch (unit.state)
        {
            case EnemyState.Unsuspicious:
                break;
            case EnemyState.Suspicious:
                suspicousSymbol.SetActive(true);
                break;
            case EnemyState.Alarmed:
                alarmedSymbol.SetActive(true);
                break;
            case EnemyState.Dead:
                break;
            default:
                break;
        }
    }
}
