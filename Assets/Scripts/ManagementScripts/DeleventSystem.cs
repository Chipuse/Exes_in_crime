using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleventSystem
{
    public delegate void PositionEvent(PositionKey pos);
    public static PositionEvent clickedOnTile;
    public static PositionEvent illegalAction;

    public delegate void PositionUnitEvent(PositionKey pos, GameObject unit);
    public static PositionUnitEvent playerUnitEnteredTile;

    public delegate void SimpleEvent();
    public static SimpleEvent playerUnitUpdate;
    public static SimpleEvent enemyUnitUpdate; //checking for state change->mostly by checking current watchable tiles for e.g. alarming stuff or player units
    public static SimpleEvent enemyPathUpdate;
    public static SimpleEvent mapVisualsUpdate;
    public static SimpleEvent handVisualsUpdate;
    public static SimpleEvent levelStart;
    public static SimpleEvent levelInit; // only used at the beginning of entering a level scene. Donot do when "just reloading serialized data"
    public static SimpleEvent lateLevelInit; // only used at the beginning of entering a level scene. Donot do when "just reloading serialized data"
    public static SimpleEvent enemyTurn;
    public static SimpleEvent allyTurn;
    public static SimpleEvent playerTurn;
    public static SimpleEvent preDeserialization;
    public static SimpleEvent postDeserialization;
    public static SimpleEvent fireAlarm;

    public delegate void InputEvent(InputMode inputMode);
    public static InputEvent newInputMode;
    public static InputEvent oldInputMode;
}
