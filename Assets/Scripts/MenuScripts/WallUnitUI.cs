using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class WallUnitUI : MonoBehaviour
{
    public WallUnit unit;
    public RectTransform hackBar;
    float hackFullScale;
    public RectTransform attackBar;
    float attackFullScale;
    public TMP_Text nameText;
    public TMP_Text hackText;
    public TMP_Text attackText;

    public GameObject hackPart;
    public GameObject attackPart;
    // Start is called before the first frame update
    private void Start()
    {
        hackFullScale = hackBar.sizeDelta.x;
        attackFullScale = attackBar.sizeDelta.x;
        nameText.text = unit.description;
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
        if (unit.currHackHealth == 0)
            tempScale = 0;
        else
            tempScale = (hackFullScale / unit.baseHackHealth) * unit.currHackHealth;
        hackBar.sizeDelta = new Vector2(tempScale, hackBar.sizeDelta.y);
        hackText.text = unit.currHackHealth.ToString();

        if (unit.currAttackHealth == 0)
            tempScale = 0;
        else
            tempScale = (attackFullScale / unit.baseAttackHealth) * unit.currAttackHealth;
        attackBar.sizeDelta = new Vector2(tempScale, attackBar.sizeDelta.y);
        attackText.text = unit.currAttackHealth.ToString();

        hackPart.SetActive(true);
        attackPart.SetActive(true);
        if(unit.currHackHealth <= 0)
        {
            hackPart.SetActive(false);
        }
        if(unit.currAttackHealth <= 0)
        {
            attackPart.SetActive(false);
        }
        if (unit.playerOpen)
        {
            hackPart.SetActive(false);
            attackPart.SetActive(false);
            nameText.text = unit.description + ": UNLOCKED";
        }
    }
}
