using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelListSO", menuName = "LevelDesign/LevelList")]

public class LevelListSO : ScriptableObject
{
    public List<LevelSO> list;
}
