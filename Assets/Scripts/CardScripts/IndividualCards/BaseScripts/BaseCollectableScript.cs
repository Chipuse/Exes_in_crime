using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCollectableScript : BaseCardScript
{
    public virtual void OnPickUp()
    {
        Debug.Log("Picked Up Collectible: " + data.Name);
        QuestManager._instance.currentQuest.UpdateQuest();
        //BaseQuest.currentQuest.UpdateQuest();
    }
}
