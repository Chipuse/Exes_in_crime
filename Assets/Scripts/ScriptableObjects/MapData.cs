using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct PositionKey
{
    public int x;
    public int y;

    public static bool operator ==(PositionKey c1, PositionKey c2)
    {
        return c1.Equals(c2);
    }

    public static bool operator !=(PositionKey c1, PositionKey c2)
    {
        return !c1.Equals(c2);
    }
}

public class InvalidKey
{
    public static PositionKey Key = new PositionKey { x = -9999, y = -9999 };
}

public class Tile
{
    public Tile(InfoForDelete[] _infoForDeletes, GroundFlags _flags, Direction _rotation = Direction.North)
    {
        rotation = _rotation;
        infoForDeletes = _infoForDeletes;
        flags = _flags;
    }
    public Direction rotation = Direction.North;
    public GroundFlags flags;

    public InfoForDelete[] infoForDeletes;
}

public class Wall
{
    public Wall(/*WallData _wallData,*/ InfoForDelete[] _infoForDeletes, Direction _rotation = Direction.North)
    {
        rotation = _rotation;
        infoForDeletes = _infoForDeletes;
        //data = _wallData;
    }
    public Direction rotation = Direction.North;
    //public WallData data;

    public InfoForDelete[] infoForDeletes;
}

public class MapInstance
{
    public MapInstance(MapData _mapData)
    {
        mapData = _mapData;
        RefreshMap();
    }
    public Dictionary<PositionKey, Tile> tiles = new Dictionary<PositionKey, Tile>();
    public Dictionary<PositionKey, Wall> walls = new Dictionary<PositionKey, Wall>();
    public Dictionary<PositionKey, int> securityLvl = new Dictionary<PositionKey, int>();
    public MapData mapData;
    public MapRenderer mapRenderer = new MapRenderer();

    public void RefreshSecurityLvl(SecurityLevelData newData)
    {
        if (securityLvl.ContainsKey(newData.positionKey))
        {
            securityLvl.Remove(newData.positionKey);
        }

        securityLvl.Add(newData.positionKey, newData.level);
    }
    public void DeleteSecurityLvl(PositionKey key)
    {
        if (securityLvl.ContainsKey(key))
        {
            securityLvl.Remove(key);
        }
    }
    public void RefreshTile(SerializableData newData)
    {
        if (tiles.ContainsKey(newData.positionKey))
        {
            mapRenderer.RemoveMesh(tiles[newData.positionKey].infoForDeletes);
            tiles.Remove(newData.positionKey);
        }
        GameObject prefab = Resources.Load(newData.assetPath) as GameObject;
        GameObject justForTransform = Resources.Load(newData.assetPath) as GameObject;
        //transform prefab to right position and rotation:
        //prefab.rotation ... TODO
        justForTransform.transform.position = MapManager._instance.transform.position + MapManager._instance.GroundGridPosToWorldPos(newData.positionKey);
        justForTransform.transform.rotation = MapManager._instance.transform.rotation;
        InfoForDelete[] infoForDeletes = mapRenderer.BuildEntryFromPrefab(prefab, MapManager._instance.transform);
        GroundFlags tempFlags = (GroundFlags)3; // current marker for all flags!
        GroundInformation gInfo = prefab.GetComponent<GroundInformation>();
        if (gInfo){
            tempFlags = gInfo.flags;
        }
        tiles.Add(newData.positionKey, new Tile(
            infoForDeletes,
            tempFlags,
            newData.direction));
    }
    public void DeleteTile(PositionKey key)
    {
        if (tiles.ContainsKey(key))
        {
            mapRenderer.RemoveMesh(tiles[key].infoForDeletes);
            tiles.Remove(key);
        }
    }
    public void RefreshWall(SerializableData newData)
    {
        if (walls.ContainsKey(newData.positionKey))
        {
            mapRenderer.RemoveMesh(walls[newData.positionKey].infoForDeletes);
            walls.Remove(newData.positionKey);
        }
        //walls.Add(newData.positionKey, new Wall(Resources.Load<WallData>(newData.assetPath), newData.direction));

        GameObject prefab = Resources.Load(newData.assetPath) as GameObject;
        GameObject justForTransform = Resources.Load(newData.assetPath) as GameObject;
        //transform prefab to right position and rotation:
        //prefab.rotation ... TODO
        
        justForTransform.transform.position = MapManager._instance.transform.position + MapManager._instance.WallGridPosToWorldPos(newData.positionKey);

        //workaround for walls:
        Quaternion rot = (Quaternion.identity);
        if (newData.positionKey.y % 2 == 0)
        {
            rot.SetLookRotation(Vector3.forward, Vector3.up);
        }
        else
        {
            rot.SetLookRotation(Vector3.right, Vector3.up);
        }
        justForTransform.transform.rotation = rot;
        InfoForDelete[] infoForDeletes = mapRenderer.BuildEntryFromPrefab(prefab, MapManager._instance.transform);
        walls.Add(newData.positionKey, new Wall(
            infoForDeletes,
            newData.direction));
    }
    public void DeleteWall(PositionKey key)
    {
        if (walls.ContainsKey(key))
        {
            mapRenderer.RemoveMesh(walls[key].infoForDeletes);
            walls.Remove(key);
        }
    }

    public void RefreshMap()
    {
        tiles = new Dictionary<PositionKey, Tile>();
        walls = new Dictionary<PositionKey, Wall>();
        securityLvl = new Dictionary<PositionKey, int>();
        mapRenderer.ClearRenderData();
        foreach (var tileItem in mapData.tiles)
        {
            RefreshTile(tileItem);
        }
        foreach (var wallItem in mapData.walls)
        {
            RefreshWall(wallItem);
        }
        foreach (var secLvl in mapData.secLvl)
        {
            RefreshSecurityLvl(secLvl);
        }
    }
}

[Serializable]
public struct SerializableData
{
    public PositionKey positionKey;
    public string assetPath;
    public Direction direction;
}

[Serializable]
public struct SecurityLevelData
{
    public PositionKey positionKey;
    public int level;
}

[CreateAssetMenu(fileName = "MapData", menuName = "ScriptableObjects/Map", order = 1)]
public class MapData : ScriptableObject
{
    [SerializeField]
    public List<SerializableData> walls;
    [SerializeField]
    public List<SerializableData> tiles;
    [SerializeField]
    public List<SecurityLevelData> secLvl;

    // Array 2d tiles
    // Array 2d walls
    //public Dictionary<PositionKey, Tile> tiles;
    //public Dictionary<PositionKey, Wall> walls;

    /*
------0/0----!-----0/1----!----0/2----
!  ________  !  _________ !  ________
1 |        | 1 |        | 1 |        |
0 |   0/0  | 1 |   0/1  | 2 |  0/2   |
! |________| ! |________| ! |________|
!-----2/0----!-----2/1----!----2/2----
!  ________  !  _________ !  ________
3 |        | 3 |        | 3 |        |
0 |   1/0  | 1 |   1/1  | 2 |  1/2   |
! |________| ! |________| ! |________|
!-----4/0----!-----4/1----!----4/2----
!  _________ ! __________ !  ________
5 |        | 5 |        | 5 |        |


    Walls to tile relation ^y >x:
    North/Up: xW = xT, yW = 2yT
    East/Right: xW = xT + 1, yW = 2yT + 1
    South/Down: xW = xT, yW = 2yT + 2
    West/Left: xW = xT, yW = 2yT + 1
     */
}
