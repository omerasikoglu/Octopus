using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MinionSetsList")]
public class MinionSetsListSO : ScriptableObject
{
    public List<MinionSetSO> list;
}
