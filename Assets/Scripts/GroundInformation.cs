using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundInformation : MonoBehaviour
{
    public GroundFlags flags;
}

[System.Serializable, System.Flags]
public enum GroundFlags
{
    Walkable = 1 << 0,    // 0001
    SeeThrough = 1 << 1   // 0010
    // 1 << 2 isw.
};
