using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private LevelSO currentLevel;
    private LevelListSO levelList;

    private void Awake()
    {
        levelList = Resources.Load<LevelListSO>(typeof(LevelListSO).Name);
        //ResourceManager.Instance.SetCurrentLevel(currentLevel);
        //if (currentLevel == null) currentLevel = levelList.list[0];
    }

    private void Start()
    {
        Instance = this;
    }

    public LevelSO GetCurrentLevel()
    {
        return currentLevel;
    }
    public void SetCurrentLevel(LevelSO currentLevel)
    {
        this.currentLevel = currentLevel;
    }
    public int GetIndexFromStar(ResourceTypeSO resourceType)
    {
        int index = 0;
        foreach (ResourceTypeSO st in currentLevel.starTypesList)
        {
            if (resourceType == st)
            {
                return index;
            }
            index++;
        }
        return -1;
    }
}
