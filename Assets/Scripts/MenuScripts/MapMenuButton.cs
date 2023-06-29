using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private bool mouse_over = false;
    public int buttonId = 0;

    public delegate void OnClickedEventId(int _buttonId);
    public OnClickedEventId ButtonClickeId;
    void Update()
    {
        
    }

    public void SetUpButton(int _id, OnClickedEventId _callBack)
    {
        buttonId = _id;
        ButtonClickeId += _callBack;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(ButtonClickeId != null)
        {
            ButtonClickeId(buttonId);
        }
    }
}
