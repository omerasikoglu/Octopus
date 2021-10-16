using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerStat")]

public class PlayerStatSO : ScriptableObject
{
    public StatType statType;

    [Header("ID")]
    public string nameString;
    public Sprite sprite;
    public string tooltipString;
    public int amount;

    public enum StatType
    {
        None,
        //essentials
        Damage,
        Health,
        Speed,
        Energy,
        Charisma, //  10/10 all the time
        //sonradan eklenecekler
        Dodge,  // yüzdelik oran dodge'un 5 ise yüzde 5 dodge'un olur
        Critical,
    }
   
}
