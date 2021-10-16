using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public ConversationScript[] conversations;
    
    Quest2 activeQuest = null;

    Quest2[] quests;

    //GameModel model = Schedule.GetModel<GameModel>();

    void OnEnable()
    {
        quests = gameObject.GetComponentsInChildren<Quest2>();
    }


}
