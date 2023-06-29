using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour, ISerializableUnit
{
    public static CardManager _instance;
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

    private void OnEnable()
    {
        deckCards = new List<BaseCardScript>();
        foreach (var cardID in startDeckCards)
        {
            deckCards.Add(CardTester.CreateCardObj(cardID));
        }
        handCards = new List<BaseCardScript>();
        foreach (var cardID in startHandCards)
        {
            handCards.Add(CardTester.CreateCardObj(cardID));
        }
        discardCards = new List<BaseCardScript>();
        foreach (var cardID in startDiscardCards)
        {
            discardCards.Add(CardTester.CreateCardObj(cardID));
        }
    }

    public void SetupSessionCards(List<int> _startDeckCards, List<int> _startHandCards, List<int> _startDiscardCards)
    {
        startDeckCards = _startDeckCards;
        startHandCards = _startHandCards;
        startDiscardCards = _startDiscardCards;

        deckCards = new List<BaseCardScript>();
        foreach (var cardID in startDeckCards)
        {
            deckCards.Add(CardTester.CreateCardObj(cardID));
        }
        handCards = new List<BaseCardScript>();
        foreach (var cardID in startHandCards)
        {
            handCards.Add(CardTester.CreateCardObj(cardID));
        }
        discardCards = new List<BaseCardScript>();
        foreach (var cardID in startDiscardCards)
        {
            discardCards.Add(CardTester.CreateCardObj(cardID));
        }

        ShuffleDeck();
        DrawStartHand();
    }

    void ShuffleDeck()
    {
        for (int i = 0; i < deckCards.Count; i++)
        {
            BaseCardScript temp = deckCards[i];
            int randomIndex = Random.Range(i, deckCards.Count);
            deckCards[i] = deckCards[randomIndex];
            deckCards[randomIndex] = temp;
        }
    }

    void DrawStartHand()
    {
        int startHandCards = 3;
        while (handCards.Count < startHandCards && deckCards.Count > 0)
        {
            DrawTopCard();
        }
    }

    public void DrawTopCard()
    {
        //edge case of too many handcards! ToDo
        if (deckCards.Count <= 0)
            return;
        BaseCardScript temp = deckCards[0];
        deckCards.RemoveAt(0);
        handCards.Add(temp);
        CardVisHand._instance.UpdateHandCards();
    }

    public List<int> startDeckCards = new List<int>();
    public List<int> startHandCards = new List<int>();
    public List<int> startDiscardCards = new List<int>();
    //[HideInInspector]
    public List<BaseCardScript> deckCards = new List<BaseCardScript>();
    //[HideInInspector]
    public List<BaseCardScript> handCards = new List<BaseCardScript>();
    //[HideInInspector]
    public List<BaseCardScript> discardCards = new List<BaseCardScript>();
    // Start is called before the first frame update

    public virtual SerializedDataContainer Serialize()
    {
        SerializedDataContainer container = new SerializedDataContainer();
        container.type = GetSerializableType();
        SerializedDataContainer deckContainer = new SerializedDataContainer();
        foreach (var item in deckCards)
        {
            deckContainer.Serialize(item.data.ID);
        }
        container.Serialize(deckContainer);
        SerializedDataContainer handContainer = new SerializedDataContainer();
        foreach (var item in handCards)
        {
            handContainer.Serialize(item.data.ID);
        }
        container.Serialize(handContainer);
        SerializedDataContainer discardContainer = new SerializedDataContainer();
        foreach (var item in discardCards)
        {
            discardContainer.Serialize(item.data.ID);
        }
        container.Serialize(discardContainer);
        return container;
    }

    public virtual void Deserialize(SerializedDataContainer input)
    {
        foreach (var item in discardCards)
        {
            Destroy(item.gameObject);
        } 
        foreach (var item in handCards)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in discardCards)
        {
            Destroy(item.gameObject);
        }
        deckCards = new List<BaseCardScript>();
        foreach (var cardID in input.GetFirstMember().ints)
        {
            deckCards.Add(CardTester.CreateCardObj(cardID));
        }
        handCards = new List<BaseCardScript>();
        foreach (var cardID in input.GetFirstMember().ints)
        {
            handCards.Add(CardTester.CreateCardObj(cardID));
        }
        discardCards = new List<BaseCardScript>();
        foreach (var cardID in input.GetFirstMember().ints)
        {
            discardCards.Add(CardTester.CreateCardObj(cardID));
        }
        CardVisHand._instance.UpdateHandCards();
    }

    public virtual SerializableClasses GetSerializableType()
    {
        return SerializableClasses.cardManager;
    }
}
