using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class HPCounterDisplay : CounterDisplayBase
{
    public RectTransform bar;
    float fullScale;

    private void Start()
    {
        fullScale = bar.sizeDelta.x;
    }
    // Update is called once per frame
    public override void UpdateUI()
    {
        float tempScale;
        if (currNumber == 0)
            tempScale = 0;
        else
            tempScale = (fullScale / maxNumber) * currNumber;
        bar.sizeDelta = new Vector2(tempScale, bar.sizeDelta.y);
        textDisplay.text = currNumber.ToString() + "/" + maxNumber.ToString();
    }
}
