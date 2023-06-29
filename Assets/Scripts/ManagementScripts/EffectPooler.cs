using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPooler : MonoBehaviour
{
    public static EffectPooler _instance;
    [HideInInspector]
    public List<GameObject> genericGroundEffects;
    public GameObject genericGroundEffect;
    [HideInInspector]
    public List<GameObject> enemyVisionEffects;
    public GameObject enemyVisionEffect;
    [HideInInspector]
    public List<GameObject> cameraVisionEffects;
    public GameObject cameraVisionEffect;
    [HideInInspector]
    public List<GameObject> enemyPathEffects;
    public GameObject enemyPathEffect;
    public int startAmountGenericGroundEffect = 20;

    [HideInInspector]
    public List<GameObject> damageEffects;
    public GameObject damageEffect;
    [HideInInspector]
    public List<GameObject> tempEnemyPathEffects;
    public GameObject tempEnemyPathEffect;
    [HideInInspector]
    public List<GameObject> tempTargetEffects;
    public GameObject tempTargetEffect;

    [HideInInspector]
    public List<GameObject> onTileEffects;
    public GameObject onTileEffect;
    [HideInInspector]
    public List<GameObject> unitMarkers;
    public GameObject unitMarker;
    [HideInInspector]
    public List<GameObject> validTargetEffects;
    public GameObject validTargetEffect;

    [HideInInspector]
    public List<GameObject> noiseEffects;
    public GameObject noiseEffect;

    [HideInInspector]
    public List<GameObject> placementEffects;
    public GameObject placementEffect;

    private void Awake()
    {
        if(_instance == null || _instance == this)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        genericGroundEffects = new List<GameObject>();
        enemyVisionEffects = new List<GameObject>();
        enemyPathEffects = new List<GameObject>();
        validTargetEffects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < startAmountGenericGroundEffect; i++)
        {
            tmp = Instantiate(genericGroundEffect);
            tmp.SetActive(false);
            genericGroundEffects.Add(tmp);
        }
        for (int i = 0; i < startAmountGenericGroundEffect; i++)
        {
            tmp = Instantiate(enemyVisionEffect);
            tmp.SetActive(false);
            enemyVisionEffects.Add(tmp);
        }
        for (int i = 0; i < startAmountGenericGroundEffect; i++)
        {
            tmp = Instantiate(enemyPathEffect);
            tmp.SetActive(false);
            enemyPathEffects.Add(tmp);
        }
        for (int i = 0; i < startAmountGenericGroundEffect; i++)
        {
            tmp = Instantiate(damageEffect);
            tmp.SetActive(false);
            damageEffects.Add(tmp);
        }
        for (int i = 0; i < startAmountGenericGroundEffect; i++)
        {
            tmp = Instantiate(tempEnemyPathEffect);
            tmp.SetActive(false);
            tempEnemyPathEffects.Add(tmp);
        }
        for (int i = 0; i < startAmountGenericGroundEffect; i++)
        {
            tmp = Instantiate(tempTargetEffect);
            tmp.SetActive(false);
            tempTargetEffects.Add(tmp);
        }
        for (int i = 0; i < startAmountGenericGroundEffect; i++)
        {
            tmp = Instantiate(onTileEffect);
            tmp.SetActive(false);
            onTileEffects.Add(tmp);
        }
        for (int i = 0; i < startAmountGenericGroundEffect; i++)
        {
            tmp = Instantiate(cameraVisionEffect);
            tmp.SetActive(false);
            cameraVisionEffects.Add(tmp);
        }for (int i = 0; i < startAmountGenericGroundEffect; i++)
        {
            tmp = Instantiate(unitMarker);
            tmp.SetActive(false);
            unitMarkers.Add(tmp);
        }
        for (int i = 0; i < startAmountGenericGroundEffect; i++)
        {
            tmp = Instantiate(validTargetEffect);
            tmp.SetActive(false);
            validTargetEffects.Add(tmp);
        }
        for (int i = 0; i < startAmountGenericGroundEffect; i++)
        {
            tmp = Instantiate(noiseEffect);
            tmp.SetActive(false);
            noiseEffects.Add(tmp);
        }
        for (int i = 0; i < 3; i++)
        {
            tmp = Instantiate(placementEffect);
            tmp.SetActive(false);
            placementEffects.Add(tmp);
        }
    }

    public GameObject GetPooledObject(List<GameObject> listOfPooledObjects)
    {
        foreach (var item in listOfPooledObjects)
        {
            if(!item.activeInHierarchy)
            {
                return item;
            }
        }
        //there arent enough objects in pool!
        GameObject tmp; 
        tmp = Instantiate(listOfPooledObjects[0]);
        tmp.SetActive(false);
        listOfPooledObjects.Add(tmp);
        return tmp;
    }
}
