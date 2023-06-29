using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelect : MonoBehaviour
{
    public static TargetSelect _instance;
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
    List<TargetSelectObject> ValidSelection = new List<TargetSelectObject>();
    public void StartTargetSelect(List<TargetSelectObject> targets)
    {
        ValidSelection = targets;
        //create marker effects to show where interactables are
    }

    public void MouseClick(PositionKey pos)
    {
        //open context for all obj for that pos
    }

    public void GeneralContext()
    {
        //open context for all obj
    }
}

public struct TargetSelectObject
{
    public PositionKey pos;
    public string objName;
    public BaseUnit unit;
}
