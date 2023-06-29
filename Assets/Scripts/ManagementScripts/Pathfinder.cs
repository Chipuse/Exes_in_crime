using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public static Pathfinder _instance;
    private void Awake()
    {
        if (_instance == null || _instance == this)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
        //DeleventSystem.clickedOnTile += ExampleMethod;
    }

    public List<PositionKey> GetTilesInRange(PositionKey origin, int range)
    {
        List<PositionKey> result = new List<PositionKey>();
        List<PositionKey> toAdd;
        int counter = range;
        result.Add(origin);
        if (counter <= 0)
            return result;
        result.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(origin, Direction.North));
        result.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(origin, Direction.East));
        result.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(origin, Direction.South));
        result.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(origin, Direction.West));
        counter -= 1;
        while (counter > 0)
        {
            toAdd = new List<PositionKey>();
            foreach (var item in result)
            {
                if (!result.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North)) && !toAdd.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North)))
                    toAdd.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North));
                if (!result.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East)) && !toAdd.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East)))
                    toAdd.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East));
                if (!result.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South)) && !toAdd.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South)))
                    toAdd.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South));
                if (!result.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West)) && !toAdd.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West)))
                    toAdd.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West));

            }
            result.AddRange(toAdd);
            counter--;
        }
        return result;
    }

    public List<PositionKey> SimplePathFindCast(PositionKey origin, int range)
    {
        List<PositionKey> result = new List<PositionKey>();
        List<PositionKey> toAdd;
        int counter = range;
        result.Add(origin);
        if (counter <= 0)
            return result;
        result.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(origin, Direction.North));
        result.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(origin, Direction.East));
        result.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(origin, Direction.South));
        result.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(origin, Direction.West));
        counter -= 1;
        while(counter > 0)
        {
            toAdd = new List<PositionKey>();
            foreach (var item in result)
            {
                if(!result.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North)) && !toAdd.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North)))
                    toAdd.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North));
                if (!result.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East)) && !toAdd.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East)))
                    toAdd.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East));
                if (!result.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South)) && !toAdd.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South)))
                    toAdd.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South));
                if (!result.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West)) && !toAdd.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West)))
                    toAdd.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West));
                
            }
            result.AddRange(toAdd);
            counter--;
        }
        return result;
    }

    public List<PositionKey> GeneralPathFindingCast(PositionKey origin, int range, bool playerUnit)
    {
        List<PositionKey> result = new List<PositionKey>();
        List<PositionKey> toAdd;
        int counter = range;
        result.Add(origin);
        if (counter <= 0)
            return result;
        while (counter > 0)
        {
            toAdd = new List<PositionKey>();
            foreach (var item in result)
            {
                //north direction
                if(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North) != InvalidKey.Key)//check if there is even a movable tile in the direction
                {
                    if (!result.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North)) && //check that tile is not already found
                        !toAdd.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North)) && //check that tile is not about to already add
                        (MapManager._instance.GetWallByGridPos(MapManager._instance.GetWallOfTileByDirection(item, Direction.North)) == null || UnitManager._instance.CheckWallAccess(MapManager._instance.GetWallOfTileByDirection(item, Direction.North), playerUnit)) && //check that wall in that direction is accessible for player
                        (!UnitManager._instance.units.ContainsKey(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North)) || UnitManager._instance.CheckTileAccess(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North), playerUnit))) //check that all units on that tile if it is accessible
                    {
                        toAdd.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North));
                    }
                }
                //east direction
                if (MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East) != InvalidKey.Key)
                {
                    if (!result.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East)) &&
                        !toAdd.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East)) &&
                        (MapManager._instance.GetWallByGridPos(MapManager._instance.GetWallOfTileByDirection(item, Direction.East)) == null || UnitManager._instance.CheckWallAccess(MapManager._instance.GetWallOfTileByDirection(item, Direction.East), playerUnit)) && //check that wall in that direction is accessible for player
                        (!UnitManager._instance.units.ContainsKey(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East)) || UnitManager._instance.CheckTileAccess(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East), playerUnit))) //check that all units on that tile if it is accessible
                    {
                        toAdd.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East));
                    }
                }
                //south direction
                if (MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South) != InvalidKey.Key)
                {
                    if (!result.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South)) &&
                        !toAdd.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South)) &&
                        (MapManager._instance.GetWallByGridPos(MapManager._instance.GetWallOfTileByDirection(item, Direction.South)) == null || UnitManager._instance.CheckWallAccess(MapManager._instance.GetWallOfTileByDirection(item, Direction.South), playerUnit)) && //check that wall in that direction is accessible for player
                        (!UnitManager._instance.units.ContainsKey(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South)) || UnitManager._instance.CheckTileAccess(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South), playerUnit))) //check that all units on that tile if it is accessible
                    {
                        toAdd.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South));
                    }
                }
                //west direction
                if (MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West) != InvalidKey.Key)
                {
                    if (!result.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West)) &&
                        !toAdd.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West)) &&
                        (MapManager._instance.GetWallByGridPos(MapManager._instance.GetWallOfTileByDirection(item, Direction.West)) == null || UnitManager._instance.CheckWallAccess(MapManager._instance.GetWallOfTileByDirection(item, Direction.West), playerUnit)) && //check that wall in that direction is accessible for player
                        (!UnitManager._instance.units.ContainsKey(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West)) || UnitManager._instance.CheckTileAccess(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West), playerUnit))) //check that all units on that tile if it is accessible
                    {
                        toAdd.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West));
                    }
                }
            }
            result.AddRange(toAdd);
            counter--;
        }
        return result;
    }

    public List<PositionKey> GeneralNoiseFindingCast(PositionKey origin, int range, bool playerUnit)
    {
        List<PositionKey> result = new List<PositionKey>();
        List<PositionKey> toAdd;
        int counter = range;
        result.Add(origin);
        if (counter <= 0)
            return result;
        while (counter > 0)
        {
            toAdd = new List<PositionKey>();
            foreach (var item in result)
            {
                //north direction
                if (MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North) != InvalidKey.Key)//check if there is even a movable tile in the direction
                {
                    if (!result.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North)) && //check that tile is not already found
                        !toAdd.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North)) && //check that tile is not about to already add
                        (MapManager._instance.GetWallByGridPos(MapManager._instance.GetWallOfTileByDirection(item, Direction.North)) == null) && //check that wall in that direction is accessible for player
                        (!UnitManager._instance.units.ContainsKey(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North)) || UnitManager._instance.CheckTileAccess(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North), playerUnit))) //check that all units on that tile if it is accessible
                    {
                        toAdd.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North));
                    }
                }
                //east direction
                if (MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East) != InvalidKey.Key)
                {
                    if (!result.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East)) &&
                        !toAdd.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East)) &&
                        (MapManager._instance.GetWallByGridPos(MapManager._instance.GetWallOfTileByDirection(item, Direction.East)) == null) && //check that wall in that direction is accessible for player
                        (!UnitManager._instance.units.ContainsKey(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East)) || UnitManager._instance.CheckTileAccess(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East), playerUnit))) //check that all units on that tile if it is accessible
                    {
                        toAdd.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East));
                    }
                }
                //south direction
                if (MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South) != InvalidKey.Key)
                {
                    if (!result.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South)) &&
                        !toAdd.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South)) &&
                        (MapManager._instance.GetWallByGridPos(MapManager._instance.GetWallOfTileByDirection(item, Direction.South)) == null) && //check that wall in that direction is accessible for player
                        (!UnitManager._instance.units.ContainsKey(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South)) || UnitManager._instance.CheckTileAccess(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South), playerUnit))) //check that all units on that tile if it is accessible
                    {
                        toAdd.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South));
                    }
                }
                //west direction
                if (MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West) != InvalidKey.Key)
                {
                    if (!result.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West)) &&
                        !toAdd.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West)) &&
                        (MapManager._instance.GetWallByGridPos(MapManager._instance.GetWallOfTileByDirection(item, Direction.West)) == null) && //check that wall in that direction is accessible for player
                        (!UnitManager._instance.units.ContainsKey(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West)) || UnitManager._instance.CheckTileAccess(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West), playerUnit))) //check that all units on that tile if it is accessible
                    {
                        toAdd.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West));
                    }
                }
            }
            result.AddRange(toAdd);
            counter--;
        }
        return result;
    }

    public List<PositionKey> GeneralVisionCast(PositionKey origin, int range, bool playerUnit = false)
    {
        List<PositionKey> result = new List<PositionKey>();
        List<PositionKey> toAdd;
        List<PositionKey> invalidPosNE = new List<PositionKey>();
        List<PositionKey> invalidPosSE = new List<PositionKey>();
        List<PositionKey> invalidPosSW = new List<PositionKey>();
        List<PositionKey> invalidPosNW = new List<PositionKey>();
        int counter = range;
        result.Add(origin);
        if (counter <= 0)
            return result;
        while (counter > 0)
        {
            toAdd = new List<PositionKey>();
            foreach (var item in result)
            {
                //north direction
                PositionKey north = MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North);
                if (north != InvalidKey.Key)//check if there is even a movable tile in the direction
                {
                    if (!result.Contains(north)) //check that tile is not already found
                    {
                        if ( NorthEastCheck(north, invalidPosNE) &&//Check that in north east is not already invalid
                             NorthWestCheck(north, invalidPosNW) && //check that north west is not already invalid
                            (MapManager._instance.GetWallByGridPos(MapManager._instance.GetWallOfTileByDirection(item, Direction.North)) == null /*|| UnitManager._instance.CheckWallAccess(MapManager._instance.GetWallOfTileByDirection(item, Direction.North), playerUnit)*/) && //check that wall in that direction is accessible for player
                            (UnitManager._instance.CheckTileSeeThrough(north))) //check that all units on that tile if it are seethrough
                        {
                            if(!toAdd.Contains(north))
                                toAdd.Add(north);
                        }
                        else
                        {
                            //add to invalid
                            if (north.y >= origin.y && north.x >= origin.x)
                                invalidPosNE.Add(north);
                            if (north.y <= origin.y && north.x >= origin.x)
                                invalidPosSE.Add(north);
                            if (north.y <= origin.y && north.x <= origin.x)
                                invalidPosSW.Add(north);
                            if (north.y >= origin.y && north.x <= origin.x)
                                invalidPosNW.Add(north);
                            if (toAdd.Contains(north))
                                toAdd.Remove(north);
                        }
                    }                    
                }
                //east direction
                PositionKey east = MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East);
                if (east != InvalidKey.Key)
                {
                    if (!result.Contains(east)) //check that tile is not already found
                    {
                        if (NorthEastCheck(east, invalidPosNE) &&//Check that in north east is not already invalid
                             SouthEastCheck(east, invalidPosSE) && //check that north west is not already invalid
                            (MapManager._instance.GetWallByGridPos(MapManager._instance.GetWallOfTileByDirection(item, Direction.East)) == null /*|| UnitManager._instance.CheckWallAccess(MapManager._instance.GetWallOfTileByDirection(item, Direction.East), playerUnit)*/) && //check that wall in that direction is accessible for player
                            (UnitManager._instance.CheckTileSeeThrough(east))) //check that all units on that tile if it are seethrough
                        {
                            if(!toAdd.Contains(east))
                                toAdd.Add(east);
                        }
                        else
                        {
                            //add to invalid
                            if (east.y >= origin.y && east.x >= origin.x)
                                invalidPosNE.Add(east);
                            if (east.y <= origin.y && east.x >= origin.x)
                                invalidPosSE.Add(east);
                            if (east.y <= origin.y && east.x <= origin.x)
                                invalidPosSW.Add(east);
                            if (east.y >= origin.y && east.x <= origin.x)
                                invalidPosNW.Add(east);
                            if (toAdd.Contains(east))
                            {
                                toAdd.Remove(east);
                            }
                        }
                    }
                }
                //south direction
                PositionKey south = MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South);
                if (south != InvalidKey.Key)
                {
                    if (!result.Contains(south)) //check that tile is not already found
                    {
                        if (SouthWestCheck(south, invalidPosSW) &&//Check that in north east is not already invalid
                             SouthEastCheck(south, invalidPosSE) && //check that north west is not already invalid
                            (MapManager._instance.GetWallByGridPos(MapManager._instance.GetWallOfTileByDirection(item, Direction.South)) == null /*|| UnitManager._instance.CheckWallAccess(MapManager._instance.GetWallOfTileByDirection(item, Direction.South), playerUnit)*/) && //check that wall in that direction is accessible for player
                            (UnitManager._instance.CheckTileSeeThrough(south))) //check that all units on that tile if it are seethrough
                        {
                            if(!toAdd.Contains(south))
                                toAdd.Add(south);
                        }
                        else
                        {
                            //add to invalid
                            if (south.y >= origin.y && south.x >= origin.x)
                                invalidPosNE.Add(south);
                            if (south.y <= origin.y && south.x >= origin.x)
                                invalidPosSE.Add(south);
                            if (south.y <= origin.y && south.x <= origin.x)
                                invalidPosSW.Add(south);
                            if (south.y >= origin.y && south.x <= origin.x)
                                invalidPosNW.Add(south);
                            if (toAdd.Contains(south))
                            {
                                toAdd.Remove(south);
                            }
                        }
                    }
                }
                //west direction
                PositionKey west = MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West);
                if (west != InvalidKey.Key)
                {
                    if (!result.Contains(west)) //check that tile is not already found
                    {
                        if (SouthWestCheck(west, invalidPosSW) &&//Check that in north east is not already invalid
                             NorthWestCheck(west, invalidPosNW) && //check that north west is not already invalid
                            (MapManager._instance.GetWallByGridPos(MapManager._instance.GetWallOfTileByDirection(item, Direction.West)) == null /*|| UnitManager._instance.CheckWallAccess(MapManager._instance.GetWallOfTileByDirection(item, Direction.West), playerUnit)*/) && //check that wall in that direction is accessible for player
                            (UnitManager._instance.CheckTileSeeThrough(west))) //check that all units on that tile if it are seethrough
                        {
                            if(!toAdd.Contains(west))
                                toAdd.Add(west);
                        }
                        else
                        {
                            //add to invalid
                            if (west.y >= origin.y && west.x >= origin.x)
                                invalidPosNE.Add(west);
                            if (west.y <= origin.y && west.x >= origin.x)
                                invalidPosSE.Add(west);
                            if (west.y <= origin.y && west.x <= origin.x)
                                invalidPosSW.Add(west);
                            if (west.y >= origin.y && west.x <= origin.x)
                                invalidPosNW.Add(west);
                            if (toAdd.Contains(west))
                                toAdd.Remove(west);
                        }
                    }
                }
            }
            result.AddRange(toAdd);
            counter--;
        }
        return result;
    }

    bool NorthEastCheck(PositionKey pos, List<PositionKey> invalidKeys)
    {
        foreach (PositionKey positionKey in invalidKeys)
        {
            if (pos.y >= positionKey.y && pos.x >= positionKey.x)
                return false;
        }
        return true;
    }

    bool SouthEastCheck(PositionKey pos, List<PositionKey> invalidKeys)
    {
        foreach (PositionKey positionKey in invalidKeys)
        {
            if (pos.y <= positionKey.y && pos.x >= positionKey.x)
                return false;
        }
        return true;
    }

    bool SouthWestCheck(PositionKey pos, List<PositionKey> invalidKeys)
    {
        foreach (PositionKey positionKey in invalidKeys)
        {
            if (pos.y <= positionKey.y && pos.x <= positionKey.x)
                return false;
        }
        return true;
    }

    bool NorthWestCheck(PositionKey pos, List<PositionKey> invalidKeys)
    {
        foreach (PositionKey positionKey in invalidKeys)
        {
            if (pos.y >= positionKey.y && pos.x <= positionKey.x)
                return false;
        }
        return true;
    }
    //TODO pathfinding for player and enemy units -> access through units

    public List<PositionKey> GetPath(PositionKey start, PositionKey end, bool playerUnit, int maxRange = 100)
    {
        if(start == end)
        {
            return new List<PositionKey>();
        }
        //TODO replace tilepath with just a list of list of tiles?
        List<List<PositionKey>> resultPaths = new List<List<PositionKey>>();
        List<List<PositionKey>> toAddPaths;
        List<PositionKey> result = new List<PositionKey>(); 
        List<PositionKey> toAdd;
        int counter = maxRange;
        resultPaths.Add(new List<PositionKey>());
        resultPaths[0].Add(start);
        result.Add(start);
        if (counter <= 0)
            return resultPaths[0];
        while (counter > 0)
        {
            toAdd = new List<PositionKey>();
            toAddPaths = new List<List<PositionKey>>();
            for (int i = 0; i < result.Count; i++)
            {
                PositionKey item = result[i];
                //north direction
                if (MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(result[i], Direction.North) != InvalidKey.Key)//check if there is even a movable tile in the direction
                {
                    if (!result.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North)) && //check that tile is not already found
                        !toAdd.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North)) && //check that tile is not about to already add
                        (MapManager._instance.GetWallByGridPos(MapManager._instance.GetWallOfTileByDirection(item, Direction.North)) == null || UnitManager._instance.CheckWallAccess(MapManager._instance.GetWallOfTileByDirection(item, Direction.North), playerUnit)) && //check that wall in that direction is accessible for player
                        (!UnitManager._instance.units.ContainsKey(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North)) || UnitManager._instance.CheckTileAccess(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North), playerUnit))) //check that all units on that tile if it is accessible
                    {
                        toAdd.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North));

                        List<PositionKey> temp = new List<PositionKey>();
                        temp.AddRange(resultPaths[i]);
                        temp.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North));
                        if (MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North) == end)
                            return temp;
                        toAddPaths.Add(temp);
                    }
                }
                //east direction
                if (MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East) != InvalidKey.Key)
                {
                    if (!result.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East)) &&
                        !toAdd.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East)) &&
                        (MapManager._instance.GetWallByGridPos(MapManager._instance.GetWallOfTileByDirection(item, Direction.East)) == null || UnitManager._instance.CheckWallAccess(MapManager._instance.GetWallOfTileByDirection(item, Direction.East), playerUnit)) && //check that wall in that direction is accessible for player
                        (!UnitManager._instance.units.ContainsKey(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East)) || UnitManager._instance.CheckTileAccess(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East), playerUnit))) //check that all units on that tile if it is accessible
                    {
                        toAdd.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East));

                        List<PositionKey> temp = new List<PositionKey>();
                        temp.AddRange(resultPaths[i]);
                        temp.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East));
                        if (MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East) == end)
                            return temp;
                        toAddPaths.Add(temp);
                    }
                }
                //south direction
                if (MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South) != InvalidKey.Key)
                {
                    if (!result.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South)) &&
                        !toAdd.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South)) &&
                        (MapManager._instance.GetWallByGridPos(MapManager._instance.GetWallOfTileByDirection(item, Direction.South)) == null || UnitManager._instance.CheckWallAccess(MapManager._instance.GetWallOfTileByDirection(item, Direction.South), playerUnit)) && //check that wall in that direction is accessible for player
                        (!UnitManager._instance.units.ContainsKey(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South)) || UnitManager._instance.CheckTileAccess(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South), playerUnit))) //check that all units on that tile if it is accessible
                    {
                        toAdd.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South));

                        List<PositionKey> temp = new List<PositionKey>();
                        temp.AddRange(resultPaths[i]);
                        temp.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South));
                        if (MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South) == end)
                            return temp;
                        toAddPaths.Add(temp);
                    }
                }
                //west direction
                if (MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West) != InvalidKey.Key)
                {
                    if (!result.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West)) &&
                        !toAdd.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West)) &&
                        (MapManager._instance.GetWallByGridPos(MapManager._instance.GetWallOfTileByDirection(item, Direction.West)) == null || UnitManager._instance.CheckWallAccess(MapManager._instance.GetWallOfTileByDirection(item, Direction.West), playerUnit)) && //check that wall in that direction is accessible for player
                        (!UnitManager._instance.units.ContainsKey(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West)) || UnitManager._instance.CheckTileAccess(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West), playerUnit))) //check that all units on that tile if it is accessible
                    {
                        toAdd.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West));

                        List<PositionKey> temp = new List<PositionKey>();
                        temp.AddRange(resultPaths[i]);
                        temp.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West));
                        if (MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West) == end)
                            return temp;
                        toAddPaths.Add(temp);
                    }
                }
            }
            result.AddRange(toAdd);
            resultPaths.AddRange(toAddPaths);
            counter--;
        }
        //tile was out of range
        return resultPaths[0];
    }

    public PositionKey GetClosestUnitPosition<T>(PositionKey origin, bool playerUnit = false, int range = 100)
    {
        List<PositionKey> result = new List<PositionKey>();
        List<PositionKey> toAdd;
        int counter = range;
        result.Add(origin);
        if (counter <= 0)
            return InvalidKey.Key;
        while (counter > 0)
        {
            toAdd = new List<PositionKey>();
            foreach (var item in result)
            {
                //north direction
                if (MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North) != InvalidKey.Key)//check if there is even a movable tile in the direction
                {
                    if (!result.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North)) && //check that tile is not already found
                        !toAdd.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North)) && //check that tile is not about to already add
                        (MapManager._instance.GetWallByGridPos(MapManager._instance.GetWallOfTileByDirection(item, Direction.North)) == null || UnitManager._instance.CheckWallAccess(MapManager._instance.GetWallOfTileByDirection(item, Direction.North), playerUnit)) && //check that wall in that direction is accessible for player
                        (!UnitManager._instance.units.ContainsKey(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North)) || UnitManager._instance.CheckTileAccess(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North), playerUnit))) //check that all units on that tile if it is accessible
                    {
                        if (UnitManager._instance.units.ContainsKey(item))
                        {
                            foreach (var unit in UnitManager._instance.units[item])
                            {                                
                                if(unit is T)
                                {
                                    if(unit is AlarmUnit)
                                    {
                                        AlarmUnit temp = (AlarmUnit)unit;
                                        if (!temp.Tried)
                                            return temp.position;
                                    }
                                    else if (unit is PlayerUnit)
                                    {
                                        PlayerUnit temp = (PlayerUnit)unit;
                                        if (temp.alive)
                                            return temp.position;
                                    }
                                    else
                                        return unit.position;
                                }
                            }
                        }
                        toAdd.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.North));
                    }
                }
                //east direction
                if (MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East) != InvalidKey.Key)
                {
                    if (!result.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East)) &&
                        !toAdd.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East)) &&
                        (MapManager._instance.GetWallByGridPos(MapManager._instance.GetWallOfTileByDirection(item, Direction.East)) == null || UnitManager._instance.CheckWallAccess(MapManager._instance.GetWallOfTileByDirection(item, Direction.East), playerUnit)) && //check that wall in that direction is accessible for player
                        (!UnitManager._instance.units.ContainsKey(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East)) || UnitManager._instance.CheckTileAccess(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East), playerUnit))) //check that all units on that tile if it is accessible
                    {
                        if (UnitManager._instance.units.ContainsKey(item))
                        {
                            foreach (var unit in UnitManager._instance.units[item])
                            {
                                if (unit is T)
                                {
                                    if (unit is AlarmUnit)
                                    {
                                        AlarmUnit temp = (AlarmUnit)unit;
                                        if (!temp.Tried)
                                            return temp.position;
                                    }
                                    else if (unit is PlayerUnit)
                                    {
                                        PlayerUnit temp = (PlayerUnit)unit;
                                        if (temp.alive)
                                            return temp.position;
                                    }
                                    else
                                        return unit.position;
                                }
                            }
                        }
                        toAdd.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.East));
                    }
                }
                //south direction
                if (MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South) != InvalidKey.Key)
                {
                    if (!result.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South)) &&
                        !toAdd.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South)) &&
                        (MapManager._instance.GetWallByGridPos(MapManager._instance.GetWallOfTileByDirection(item, Direction.South)) == null || UnitManager._instance.CheckWallAccess(MapManager._instance.GetWallOfTileByDirection(item, Direction.South), playerUnit)) && //check that wall in that direction is accessible for player
                        (!UnitManager._instance.units.ContainsKey(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South)) || UnitManager._instance.CheckTileAccess(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South), playerUnit))) //check that all units on that tile if it is accessible
                    {
                        if (UnitManager._instance.units.ContainsKey(item))
                        {
                            foreach (var unit in UnitManager._instance.units[item])
                            {
                                if (unit is T)
                                {
                                    if (unit is AlarmUnit)
                                    {
                                        AlarmUnit temp = (AlarmUnit)unit;
                                        if (!temp.Tried)
                                            return temp.position;
                                    }
                                    else if (unit is PlayerUnit)
                                    {
                                        PlayerUnit temp = (PlayerUnit)unit;
                                        if (temp.alive)
                                            return temp.position;
                                    }
                                    else
                                        return unit.position;
                                }
                            }
                        }
                        toAdd.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.South));
                    }
                }
                //west direction
                if (MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West) != InvalidKey.Key)
                {
                    if (!result.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West)) &&
                        !toAdd.Contains(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West)) &&
                        (MapManager._instance.GetWallByGridPos(MapManager._instance.GetWallOfTileByDirection(item, Direction.West)) == null || UnitManager._instance.CheckWallAccess(MapManager._instance.GetWallOfTileByDirection(item, Direction.West), playerUnit)) && //check that wall in that direction is accessible for player
                        (!UnitManager._instance.units.ContainsKey(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West)) || UnitManager._instance.CheckTileAccess(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West), playerUnit))) //check that all units on that tile if it is accessible
                    {
                        if (UnitManager._instance.units.ContainsKey(item))
                        {
                            foreach (var unit in UnitManager._instance.units[item])
                            {
                                if(unit is T)
                                {
                                    if (unit is AlarmUnit)
                                    {
                                        AlarmUnit temp = (AlarmUnit)unit;
                                        if (!temp.Tried)
                                            return temp.position;
                                    }
                                    else if (unit is PlayerUnit)
                                    {
                                        PlayerUnit temp = (PlayerUnit)unit;
                                        if (temp.alive)
                                            return temp.position;
                                    }
                                    else
                                        return unit.position;
                                }
                            }
                        }
                        toAdd.Add(MapManager._instance.GetAdjacentTilesPosKeyByPositionKey(item, Direction.West));
                    }
                }
            }
            result.AddRange(toAdd);
            counter--;
        }
        return InvalidKey.Key;
    }

    public T GetClosestUnit<T>(PositionKey origin, bool player = false, int range = 100)
    {
        PositionKey tile = GetClosestUnitPosition<T>(origin, player, range);
        if(tile != InvalidKey.Key)
        {
            if (UnitManager._instance.units.ContainsKey(tile))
            {
                foreach (var unit in UnitManager._instance.units[tile])
                {
                    if (unit is T)
                    {
                        return (T)Convert.ChangeType(unit, typeof(T)); //idk what this does...
                    }
                }
            }
        }
        return default(T);
    }

    public int range = 5;
    List<GameObject> usedEffectObjects = new List<GameObject>();
    private void Update()
    {

    }

    public void ExampleMethod(PositionKey origin, int range)
    {
        foreach (var item in usedEffectObjects)
        {
            item.SetActive(false);
        }
        usedEffectObjects = new List<GameObject>();
        foreach (var item in GeneralPathFindingCast( origin, range, true))
        {
            GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.genericGroundEffects);
            go.SetActive(true);
            go.transform.SetPositionAndRotation(MapManager._instance.GroundGridPosToWorldPos(item), go.transform.rotation);
            go.transform.Translate(Vector3.up * 0.53f, Space.World);
            usedEffectObjects.Add(go);
        }
    }

    public void ExampleMethodTwo(PositionKey origin, PositionKey target, int range = 10)
    {
        foreach (var item in usedEffectObjects)
        {
            item.SetActive(false);
        }
        usedEffectObjects = new List<GameObject>();
        foreach (var item in GetPath(origin, target, true, range))
        {
            GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.genericGroundEffects);
            go.SetActive(true);
            go.transform.SetPositionAndRotation(MapManager._instance.GroundGridPosToWorldPos(item), go.transform.rotation);
            go.transform.Translate(Vector3.up * 0.53f, Space.World);
            usedEffectObjects.Add(go);
        }
    }

    public void ExampleMethodThree(PositionKey origin)
    {
        foreach (var item in usedEffectObjects)
        {
            item.SetActive(false);
        }
        usedEffectObjects = new List<GameObject>();
        
        foreach (var item in GeneralVisionCast(origin, 10))
        //foreach (var item in GeneralPathFindingCast(origin, 10, false))
        {
            GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.genericGroundEffects);
            go.SetActive(true);
            go.transform.SetPositionAndRotation(MapManager._instance.GroundGridPosToWorldPos(item), go.transform.rotation);
            go.transform.Translate(Vector3.up * 0.53f, Space.World);
            usedEffectObjects.Add(go);
        }
    }
}
