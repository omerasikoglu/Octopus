using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MinionSet")]
public class MinionSetSO : ScriptableObject
{
    public List<MinionTypeSO> list;
}
