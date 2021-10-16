using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSO", menuName = "LevelDesign/Level")]

public class LevelSO : ScriptableObject
{
    [Header("[Level Data]")]
    public int sceneNumber;
    public int requiedStarAmount;
    public bool isPlayed;
    public bool isBossLevel;

    [Header("[3 Star Data]")]
    public Sprite noStar;
    public List<ResourceTypeSO> starTypesList;
    public List<bool> achieveList;

}


//private Dictionary<StarTypeSO, bool> doneList;
//private void Awake()
//{
//    int index = 0;
//    foreach (StarTypeSO starType in starTypesList)
//    {
//        doneList[starType] = achieveList[index];
//    }
//}