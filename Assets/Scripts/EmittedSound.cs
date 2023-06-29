using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmittedSound : MonoBehaviour
{
    public PositionKey originPos = InvalidKey.Key;
    public List<PositionKey> affectedArea = new List<PositionKey>();
    public float timer = 0f;
    public int range = 1;
    bool finished = false;

    public static void NoiseEventOnUnit(BaseUnit unit, int _range, float _timer = 0f)
    {
        StartNoiseEventOnGo(unit.gameObject, unit.position, _range, _timer);
    }

    public static void NoiseEventOnUnit(WallUnit unit, int _range, float _timer = 0f)
    {
        foreach (var tile in MapManager._instance.GetAllTilesAroundWall(unit.position))
        {
            StartNoiseEventOnGo(unit.gameObject, tile, _range, _timer);
        }       
    }

    public static void StartNoiseEventOnGo(GameObject go, PositionKey _origin, int _range, float _timer = 0f)
    {
        EmittedSound noise = go.AddComponent<EmittedSound>();
        noise.StartEvent(_origin, _range, _timer);
    }

    public void StartEvent(PositionKey _origin, int _range, float _timer = 0f)
    {
        originPos = _origin;
        range = _range;
        timer = _timer;
        affectedArea = Pathfinder._instance.GeneralNoiseFindingCast(originPos, range, false);
        AnimationManager._instance.StartWaitTime(timer, EventWrapUp);
        StartCoroutine(SoundRoutine());
    }

    IEnumerator SoundRoutine()
    {
        foreach (var tile in affectedArea)
        {
            GameObject go = EffectPooler._instance.GetPooledObject(EffectPooler._instance.noiseEffects);
            go.GetComponent<DamageEffect>().StartEffect(MapManager._instance.GroundGridPosToWorldPos(tile), "(((NOISE)))", Color.black, 1, 0);
            go.SetActive(true);
            yield return new WaitForSeconds(timer);
        }
        while (!finished)
        {
            yield return null;
        }
        Destroy(this);
    }

    public void EventWrapUp()
    {
        foreach (var tile in affectedArea)
        {
            if (UnitManager._instance.units.ContainsKey(tile))
            {
                foreach (var unit in UnitManager._instance.units[tile])
                {
                    if(unit is EnemyUnit)
                    {
                        EnemyUnit temp = (EnemyUnit)unit;
                        //do se suspicious thingy
                        temp.GetSuspicous(originPos);
                    }
                }
            }
        }
        finished = true;
    }
}
