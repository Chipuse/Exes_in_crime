using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    public static InventoryMenu _instance;
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(this);
        }
    }

    public GameObject buttonParent;
    public GameObject buttonPrefab;
    public bool ChangedState = false;
    bool MenuOpen = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && MenuOpen)
        {
            CloseMenu();
        }
    }

    private void LateUpdate()
    {
        if (ChangedState)
            ChangedState = false;
    }

    public void OpenMenu()
    {
        if (ChangedState)
            return;
        InputManager._instance.SwitchInputMode(InputMode.menu);
        transform.position = MapManager._instance.GroundGridPosToWorldPos(InputManager._instance.cursor.mouseGridPos);
        buttonParent.SetActive(true);
        ChangedState = true;
        MenuOpen = true;
        CreateButtons();
    }

    public void CloseMenu()
    {
        InputManager._instance.SwitchInputMode(InputMode.map);
        if (ChangedState)
            return;
        foreach (Transform child in buttonParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        buttonParent.SetActive(false);
        ChangedState = true;
        MenuOpen = false;
    }

    public void OnMenuElemtClicked()
    {

    }

    private void CreateButtons()
    {
        int buttonID = 0;
        GameObject go = Instantiate<GameObject>(buttonPrefab, buttonParent.transform, false);
        //hand
        go.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -200 * buttonID);
        go.GetComponent<MapMenuButton>().SetUpButton(buttonID, OnButtonCallback);
        TMP_Text buttonText = go.transform.GetChild(0).GetComponent<TMP_Text>();
        if (GameManager._instance.activeUnit.handSlot != null)
            buttonText.text = "Hand: " + GameManager._instance.activeUnit.handSlot.data.Name;
        else
            buttonText.text = "Hand: ---";
        buttonID++;
        go = Instantiate<GameObject>(buttonPrefab, buttonParent.transform, false);
        go.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -200 * buttonID);
        go.GetComponent<MapMenuButton>().SetUpButton(buttonID, OnButtonCallback);
        buttonText = go.transform.GetChild(0).GetComponent<TMP_Text>();
        if (GameManager._instance.activeUnit.handSlot != null)
            buttonText.text = "Unequip: " + GameManager._instance.activeUnit.handSlot.data.Name;
        else
            buttonText.text = "Unequip: ---";
        buttonID++;
        //body buttonText.text = "Hand: " + GameManager._instance.activeUnit.handSlot.data.Name;
        go = Instantiate<GameObject>(buttonPrefab, buttonParent.transform, false);
        go.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -200 * buttonID);
        go.GetComponent<MapMenuButton>().SetUpButton(buttonID, OnButtonCallback);
        buttonText = go.transform.GetChild(0).GetComponent<TMP_Text>();
        if (GameManager._instance.activeUnit.bodySlot != null)
            buttonText.text = "Body: " + GameManager._instance.activeUnit.bodySlot.data.Name;
        else
            buttonText.text = "Body: ---";
        buttonID++;

        go = Instantiate<GameObject>(buttonPrefab, buttonParent.transform, false);
        go.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -200 * buttonID);
        go.GetComponent<MapMenuButton>().SetUpButton(buttonID, OnButtonCallback);
        buttonText = go.transform.GetChild(0).GetComponent<TMP_Text>();
        if (GameManager._instance.activeUnit.bodySlot != null)
            buttonText.text = "Unequip: " + GameManager._instance.activeUnit.bodySlot.data.Name;
        else
            buttonText.text = "Unequip: ---";
        buttonID++;
        //inv
        foreach (var cardObj in GameManager._instance.activeUnit.inventory)
        {
            go = Instantiate<GameObject>(buttonPrefab, buttonParent.transform, false);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -200 * buttonID);
            go.GetComponent<MapMenuButton>().SetUpButton(buttonID, OnButtonCallback);
            buttonText = go.transform.GetChild(0).GetComponent<TMP_Text>();
            buttonText.text = "Inv" + (buttonID - 1) +": " + cardObj.data.Name;
            buttonID++;
        }
    }

    private void OnButtonCallback(int _buttonId)
    {
        CloseMenu();        
        //ToDo possibility to put equips away
        switch (_buttonId)
        {
            case 0:
                //hand
                if(GameManager._instance.activeUnit.handSlot is IAbility)
                {
                    IAbility tempAbility = (IAbility)GameManager._instance.activeUnit.handSlot;
                    if (tempAbility.CheckAbilityCondition())
                    {
                        tempAbility.TriggerAbility();
                    }
                }
                break;
            case 1:
                if(GameManager._instance.activeUnit.handSlot != null)
                {
                    GameManager._instance.AddSaveState();
                    GameManager._instance.activeUnit.inventory.Add(GameManager._instance.activeUnit.handSlot);
                    GameManager._instance.activeUnit.handSlot = null;
                }
                break;
            case 2:
                if (GameManager._instance.activeUnit.bodySlot is IAbility)
                {
                    IAbility tempAbility = (IAbility)GameManager._instance.activeUnit.bodySlot;
                    if (tempAbility.CheckAbilityCondition())
                    {
                        tempAbility.TriggerAbility();
                    }
                }
                //body
                break;
            case 3:
                if (GameManager._instance.activeUnit.bodySlot != null)
                {
                    GameManager._instance.AddSaveState();
                    GameManager._instance.activeUnit.inventory.Add(GameManager._instance.activeUnit.bodySlot);
                    GameManager._instance.activeUnit.bodySlot = null;
                }
                break;
            default:
                // equip body or hand thingies
                if(GameManager._instance.activeUnit.inventory[_buttonId - 4] is BaseHandScript || GameManager._instance.activeUnit.inventory[_buttonId - 4] is BaseBodyScript)
                {
                    //equip hand thing
                    if(GameManager._instance.activeUnit.inventory[_buttonId - 4].CheckPlayCondition())
                    {
                        GameManager._instance.AddSaveState();
                        GameManager._instance.activeUnit.CurrAP -= GameManager._instance.activeUnit.inventory[_buttonId - 4].data.Cost;
                        Debug.Log("equipped card: " + GameManager._instance.activeUnit.inventory[_buttonId - 4].data.Name + " to " + GameManager._instance.activeUnit.CharacterName);
                        if(GameManager._instance.activeUnit.inventory[_buttonId - 4] is BaseHandScript)
                        {
                            if (GameManager._instance.activeUnit.handSlot != null)
                            {
                                GameManager._instance.activeUnit.inventory.Add(GameManager._instance.activeUnit.handSlot);
                            }
                            GameManager._instance.activeUnit.handSlot = GameManager._instance.activeUnit.inventory[_buttonId - 4];
                        }
                        else if(GameManager._instance.activeUnit.inventory[_buttonId - 4] is BaseBodyScript)
                        {
                            if (GameManager._instance.activeUnit.bodySlot != null)
                            {
                                GameManager._instance.activeUnit.inventory.Add(GameManager._instance.activeUnit.bodySlot);
                            }
                            GameManager._instance.activeUnit.bodySlot = GameManager._instance.activeUnit.inventory[_buttonId - 4];
                        }

                        GameManager._instance.activeUnit.inventory.RemoveAt(_buttonId - 4);
                    }
                }
                break;
        }
        DeleventSystem.playerUnitUpdate();
    }
}
