using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHackable
{
    bool Hackable();
    void GetHacked(int damage);
    ReachType GetHackReachType();
}

public enum ReachType
{
    sameTile,
    sameAndAdjacentTiles,
    adjacentTiles,
    wall
}