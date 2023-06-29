using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//[Serializable]
public class SerializedDataContainer
{
    public SerializableClasses type = SerializableClasses.undef;
    public string prefabPath = "";
    public List<PositionKey> posKeys = new List<PositionKey>();
    public List<SerializedDataContainer> members = new List<SerializedDataContainer>();
    public List<bool> bools= new List<bool>();
    public List<string> strings = new List<string>();
    public List<int> ints = new List<int>();
    public List<float> floats = new List<float>();

    public void Serialize(PositionKey value) { posKeys.Add(value); }
    public void Serialize(SerializedDataContainer value) { members.Add(value); }
    public void Serialize(bool value) { bools.Add(value); }
    public void Serialize(string value) { strings.Add(value); }
    public void Serialize(int value) { ints.Add(value); }
    public void Serialize(float value) { floats.Add(value); }
    public void Serialize(ISerializableUnit value) { Serialize(value.Serialize()); }

    public PositionKey GetFirstPosKey()
    {
        PositionKey result = posKeys[0];
        posKeys.RemoveAt(0);
        return result;
    }
    public SerializedDataContainer GetFirstMember()
    {
        SerializedDataContainer result = members[0];
        members.RemoveAt(0);
        return result;
    }
    public bool GetFirstBool()
    {
        bool result = bools[0];
        bools.RemoveAt(0);
        return result;
    }

    public string GetFirstString()
    {
        string result = strings[0];
        strings.RemoveAt(0);
        return result;
    }
    public int GetFirstInt()
    {
        int result = ints[0];
        ints.RemoveAt(0);
        return result;
    }

    public float GetFirstFloat()
    {
        float result = floats[0];
        floats.RemoveAt(0);
        return result;
    }
}
