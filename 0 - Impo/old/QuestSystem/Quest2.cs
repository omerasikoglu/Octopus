using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest2 : MonoBehaviour
{
    public enum SpawnMode
    {
        CloneAndEnable,
        CloneOnly
    }

    [System.Serializable]
    public class ItemRequirement
    {
        public Item item;
        public int count = 1;
    }

    public string title;
    public string desc;
    public ConversationScript questInProgressConversation, questCompletedConversation;

    public SpawnMode spawnMode = SpawnMode.CloneAndEnable;
    bool disableItemsOnStart = true;

    public GameObject[] enableOnQuestStart;
    public GameObject[] spawnOnQuestStart;
    public ItemRequirement[] requiredItems;
    public GameObject[] spawnOnQuestComplete;
    public Item[] rewardItems;

    public bool destroySpawnsOnQuestComplete = true;

    //public Cutscene introCutscenePrefab, outroCutscenePrefab;

    List<GameObject> cleanup = new List<GameObject>();

    public bool isStarted = false;
    public bool isFinished = false;


}
