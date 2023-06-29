using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEffectsManager : MonoBehaviour
{
    public static GroundEffectsManager _instance;
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

    private void OnEnable()
    {
        DeleventSystem.mapVisualsUpdate += OnMapVisualsUpdate;
    }
    private void OnDisable()
    {
        DeleventSystem.mapVisualsUpdate -= OnMapVisualsUpdate;
        
    }

    List<GameObject> enemyVisionObjects = new List<GameObject>();
    List<GameObject> enemyPathObjects = new List<GameObject>();
    List<GameObject> activeUnitMovementObjects = new List<GameObject>();
    List<GameObject> activeUnitPathObjects = new List<GameObject>();
    
    void OnMapVisualsUpdate()
    {
        foreach (var item in enemyVisionObjects)
        {
            item.SetActive(false);
        }
        enemyVisionObjects = new List<GameObject>();
        foreach (var item in enemyPathObjects)
        {
            item.SetActive(false);
        }
        enemyPathObjects = new List<GameObject>();
        foreach (var tile in UnitManager._instance.units)
        {
            for (int i = 0; i < tile.Value.Count; i++)
            {
                BaseUnit unit = tile.Value[i];
                if (unit is AlarmUnit)
                {
                    AlarmUnit temp = (AlarmUnit)unit;
                    if (!temp.Tried)
                    {                        
                        GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.unitMarkers);
                        go.SetActive(true);
                        go.transform.SetPositionAndRotation(MapManager._instance.GroundGridPosToWorldPos(temp.position), go.transform.rotation);
                        go.transform.Translate(Vector3.up * 0.53f, Space.World);
                        enemyVisionObjects.Add(go);
                    }
                }
                else if (unit is CameraUnit)
                {
                    CameraUnit temp = (CameraUnit)unit;
                    if (temp.Active)
                    {
                        foreach (var item in temp.watchedTiles)
                        {
                            GameObject tempGo = EffectPooler._instance.GetPooledObject(EffectPooler._instance.cameraVisionEffects);
                            tempGo.SetActive(true);
                            tempGo.transform.SetPositionAndRotation(MapManager._instance.GroundGridPosToWorldPos(item), tempGo.transform.rotation);
                            tempGo.transform.Translate(Vector3.up * 0.53f, Space.World);
                            enemyVisionObjects.Add(tempGo);
                        }
                        GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.unitMarkers);
                        go.SetActive(true);
                        go.transform.SetPositionAndRotation(MapManager._instance.GroundGridPosToWorldPos(temp.position), go.transform.rotation);
                        go.transform.Translate(Vector3.up * 0.53f, Space.World);
                        enemyVisionObjects.Add(go);
                    }
                }
                else if (unit is EnemyUnit)
                {
                    EnemyUnit temp = (EnemyUnit)unit;
                    foreach (var item in temp.watchedTiles)
                    {
                        GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.enemyVisionEffects);
                        go.SetActive(true);
                        go.transform.SetPositionAndRotation(MapManager._instance.GroundGridPosToWorldPos(item), go.transform.rotation);
                        go.transform.Translate(Vector3.up * 0.53f, Space.World);
                        enemyVisionObjects.Add(go);
                    }
                    /*
                    foreach (var item in temp.nextWatchedTiles)
                    {
                        GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.enemyVisionEffects);
                        go.SetActive(true);
                        go.transform.SetPositionAndRotation(MapManager._instance.GroundGridPosToWorldPos(item), go.transform.rotation);
                        go.transform.Translate(Vector3.up * 0.53f, Space.World);
                        enemyVisionObjects.Add(go);
                    }*/
                    if(temp.state == EnemyState.Unsuspicious && temp.route.Count > 1)
                    {
                        PositionKey nextNode = InvalidKey.Key;
                        for(int routeIndex = 0; routeIndex < temp.route.Count; routeIndex++)
                        {
                            PositionKey currentNode = temp.route[routeIndex];
                            if(routeIndex + 1 < temp.route.Count)
                            {
                                nextNode = temp.route[routeIndex + 1];
                            }
                            else
                            {
                                nextNode = temp.route[0];
                            }
                            foreach (var item in Pathfinder._instance.GetPath(currentNode, nextNode,false))
                            {
                                GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.enemyPathEffects);
                                go.SetActive(true);
                                go.transform.SetPositionAndRotation(MapManager._instance.GroundGridPosToWorldPos(item), go.transform.rotation);
                                go.transform.Translate(Vector3.up * 0.53f, Space.World);
                                enemyPathObjects.Add(go);
                            }                            
                        }
                    }
                    /*
                    if(temp.nextPosition != InvalidKey.Key && temp.nextPosition != temp.position)
                    {
                        foreach (var item in Pathfinder._instance.GetPath(temp.position, temp.nextPosition, false))
                        {
                            GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.enemyPathEffects);
                            go.SetActive(true);
                            go.transform.SetPositionAndRotation(MapManager._instance.GroundGridPosToWorldPos(item), go.transform.rotation);
                            go.transform.Translate(Vector3.up * 0.53f, Space.World);
                            enemyPathObjects.Add(go);
                        }
                    }*/
                    
                }
            }
        }
    }

    void OnPlayerUnitUpdate()
    {

    }
}
