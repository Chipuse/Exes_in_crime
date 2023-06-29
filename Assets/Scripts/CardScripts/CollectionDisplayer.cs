using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionDisplayer : MonoBehaviour
{
    public static CollectionDisplayer _instance;
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
        UpdateDisplayCards();
    }

    public GameObject cardVisPrefab;
    public int highlightedCard;
    public int leftMargin;
    public int rightMargin;
    public int topMargin;
    public int bottomMargin;
    public float scrollOffset;
    public List<int> startCardsToDisplay = new List<int>();
    public List<BaseCardScript> cardsToDisplay = new List<BaseCardScript>();
    List<CardUnitDisplay> displayCards = new List<CardUnitDisplay>();
    public float cardStepLength;
    public float cardAreaStepLength;
    public Collider cardLayerSurface;
    Vector3 mouseOffset = new Vector3(0, 0, 0);
    public Vector3 highlightedOffset = new Vector3(0, 0, 0);
    private void OnEnable()
    {
        cardsToDisplay = new List<BaseCardScript>();
        foreach (var cardID in startCardsToDisplay)
        {
            cardsToDisplay.Add(CardTester.CreateCardObj(cardID));
        }
        //cardsToDisplay = CardManager._instance.discardCards;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCardPositions();
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            scrollOffset -= 1 * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            scrollOffset += 1 * Time.deltaTime;
        }
    }

    public void UpdateDisplayCards()
    {
        if (cardVisPrefab == null)
            return;
        foreach (var card in displayCards)
        {
            Destroy(card.gameObject);
        }
        displayCards = new List<CardUnitDisplay>();
        foreach (var item in cardsToDisplay)
        {
            GameObject go = Instantiate(cardVisPrefab);
            CardUnitDisplay temp = go.GetComponent<CardUnitDisplay>();
            temp.SetCardData(item);
            displayCards.Add(temp);
        }
        UpdateCardPositions();
    }

    public void UpdateCardPositions()
    {
        if (displayCards.Count <= 0)
            return;
        CheckHighlightCard();
        Camera camera = Camera.main;

        for (int i = 0; i < displayCards.Count; i++)
        {
            displayCards[i].transform.position = GetCardPosition(i);
        }
        if (highlightedCard > -1 && (highlightedCard < displayCards.Count))
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseOffset = Vector3.zero;
                mouseOffset = GetCardPosition(highlightedCard) - GetCardMousePosition(highlightedCard);
            }
            if (Input.GetMouseButton(0))
            {
                displayCards[highlightedCard].transform.position = GetCardMousePosition(highlightedCard);
            }
            else
            {
                displayCards[highlightedCard].transform.position = GetCardPosition(highlightedCard) + highlightedOffset;
            }
        }

    }

    Vector3 GetCardPosition(int cardNum)
    {
        Camera camera = Camera.main;
        Vector3 result = new Vector3();
        Vector3 screenPoint = new Vector3(Screen.width/2, Screen.height/2, 0);
        Ray ray = camera.ScreenPointToRay(screenPoint);
        if (cardLayerSurface.Raycast(ray, out RaycastHit hit, 19) && hit.collider.gameObject.layer == 19)
        {
            result = hit.point;
        }
        return result + Vector3.right * (scrollOffset + (cardNum + 1) * cardStepLength);
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
        //cardStepLength = distancePx / (displayCards.Count + 1);
        //cardAreaStepLength = distancePx / displayCards.Count;
        if (Input.GetMouseButton(0))
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
