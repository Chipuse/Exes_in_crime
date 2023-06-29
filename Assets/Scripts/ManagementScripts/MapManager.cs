using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[ExecuteAlways]
public class MapManager : MonoBehaviour
{
    public static MapManager _instance;
    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(_instance);
            _instance = this;
        }
        if(_mapData)
            mapInstance = new MapInstance(_mapData);
    }

    public float wallHeight;
    public float unitHeight;


    public Collider mapSurface;
    public MapInstance mapInstance;
    [SerializeField]
    public MapData _mapData;
    [Range(1.0f, 2.0f)]
    public float spacing = 1.0f;

    public void OnValidate()
    {
        
        if (_instance == null)
        {
            _instance = this;
        }
        else if( _instance != this)
        {
            //Destroy(_instance);
            _instance = this;
        }
        if (mapInstance == null)
            return;
        mapInstance.RefreshMap();
    }

    public void Update()
    {
        if(mapInstance != null)
        {
            mapInstance.mapRenderer.DrawInstancedMeshes();
        }
    }

    public PositionKey WorldPosToGroundGridPos(Vector3 _worldPos)
    {
        Vector3 gridPos = _worldPos - transform.position;
        gridPos.x /= spacing;
        gridPos.z /= spacing;
        return new PositionKey { x = Mathf.RoundToInt(gridPos.x), y = Mathf.RoundToInt(gridPos.z) };
    }

    public Vector3 GroundGridPosToWorldPos(PositionKey _gridPos)
    {
        return new Vector3(_gridPos.x * spacing, 0, _gridPos.y * spacing) + transform.position;
    }

    public PositionKey WorldPosToWallGridPos(Vector3 _worldPos)
    {
        Vector3 gridPos = _worldPos - transform.position;
        gridPos.x /= spacing;
        gridPos.z /= spacing;
        float deltaX = Mathf.Abs(gridPos.x % 1);
        if(gridPos.x < 0)
            deltaX = Mathf.Abs(deltaX - 1);
        float deltaY = Mathf.Abs(gridPos.z % 1);
        if (gridPos.z < 0)
            deltaY = Mathf.Abs(deltaY - 1);
        Direction dir;

        if(deltaY <= 0.5f)
        {
            //we are on the north half
            if(deltaX <= 0.5f)
            {
                //we are on the east half
                if(deltaX >= deltaY)
                {
                    dir = Direction.East;
                }
                else
                {
                    dir = Direction.North;
                }
            }
            else
            {
                //we are on the west side
                if (1 - deltaX >= deltaY)
                {
                    dir = Direction.West;
                }
                else
                {
                    dir = Direction.North;
                }
            }
        }
        else
        {
            //we are on the south half
            if (deltaX <= 0.5f)
            {
                //we are on the east half
                if (deltaX >= 1 - deltaY)
                {
                    dir = Direction.East;
                }
                else
                {
                    dir = Direction.South;
                }
            }
            else
            {
                //we are on the west side
                if (1 - deltaX >= 1 - deltaY)
                {
                    dir = Direction.West;
                }
                else
                {
                    dir = Direction.South;
                }
            }
        }
        return GetWallOfTileByDirection(WorldPosToGroundGridPos(_worldPos), dir);
    }

    public Vector3 WallGridPosToWorldPos(PositionKey _gridPos)
    {
        Vector3 pos = Vector3.zero;
        if (_gridPos.y % 2 == 0)
        {
            pos.z -= 0.5f;
            pos.z += _gridPos.y / 2;
            pos.x += _gridPos.x;
        }
        else
        {
            pos.x -= 0.5f;
            pos.z += _gridPos.y / 2;
            pos.x += _gridPos.x;
        }
        pos.x *= spacing;
        pos.z *= spacing;
        pos.y = 0.5f;
        return pos + transform.position;
    }

    public PositionKey GetWallOfTileByDirection(PositionKey _groundTilePos, Direction _wallDirection)
    {
        PositionKey wallPos = new PositionKey { };
        switch (_wallDirection)
        {
            case Direction.North:
                wallPos.x = _groundTilePos.x;
                wallPos.y = _groundTilePos.y * 2 + 2;
                //Debug.Log("north");
                break;
            case Direction.East:
                wallPos.x = _groundTilePos.x + 1;
                if(_groundTilePos.y < 0)
                    wallPos.y = _groundTilePos.y * 2 - 1;
                else
                    wallPos.y = _groundTilePos.y * 2 + 1;
                //Debug.Log("east");
                break;
            case Direction.South:
                wallPos.x = _groundTilePos.x;
                wallPos.y = _groundTilePos.y * 2;
                //Debug.Log("south");
                break;
            case Direction.West:
                wallPos.x = _groundTilePos.x;
                wallPos.y = _groundTilePos.y * 2;
                if (_groundTilePos.y < 0)
                    wallPos.y = _groundTilePos.y * 2 - 1;
                else
                    wallPos.y = _groundTilePos.y * 2 + 1;
                //Debug.Log("west");
                break;
            default:
                break;
        }
        return wallPos;
    }

    public PositionKey[] GetAllWallsAroundTile(PositionKey _groundTilePos)
    {
        List<PositionKey> result = new List<PositionKey>();
        result.Add(GetWallOfTileByDirection(_groundTilePos, Direction.North));
        result.Add(GetWallOfTileByDirection(_groundTilePos, Direction.East));
        result.Add(GetWallOfTileByDirection(_groundTilePos, Direction.South));
        result.Add(GetWallOfTileByDirection(_groundTilePos, Direction.West));
        return result.ToArray();
    }

    public PositionKey GetTileAroundWall(PositionKey _wallPos, Direction _dir)
    {
        PositionKey result = InvalidKey.Key;
        if(_wallPos.y % 2 == 0)
        {
            //north south wall
            if(_dir == Direction.North || _dir ==  Direction.East)
            {
                result.x = _wallPos.x;
                result.y = (_wallPos.y - 2) / 2;
            }
            else
            {
                result.x = _wallPos.x;
                result.y = (_wallPos.y) / 2;
            }
        }
        else
        {
            //east west wall
            if (_dir == Direction.North || _dir == Direction.East)
            {
                result.x = _wallPos.x - 1;
                if(_wallPos.y < 0)
                {
                    result.y = (_wallPos.y + 1) / 2;
                }
                else
                {
                    result.y = (_wallPos.y - 1) / 2;
                }
            }
            else
            {
                result.x = _wallPos.x;
                if(_wallPos.y < 0)
                {
                    result.y = (_wallPos.y + 1) / 2;
                }
                else
                {
                    result.y = (_wallPos.y - 1) / 2;
                }
            }
        }
        return result;
    }

    public PositionKey[] GetAllTilesAroundWall(PositionKey _wallPos)
    {
        List<PositionKey> result = new List<PositionKey>();
        result.Add(GetTileAroundWall(_wallPos, Direction.North));
        result.Add(GetTileAroundWall(_wallPos, Direction.South));
        return result.ToArray();
    }

    public Tile GetTileByGridPos(PositionKey pos)
    {
        return mapInstance.tiles[pos];
    }
    public PositionKey GetGridPosByTile(Tile tile)
    {
        foreach (var item in mapInstance.tiles)
        {
            if (item.Value == tile)
            {
                return item.Key;
            }
        }
        Debug.LogError("Map Does not Contain Searched Tile!");
        return InvalidKey.Key;
    }

    public Wall GetWallByGridPos(PositionKey pos)
    {
        if (mapInstance.walls.ContainsKey(pos))
            return mapInstance.walls[pos];
        else
            return null;
    }
    public PositionKey GetGridPosByWall(Wall wall)
    {
        foreach (var item in mapInstance.walls)
        {
            if (item.Value == wall)
            {
                return item.Key;
            }
        }
        Debug.LogError("Map Does not Contain Searched Tile!");
        return InvalidKey.Key;
    }

    public Tile GetAdjacentTile(Tile origin, Direction direction)
    {
        PositionKey originPos = GetGridPosByTile(origin);
        switch (direction)
        {
            case Direction.North:
                return mapInstance.tiles[new PositionKey { x = originPos.x, y = originPos.y + 1 }];
            case Direction.East:
                return mapInstance.tiles[new PositionKey { x = originPos.x + 1, y = originPos.y}];
            case Direction.South:
                return mapInstance.tiles[new PositionKey { x = originPos.x, y = originPos.y - 1}];
            case Direction.West:
                return mapInstance.tiles[new PositionKey { x = originPos.x - 1, y = originPos.y}];
            default:
                break;
        }
        return null;
    }

    public Tile GetAdjacentTileByPositionKey(PositionKey originPos, Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return mapInstance.tiles[new PositionKey { x = originPos.x, y = originPos.y + 1 }];
            case Direction.East:
                return mapInstance.tiles[new PositionKey { x = originPos.x + 1, y = originPos.y }];
            case Direction.South:
                return mapInstance.tiles[new PositionKey { x = originPos.x, y = originPos.y - 1 }];
            case Direction.West:
                return mapInstance.tiles[new PositionKey { x = originPos.x - 1, y = originPos.y }];
            default:
                break;
        }
        return null;
    }

    public PositionKey GetAdjacentTilesPosKeyByPositionKey(PositionKey originPos, Direction direction)
    {
        PositionKey tempKey = InvalidKey.Key;
        switch (direction)
        {
            case Direction.North:
                tempKey = new PositionKey { x = originPos.x, y = originPos.y + 1 };
                break;
            case Direction.East:
                tempKey = new PositionKey { x = originPos.x + 1, y = originPos.y };
                break;
            case Direction.South:
                tempKey = new PositionKey { x = originPos.x, y = originPos.y - 1 };
                break;
            case Direction.West:
                tempKey = new PositionKey { x = originPos.x - 1, y = originPos.y };
                break;
            default:
                break;
        }
        if (mapInstance.tiles.ContainsKey(tempKey))
            return tempKey;
        //Debug.LogError("Something went horrible wrong! lol?");
        return InvalidKey.Key;
    }

    public PositionKey GetAdjacentPosKeyByPositionKey(PositionKey originPos, Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return new PositionKey { x = originPos.x, y = originPos.y + 1 };
            case Direction.East:
                return new PositionKey { x = originPos.x + 1, y = originPos.y };
            case Direction.South:
                return new PositionKey { x = originPos.x, y = originPos.y - 1 };
            case Direction.West:
                return new PositionKey { x = originPos.x - 1, y = originPos.y };
            default:
                break;
        }
        Debug.LogError("Something went horrible wrong! lol?");
        return InvalidKey.Key;
    }

    public PositionKey[] GetAllTilesAroundTile(PositionKey _groundTilePos)
    {
        List<PositionKey> result = new List<PositionKey>();
        result.Add(GetAdjacentPosKeyByPositionKey(_groundTilePos, Direction.North));
        result.Add(GetAdjacentPosKeyByPositionKey(_groundTilePos, Direction.East));
        result.Add(GetAdjacentPosKeyByPositionKey(_groundTilePos, Direction.South));
        result.Add(GetAdjacentPosKeyByPositionKey(_groundTilePos, Direction.West));
        return result.ToArray();
    }

    public int GetSecurityLevel(PositionKey _pos)
    {
        if (mapInstance == null)
            return 0;
        if (mapInstance.securityLvl.ContainsKey(_pos))
            return mapInstance.securityLvl[_pos];
        return 0;
    }
}

public enum Direction { North, East, South, West };
