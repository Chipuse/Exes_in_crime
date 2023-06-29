using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager _instance;
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
    public delegate void TestDelegate();
    // beliebige scripte können hier animationen starten und sich mit funktionscalls darauf warten bis diese animationen fertig sind

    //should have functionality to wait for animation clips, soundclips and fixed times
    //Maybe even other stuff like transform translation

    public List<TimeEvent> timeEvents = new List<TimeEvent>();
    private float Timer = 0;

    public void StartWaitTime(float secToWait, TestDelegate _func)
    {
        PauseGameManager();
        TimeEvent timeEvent = new TimeEvent { func = _func, timeExecute = Timer + secToWait };
        timeEvents.Add(timeEvent);
    }

    public List<TranslateEvent> translateEvents = new List<TranslateEvent>();
    public void StartWaitTranslate(Transform _tf, Vector3 _target, float _speed, TestDelegate _func)
    {
        PauseGameManager();
        TranslateEvent translateEvent = new TranslateEvent { func = _func, speed = _speed, targetPos = _target, tf = _tf };
        translateEvents.Add(translateEvent);
    }

    void PauseGameManager()
    {
        if (GameManager._instance.GetPauseStatus())
        {
            GameManager._instance.PauseGame();
        }
    }

    private void Update()
    {
        if(timeEvents.Count == 0 && translateEvents.Count == 0 && !GameManager._instance.GetPauseStatus())
        {
            GameManager._instance.UnpauseGame();
        }
        Timer += 1 * Time.deltaTime;
        {
            List<TimeEvent> deleteEvents = new List<TimeEvent>();
            foreach (var item in timeEvents)
            {
                if (item.timeExecute <= Timer)
                {
                    item.func();
                    deleteEvents.Add(item);
                }
            }
            foreach (var item in deleteEvents)
            {
                timeEvents.Remove(item);
            }
        }
        {
            List<TranslateEvent> deleteEvents = new List<TranslateEvent>();
            foreach (var item in translateEvents)
            {
                if (item.tf.position != item.targetPos)
                {
                    if((item.tf.position - item.targetPos).magnitude <= item.speed * Time.deltaTime)
                    {
                        item.tf.position = item.targetPos;
                    }
                    else
                    {
                        item.tf.Translate((item.targetPos - item.tf.position).normalized * (item.speed * Time.deltaTime));
                    }
                    
                }
                else
                {
                    item.func();
                    deleteEvents.Add(item);
                    continue;
                }
                if(item.tf.position == item.targetPos)
                {
                    item.func();
                    deleteEvents.Add(item);
                }
            }
            foreach (var item in deleteEvents)
            {
                translateEvents.Remove(item);
            }
        }
        
    }
}
public struct TimeEvent{
    public AnimationManager.TestDelegate func;
    public float timeExecute;
}
public struct TranslateEvent
{
    public AnimationManager.TestDelegate func;
    public Transform tf;
    public Vector3 targetPos;
    public float speed;
}