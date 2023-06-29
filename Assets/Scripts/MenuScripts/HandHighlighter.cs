using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandHighlighter : MonoBehaviour
{
    RectTransform rTf;
    // Start is called before the first frame update
    private void OnEnable()
    {
        rTf = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rTf != null)
            rTf.sizeDelta = new Vector2(Screen.width, Screen.height/5);
    }
}
