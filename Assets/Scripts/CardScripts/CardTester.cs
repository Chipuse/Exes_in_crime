using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTester : MonoBehaviour
{
    public static CardTester _instance;
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
        FetchCardList();
    }

    [SerializeField]
    public List<CardData> cards;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void FetchCardList()
    {
        cards = FetchCardListStatic();
    }

    public static List<CardData> FetchCardListStatic()
    {
        List<CardData> result = new List<CardData>();
        List<Dictionary<string, object>> data = CSVReader.Read("Cards/Card List 210621");

        for (var i = 0; i < data.Count; i++)
        {
            List<string> tempTypes = new List<string>();
            for (int types = 1; types <= 4; types++)
            {
                if (data[i]["Type" + types] is string && (string)data[i]["Type" + types] != "")
                    tempTypes.Add((string)data[i]["Type" + types]);
            }

            List<int> tempVars = new List<int>();
            for (int vars = 1; vars < 10; vars++)
            {
                if (data[i]["var" + vars] is int)
                    tempVars.Add((int)data[i]["var" + vars]);
            }
            CardData newData = new CardData { };
            /*s
            CardData newData = new CardData
            {
                ID = (int)data[i]["ID"],
                Name = (string)data[i]["Name"],
                Cost = (int)data[i]["Cost"],
                Int = (int)data[i]["Int"],
                Pwr = (int)data[i]["Pwr"],
                Chr = (int)data[i]["Chr"],
                Spd = (int)data[i]["Spd"],
                Slot = (string)data[i]["Slot"],
                Type = tempStuff,
                DescriptionText = (string)data[i]["DescriptionText"],
                FlavorText = (string)data[i]["FlavorText"],
                CardPool = tempStuff,
                Variables = tempNumStuff
            };*/
            newData.ID = (int)data[i]["ID"];

            newData.Name = (string)data[i]["Name"];

            newData.IlluName = (string)data[i]["Illustration"];

            if (data[i]["Cost"] is int)
                newData.Cost = (int)data[i]["Cost"];

            if ((string)data[i]["RequType"] == "INT")
                newData.RequType = RequTypes.Int;
            else if ((string)data[i]["RequType"] == "PWR")
                newData.RequType = RequTypes.Pwr;
            else if ((string)data[i]["RequType"] == "CHR")
                newData.RequType = RequTypes.Chr;
            else if ((string)data[i]["RequType"] == "SPD")
                newData.RequType = RequTypes.Spd;
            else if ((string)data[i]["RequType"] == "HP")
                newData.RequType = RequTypes.Hp;
            else if ((string)data[i]["RequType"] == "AP")
                newData.RequType = RequTypes.Ap;
            if (data[i]["RequAmount"] is int)
                newData.RequAmount = (int)data[i]["RequAmount"];

            if ((string)data[i]["Slot"] == "Event")
                newData.Slot = Slots.Event;
            else if ((string)data[i]["Slot"] == "Hand")
                newData.Slot = Slots.Hand;
            else if ((string)data[i]["Slot"] == "Body")
                newData.Slot = Slots.Body;
            else if ((string)data[i]["Slot"] == "Companion")
                newData.Slot = Slots.Companion;
            else if ((string)data[i]["Slot"] == "Collectable")
                newData.Slot = Slots.Collectable;

            if ((string)data[i]["CardPool1"] == "Neutral")
                newData.CardPool1 = CardPools.Neutral;
            else if ((string)data[i]["CardPool1"] == "Phib")
                newData.CardPool1 = CardPools.Phib;
            else if ((string)data[i]["CardPool1"] == "Sam")
                newData.CardPool1 = CardPools.Sam;
            else if ((string)data[i]["CardPool1"] == "Kero")
                newData.CardPool1 = CardPools.Kero;
            else if ((string)data[i]["CardPool1"] == "Jeanne")
                newData.CardPool1 = CardPools.Jeanne;
            else if ((string)data[i]["CardPool1"] == "Vin")
                newData.CardPool1 = CardPools.Vin;

            if ((string)data[i]["CardPool2"] == "Neutral")
                newData.CardPool2 = CardPools.Neutral;
            else if ((string)data[i]["CardPool2"] == "Phib")
                newData.CardPool2 = CardPools.Phib;
            else if ((string)data[i]["CardPool2"] == "Sam")
                newData.CardPool2 = CardPools.Sam;
            else if ((string)data[i]["CardPool2"] == "Kero")
                newData.CardPool2 = CardPools.Kero;
            else if ((string)data[i]["CardPool2"] == "Jeanne")
                newData.CardPool2 = CardPools.Jeanne;
            else if ((string)data[i]["CardPool2"] == "Vin")
                newData.CardPool2 = CardPools.Vin;


            //Unlock
            if (data[i]["Unlock"] is int)
                newData.Unlock = (int)data[i]["Unlock"];
            else
                newData.Unlock = 0;
            //Shop
            if (data[i]["Shop"] is int)
                newData.Shop = (int)data[i]["Shop"];
            else
                newData.Shop = 0;

            newData.Type = tempTypes.ToArray();
            newData.DescriptionText = (string)data[i]["DescriptionText"];
            newData.FlavorText = (string)data[i]["FlavorText"];
            newData.Variables = tempVars.ToArray();
            result.Add(newData);
        }
        return result;
    }
    public static CardData GetCardByID(int _id)
    {
        foreach (var item in cardDataBase.cards)
        {
            if(_id == item.ID)
            {
                return item;
            }
        }
        return new CardData { };
    }

    public static CardData GetCardByID(string _id)
    {
        int tempID = ParseID(_id);
        foreach (var item in cardDataBase.cards)
        {
            if (tempID == item.ID)
            {
                return item;
            }
        }
        return new CardData { };
    }

    public static CardObj GetCardObjByID(int _id)
    {
        foreach (var item in cardDataBase.cardObjs)
        {
            if (_id == item.data.ID)
            {
                return item;
            }
        }
        return new CardObj { };
    }

    public static CardObj GetCardObjByID(string _id)
    {
        int tempID = ParseID(_id);
        foreach (var item in cardDataBase.cardObjs)
        {
            if (tempID == item.data.ID)
            {
                return item;
            }
        }
        return new CardObj { };
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public static int ParseID(string id)
    {
        int result;
        if(int.TryParse(id, out result))
        {
            return result;
        }
        return -1;
    }

    public static string ParseID(int id)
    {
        if(id < 0)
        {
            return id.ToString();
        }
        string result = "";
        if(id < 10)
        {
            result += "00";
        }
        else if(id < 100)
        {
            result += "0";
        }
        result += id.ToString();
        return result;
    }

    public static string ParseRequType(RequTypes type)
    {
        switch (type)
        {
            case RequTypes.Int:
                return "INT";
            case RequTypes.Pwr:
                return "PWR";
            case RequTypes.Chr:
                return "CHR";
            case RequTypes.Spd:
                return "SPD";
            case RequTypes.Hp:
                return "HP";
            case RequTypes.Ap:
                return "AP";
            case RequTypes.Invalid:
                return "Invalid";
            default:
                return "Invalid";
        }
    }

    public static RequTypes ParseRequType(string type)
    {
        if (type == "INT")
            return RequTypes.Int;
        else if (type == "PWR")
            return RequTypes.Pwr;
        else if (type == "CHR")
            return RequTypes.Chr;
        else if (type == "SPD")
            return RequTypes.Spd;
        else if (type == "HP")
            return RequTypes.Hp;
        else if (type == "AP")
            return RequTypes.Ap;
        return RequTypes.Invalid;
    }

    public static string ParseRequType(CardPools pool)
    {
        switch (pool)
        {
            case CardPools.Vin:
                return "Vin";
            case CardPools.Phib:
                return "Phib";
            case CardPools.Sam:
                return "Sam";
            case CardPools.Kero:
                return "Kero";
            case CardPools.Jeanne:
                return "Jeanne";
            case CardPools.Neutral:
                return "Neutral";
            case CardPools.Invalid:
                return "Invalid";
            default:
                return "Invalid";
        }
    }

    public static Slots ParseSlotType(string slot)
    {
        if (slot == "Event")
            return Slots.Event;
        else if (slot == "Hand")
            return Slots.Hand;
        else if (slot == "Body")
            return Slots.Body;
        else if (slot == "Companion")
            return Slots.Companion;
        else if (slot == "Collectable")
            return Slots.Collectable;
        return Slots.Invalid;
    }

    public static string ParseSlotType(Slots slot)
    {
        switch (slot)
        {
            case Slots.Event:
                return "Event";
            case Slots.Hand:
                return "Hand";
            case Slots.Body:
                return "Body";
            case Slots.Companion:
                return "Companion";
            case Slots.Collectable:
                return "Collectable";
            case Slots.Invalid:
                return "Invalid";
            default:
                return "Invalid";
        }
    }

    public static CardPools ParseCardPool(string pool)
    {
        if (pool == "Neutral")
            return CardPools.Neutral;
        else if (pool == "Vin")
            return CardPools.Vin;
        else if (pool == "Phib")
            return CardPools.Phib;
        else if (pool == "Sam")
            return CardPools.Sam;
        else if (pool == "Kero")
            return CardPools.Kero;
        else if (pool == "Jeanne")
            return CardPools.Jeanne;
        return CardPools.Invalid;
    }

    public static bool CheckRequForUnit(CardData _data, PlayerUnit _unit)
    {
        if(_data.RequAmount <= 0)
        {
            return true;
        }
        switch (_data.RequType)
        {
            case RequTypes.Int:
                if (_unit.CurrInt >= _data.RequAmount)
                    return true;
                break;
            case RequTypes.Pwr:
                if (_unit.CurrAtk >= _data.RequAmount)
                    return true;
                break;
            case RequTypes.Chr:
                if (_unit.CurrCha >= _data.RequAmount)
                    return true;
                break;
            case RequTypes.Spd:
                if (_unit.CurrMove >= _data.RequAmount)
                    return true;
                break;
            case RequTypes.Hp:
                if (_unit.CurrHealth >= _data.RequAmount)
                    return true;
                break;
            case RequTypes.Ap:
                if (_unit.CurrAP >= _data.RequAmount)
                    return true;
                break;
            case RequTypes.Invalid:
                return false;
            default:
                break;
        }
        return false;
    }

    public static GameObject GetPrefabByData(CardData _data)
    {
        string cardFolderPath = "Cards/CardObjs";
        GameObject prefab = Resources.Load(cardFolderPath + "/Card" + ParseID(_data.ID)) as GameObject;
        if(prefab == null)
        {
            Debug.LogWarning("For Card" + ParseID(_data.ID) + " " + _data.Name + " does not exist a individual prefab yet! Trying to load a default prefab instead");
            switch (_data.Slot)
            {
                case Slots.Event:
                    prefab = Resources.Load(cardFolderPath + "/Defaults/DefaultEvent") as GameObject;
                    break;
                case Slots.Hand:
                    prefab = Resources.Load(cardFolderPath + "/Defaults/DefaultHand") as GameObject;
                    break;
                case Slots.Body:
                    prefab = Resources.Load(cardFolderPath + "/Defaults/DefaultBody") as GameObject;
                    break;
                case Slots.Companion:
                    prefab = Resources.Load(cardFolderPath + "/Defaults/DefaultCompanion") as GameObject;
                    break;
                case Slots.Collectable:
                    prefab = Resources.Load(cardFolderPath + "/Defaults/DefaultCollectable") as GameObject;
                    break;
            }
            if(prefab == null)
            {
                prefab = Resources.Load(cardFolderPath + "/Defaults/DefaultBasis") as GameObject;
            }
        }
        if(prefab == null)
        {
            Debug.LogError("No CardPrefab Found!!!");
        }
        return prefab;
    }

    static CardDatabase _cardDataBase;
    public static CardDatabase cardDataBase
    {
        get 
        {
            if(_cardDataBase == null)
                FetchCardDataBase();
            return _cardDataBase;
        }
        set { _cardDataBase = value; }
    }

    public static void FetchCardDataBase()
    {
        _cardDataBase = Resources.Load("Cards/CardDatabase") as CardDatabase;
    }

    public static BaseCardScript CreateCardObj(int _id)
    {
        if (GetCardObjByID(_id).prefab == null)
            _cardDataBase.ReloadCardData();
        GameObject instance = Instantiate(GetCardObjByID(_id).prefab);
        BaseCardScript objScript = instance.GetComponent<BaseCardScript>();
        BaseCardScript.UniqueIDCounter++;
        objScript.UniqueID = BaseCardScript.UniqueIDCounter;
        objScript.data = GetCardByID(_id);
        return objScript;
    }
    public static BaseCardScript CreateCardObj(string _id)
    {
        if (GetCardObjByID(_id).prefab == null)
            _cardDataBase.ReloadCardData();
        GameObject instance = Instantiate(GetCardObjByID(_id).prefab);
        BaseCardScript objScript = instance.GetComponent<BaseCardScript>();
        BaseCardScript.UniqueIDCounter++;
        objScript.UniqueID = BaseCardScript.UniqueIDCounter;
        objScript.data = GetCardByID(_id);
        return objScript;
    }

    public static Sprite GetIlluByName(string name)
    {
        Sprite tempSprite = Resources.Load<Sprite>("Cards/Art/Illustrations/Illustration_" + name);
        if (tempSprite == null)
            Debug.LogError("Illustration_" + name + " was not found");
        return tempSprite;
    }

    public static Sprite GetIlluByID(int id)
    {
        return GetIlluByName(GetCardObjByID(id).data.IlluName);
    }
}

public enum CardPools
{
    Neutral,
    Vin,
    Phib,
    Sam,
    Kero,
    Jeanne,
    Invalid
}

public enum Slots
{
    Event,
    Hand,
    Body,
    Companion,
    Collectable,
    Invalid
}

public enum RequTypes
{
    Int,
    Pwr,
    Chr,
    Spd,
    Hp,
    Ap,
    Invalid
}
