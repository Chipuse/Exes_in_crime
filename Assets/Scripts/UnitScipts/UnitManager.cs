using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour, ISerializableUnit
{
    public static UnitManager _instance;
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


    //these two lists just have to be serialized ahhhhhhhhhhhhhhhh
    public Dictionary<PositionKey, List<BaseUnit>> units = new Dictionary<PositionKey, List<BaseUnit>>();
    public Dictionary<PositionKey, WallUnit> wallUnits = new Dictionary<PositionKey, WallUnit>();
    //Card unit list for deck and hand

    public bool CheckTileSeeThrough(PositionKey tilePos)
    {
        if (units.ContainsKey(tilePos))
        {
            foreach (var item in units[tilePos])
            {
                if (!item.seeThrough)
                    return false;
            }
            return true;
        }
        return true;
    }

    public bool CheckTileOccupied(PositionKey tilePos)
    {
        if (units.ContainsKey(tilePos))
        {
            foreach (var item in units[tilePos])
            {
                if (item.occupying)
                    return true;
            }
            return false;
        }
        return false;
    }

    public bool CheckTileAccess(PositionKey tilePos, bool player)
    {
        if (units.ContainsKey(tilePos))
        {
            if (player)
            {
                foreach (var item in units[tilePos])
                {
                    if (!item.playerOpen)
                        return false;
                }
                return true;
            }
            else
            {
                foreach (var item in units[tilePos])
                {
                    if (!item.enemyOpen)
                        return false;
                }
                return true;
            }
        }
        return false;
    }
    public bool CheckWallAccess(PositionKey wallPos, bool player)
    {
        if (wallUnits.ContainsKey(wallPos))
        {
            if (player)
            {
                if (wallUnits[wallPos].playerOpen)
                    return true;
            }
            else
            {
                if (wallUnits[wallPos].enemyOpen)
                    return true;
            }
        }
        return false;
    }

    public bool CheckAttack(PositionKey origin)
    {
        //Check all hackable units if they are reached by this tile
        foreach (var tile in units)
        {
            foreach (var unit in tile.Value)
            {
                if (unit is IAttackable)
                {
                    IAttackable temp = (IAttackable)unit;
                    if (temp.Attackable() && CheckReachType(temp.GetAttackReachType(), unit.position, origin))
                        return true;
                }
            }
        }

        foreach (var wall in wallUnits)
        {
            if (wall.Value is IAttackable)
            {
                IAttackable temp = (IAttackable)wall.Value;
                if (temp.Attackable() && CheckReachType(temp.GetAttackReachType(), wall.Value.position, origin))
                    return true;
            }
        }
        return false;
    }

    public bool CheckHack(PositionKey origin)
    {
        //Check all hackable units if they are reached by this tile
        foreach (var tile in units)
        {
            foreach (var unit in tile.Value)
            {
                if (unit is IHackable)
                {
                    IHackable temp = (IHackable)unit;
                    if (temp.Hackable() && CheckReachType(temp.GetHackReachType(), unit.position, origin))
                        return true;
                }
            }
        }

        foreach (var wall in wallUnits)
        {
            if (wall.Value is IHackable)
            {
                IHackable temp = (IHackable)wall.Value;
                if (temp.Hackable() && CheckReachType(temp.GetHackReachType(), wall.Value.position, origin))
                    return true;
            }
        }
        return false;
    }

    public bool CheckLoot(PositionKey origin)
    {
        //Check all hackable units if they are reached by this tile
        foreach (var tile in units)
        {
            foreach (var unit in tile.Value)
            {
                if (unit is ILootable)
                {
                    ILootable temp = (ILootable)unit;
                    if (temp.Lootable() && CheckReachType(temp.GetLootReachType(), unit.position, origin))
                        return true;
                }
            }
        }

        foreach (var wall in wallUnits)
        {
            if (wall.Value is ILootable)
            {
                ILootable temp = (ILootable)wall.Value;
                if (temp.Lootable() && CheckReachType(temp.GetLootReachType(), wall.Value.position, origin))
                    return true;
            }
        }
        return false;
    }

    public bool CheckReachType(ReachType type, PositionKey objectPos, PositionKey checkPos, bool player = true)
    {
        switch (type)
        {
            case ReachType.sameTile:
                if (objectPos == checkPos)
                    return true;
                break;
            case ReachType.sameAndAdjacentTiles:
                if (CheckReachType(ReachType.adjacentTiles, objectPos, checkPos, player) || CheckReachType(ReachType.sameTile, objectPos, checkPos, player))
                {
                    return true;
                }
                break;
            case ReachType.adjacentTiles:
                if (
                    ((MapManager._instance.GetWallByGridPos(MapManager._instance.GetWallOfTileByDirection(objectPos, Direction.North)) == null || CheckWallAccess(MapManager._instance.GetWallOfTileByDirection(objectPos, Direction.North), player)) && MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(objectPos, Direction.North) == checkPos) || //north direction
                    ((MapManager._instance.GetWallByGridPos(MapManager._instance.GetWallOfTileByDirection(objectPos, Direction.East)) == null || CheckWallAccess(MapManager._instance.GetWallOfTileByDirection(objectPos, Direction.East), player)) && MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(objectPos, Direction.East) == checkPos) || //east direction
                    ((MapManager._instance.GetWallByGridPos(MapManager._instance.GetWallOfTileByDirection(objectPos, Direction.South)) == null || CheckWallAccess(MapManager._instance.GetWallOfTileByDirection(objectPos, Direction.South), player)) && MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(objectPos, Direction.South) == checkPos) || //south direction
                    ((MapManager._instance.GetWallByGridPos(MapManager._instance.GetWallOfTileByDirection(objectPos, Direction.West)) == null || CheckWallAccess(MapManager._instance.GetWallOfTileByDirection(objectPos, Direction.West), player)) && MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(objectPos, Direction.West) == checkPos) //west direction
                    )
                {
                    return true;
                }
                break;
            case ReachType.wall:
                foreach (var item in MapManager._instance.GetAllTilesAroundWall(objectPos))
                {
                    if (checkPos == item)
                        return true;
                }
                break;
            default:
                break;
        }
        return false;
    }

    public SuspiciousLevel CheckSusLevel(PositionKey checkPos)
    {
        SuspiciousLevel highestSusLevel = SuspiciousLevel.Unsuspicious;
        if (units.ContainsKey(checkPos))
        {
            foreach (var item in units[checkPos])
            {
                if (highestSusLevel == SuspiciousLevel.Unsuspicious)
                {
                    if (item.susLvl == SuspiciousLevel.Suspicious)
                    {
                        highestSusLevel = SuspiciousLevel.Suspicious;
                    }
                }
                if (item.susLvl == SuspiciousLevel.Alarming)
                {
                    return SuspiciousLevel.Alarming;
                }
            }
        }
        foreach (var item in MapManager._instance.GetAllWallsAroundTile(checkPos))
        {
            if (wallUnits.ContainsKey(item))
            {
                if (highestSusLevel == SuspiciousLevel.Unsuspicious)
                {
                    if (wallUnits[item].susLvl == SuspiciousLevel.Suspicious)
                    {
                        highestSusLevel = SuspiciousLevel.Suspicious;
                    }
                }
                if (wallUnits[item].susLvl == SuspiciousLevel.Alarming)
                {
                    return SuspiciousLevel.Alarming;
                }
            }
        }
        return highestSusLevel;
    }

    public T GetFirstUnitOfType<T>()
    {
        foreach (var tile in units)
        {
            foreach (var unit in units[tile.Key])
            {
                if (unit is PlayerUnit)
                {
                    PlayerUnit temp = (PlayerUnit)unit;
                    if (temp.alive)
                        break;
                }
                if (unit is T)
                    return (T)Convert.ChangeType(unit, typeof(T));
            }
        }
        foreach (var unit in wallUnits)
        {
            if (unit is T)
                return (T)Convert.ChangeType(unit, typeof(T));
        }
        return default(T);
    }


    public SerializedDataContainer Serialize()
    {
        SerializedDataContainer result = new SerializedDataContainer();
        result.type = GetSerializableType();
        foreach (var tile in units)
        {
            foreach (var unit in tile.Value)
            {
                result.Serialize((ISerializableUnit)unit);
            }
        }

        foreach (var wall in wallUnits)
        {
            result.Serialize((ISerializableUnit)wall.Value);
        }
        return result;
    }

    public void Deserialize(SerializedDataContainer input)
    {
        foreach (var tile in units)
        {
            foreach (var unit in tile.Value)
            {
                Destroy(unit.gameObject);
            }
        }

        foreach (var wall in wallUnits)
        {
            Destroy(wall.Value.gameObject);
        }
        units = new Dictionary<PositionKey, List<BaseUnit>>();
        wallUnits = new Dictionary<PositionKey, WallUnit>();
        foreach (var item in input.members)
        {
            GameObject prefab;
            switch (item.type)
            {
                case SerializableClasses.undef:
                    break;
                case SerializableClasses.unitManager:
                    break;
                case SerializableClasses.baseUnit:
                    prefab = Resources.Load(item.prefabPath) as GameObject;
                    Instantiate(prefab).GetComponent<BaseUnit>().Deserialize(item);
                    break;
                case SerializableClasses.playerUnit:
                    prefab = Resources.Load(item.prefabPath) as GameObject;
                    Instantiate(prefab).GetComponent<BaseUnit>().Deserialize(item);
                    break;
                case SerializableClasses.enemyUnit:
                    prefab = Resources.Load(item.prefabPath) as GameObject;
                    Instantiate(prefab).GetComponent<BaseUnit>().Deserialize(item);
                    break;
                case SerializableClasses.wallUnit:
                    prefab = Resources.Load(item.prefabPath) as GameObject;
                    Instantiate(prefab).GetComponent<WallUnit>().Deserialize(item);
                    break;
                case SerializableClasses.gameState:
                    break;
                default:
                    break;
            }
        }
    }

    public SerializableClasses GetSerializableType()
    {
        return SerializableClasses.unitManager;
    }
}
