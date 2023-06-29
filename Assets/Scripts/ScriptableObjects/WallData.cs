using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WallData", menuName = "ScriptableObjects/Wall", order = 1)]
public class WallData : ScriptableObject
{
    //Wall ID
    //Position
    //Rotation
    //"wallSkin"
    public string tileResourcePath;
    public GameObject tilePrefab;
    public Mesh skin;
    public Material material;
}
