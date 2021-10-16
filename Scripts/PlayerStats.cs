using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
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
    public StatType statType;

    public PlayerStatSO damage;
    public PlayerStatSO health;
    public PlayerStatSO energy;
    public PlayerStatSO speed;
    public PlayerStatSO dodge;
    public PlayerStatSO critic;

    public PlayerStats(int damage, int health, int energy, int speed, int dodge, int critic)
    {
        this.damage.amount = damage;
        this.health.amount = health;
        this.energy.amount = energy;
        this.speed.amount = speed;
        this.dodge.amount = dodge;
        this.critic.amount = critic;
    }
    public StatType GetStatType(PlayerStatSO playerStat)
    {
        switch (statType)
        {
            case StatType.Damage: return StatType.Damage;
            case StatType.Health: return StatType.Health;
            case StatType.Speed: return StatType.Speed;
            case StatType.Energy: return StatType.Energy;
            case StatType.Charisma: return StatType.Charisma;
            case StatType.Dodge: return StatType.Dodge;
            case StatType.Critical: return StatType.Critical;
            default: return StatType.None;
        }
    }
    public int GetStatValue(PlayerStatSO.StatType statType)
    {
        switch (statType)
        {
            case PlayerStatSO.StatType.Damage: return damage.amount;
            case PlayerStatSO.StatType.Health: return health.amount;
            case PlayerStatSO.StatType.Speed: return speed.amount;
            case PlayerStatSO.StatType.Energy: return energy.amount;
            case PlayerStatSO.StatType.Dodge: return dodge.amount;
            case PlayerStatSO.StatType.Critical: return critic.amount;
            default: return 0;
        }
    }


}
