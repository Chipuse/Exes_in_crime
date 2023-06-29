using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APCounterDisplayBase : CounterDisplayBase
{
    // Start is called before the first frame update
    public GameObject bar1;
    public GameObject bar2;
    public GameObject bar3;
    void Start()
    {
        
    }

    // Update is called once per frame
    public override void UpdateUI()
    {
        bar3.SetActive(false);
        bar2.SetActive(false);
        bar1.SetActive(false);
        if (currNumber >= 3)
        {
            bar3.SetActive(true);
        }
        else if(currNumber >= 2)
        {
            bar2.SetActive(true);
        }
        else if(currNumber >= 1)
        {
            bar1.SetActive(true);
        }
        textDisplay.text = currNumber.ToString() + "/" + maxNumber.ToString();
    }
}
