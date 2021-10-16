using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerStatList")]
public class PlayerStatListSO : ScriptableObject
{
    public List<PlayerStatSO> list;
}
