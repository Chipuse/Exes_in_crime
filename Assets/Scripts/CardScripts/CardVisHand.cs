using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardVisHand : MonoBehaviour
{
    public static CardVisHand _instance;
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

    public GameObject cardVisPrefab;
    public Vector3 offset;
    public Vector3 passiveOffset;
    public Vector3 highlightOffset;
    Vector3 mouseOffset = new Vector3(0,0,0);
    public int highlightedCard;
    public int leftMargin;
    public int rightMargin;
    List<CardUnitDisplay> handCards = new List<CardUnitDisplay>();
    public Collider cardLayerSurface;
    float cardStepLength;
    float cardAreaStepLength;

    private void OnEnable()
    {
        DeleventSystem.newInputMode += EnterHandMode;
        DeleventSystem.oldInputMode += ExitHandMode;
        DeleventSystem.handVisualsUpdate += UpdateCardHighlight;
        DeleventSystem.playerUnitUpdate += UpdateCardHighlight;
    }
    private void OnDisable()
    {
        DeleventSystem.newInputMode -= EnterHandMode;
        DeleventSystem.oldInputMode -= ExitHandMode;
        DeleventSystem.handVisualsUpdate -= UpdateCardHighlight;
        DeleventSystem.playerUnitUpdate -= UpdateCardHighlight;
    }

    private void Update()
    {
        UpdateCardPositions();
    }

    public void EnterHandMode(InputMode _mode)
    {
        if(_mode == InputMode.hand)
        {
            DeleventSystem.handVisualsUpdate();
        }
    }

    public void ExitHandMode(InputMode _mode)
    {
        if (_mode == InputMode.hand)
        {
            DeleventSystem.handVisualsUpdate();
        }
    }

    public void UpdateHandCards()
    {
        if (cardVisPrefab == null)
            return;
        foreach (var card in handCards)
        {
            Destroy(card.gameObject);
        }
        handCards = new List<CardUnitDisplay>();
        foreach (var item in CardManager._instance.handCards)
        {
            GameObject go = Instantiate(cardVisPrefab);
            CardUnitDisplay temp = go.GetComponent<CardUnitDisplay>();
            temp.SetCardData(item);
            handCards.Add(temp);
        }
    }

    public void UpdateCardPositions()
    {
        if (handCards.Count <= 0)
            return;
        CheckHighlightCard();
        Camera camera = Camera.main;
        Vector3[] frustumCorners = new Vector3[4];
        camera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), camera.nearClipPlane, Camera.MonoOrStereoscopicEye.Mono, frustumCorners);
        
        if (InputManager._instance.currentMode != InputMode.hand)
        {
            for (int i = 0; i < handCards.Count; i++)
            {
                handCards[i].transform.position = GetCardPosition(i, passiveOffset);
            }
        }
        else
        {
            for (int i = 0; i < handCards.Count; i++)
            {
                handCards[i].transform.position = GetCardPosition(i, offset);
            }
            if(highlightedCard > -1 && (highlightedCard < handCards.Count))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    mouseOffset = Vector3.zero;
                    mouseOffset = GetCardPosition(highlightedCard, highlightOffset) - GetCardMousePosition(highlightedCard);
                }
                if (Input.GetMouseButton(0))
                {
                    handCards[highlightedCard].transform.position = GetCardMousePosition(highlightedCard);
                }
                else if (Input.GetMouseButtonUp(0) && mouseOffset != Vector3.zero && Input.mousePosition.y >= Screen.height / 2)
                {
                    if (handCards[highlightedCard].data.CheckPlayCondition())
                    {
                        handCards[highlightedCard].data.PlayFromHand();
                        return; //workaround to avoid playing multiple cards during the same step! Since card list gets modified by playing
                    }
                }
                else
                {
                    mouseOffset = Vector3.zero;
                    handCards[highlightedCard].transform.position = GetCardPosition(highlightedCard, highlightOffset);
                }          
            }
        }
    }

    public void UpdateCardHighlight()
    {
        if (GameManager._instance == null || GameManager._instance.activeUnit == null)
            return;
        foreach (var card in handCards)
        {
            card.highlighted = card.data.CheckPlayCondition();
        }
    }

    Vector3 GetCardPosition(int cardNum, Vector3 offset)
    {
        Camera camera = Camera.main;
        Vector3 result = new Vector3();
        Vector3 screenPoint = new Vector3( leftMargin + (cardNum + 1) * cardStepLength, 0, 0);
        Ray ray = camera.ScreenPointToRay(screenPoint);
        if (cardLayerSurface.Raycast(ray, out RaycastHit hit, 19) && hit.collider.gameObject.layer == 19)
        {
            result = hit.point;
        }
        return result + offset;
    }
    Vector3 GetCardMousePosition(int cardNum)
    {
        Camera camera = Camera.main;
        Vector3 result = new Vector3();
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (cardLayerSurface.Raycast(ray, out RaycastHit hit, 19) && hit.collider.gameObject.layer == 19)
        {
            result = hit.point;
        }
        return result + mouseOffset;
    }

    public void CheckHighlightCard()
    {
        int distancePx = (Screen.width - rightMargin) - (0 + leftMargin);
        cardStepLength = distancePx / (handCards.Count + 1);
        cardAreaStepLength = distancePx / handCards.Count;
        if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
        {
            return;
        }
        if (Input.mousePosition.x < leftMargin || Input.mousePosition.x > Screen.width - rightMargin)
        {
            highlightedCard = -1;
        }
        float result = (Input.mousePosition.x - leftMargin) / cardAreaStepLength;
        highlightedCard = Mathf.FloorToInt(result);
    }

    public bool CheckCardData(CardUnitDisplay card, int data)
    {
        if (card.id == data)
            return true;
        return false;
    }
}
