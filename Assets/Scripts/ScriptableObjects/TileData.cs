using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileData", menuName = "ScriptableObjects/Tile", order = 1)]
public class TileData : ScriptableObject
{
    //Tile ID
    //Position
    //Rotation
    //"tileSkin" -> path to resource folder + relative location of tile prefab
    public string tileResourcePath;
    public GameObject tilePrefab;
    public Mesh skin;
    public Material material;
    //Security Lvl
    //movement cost/type
}
