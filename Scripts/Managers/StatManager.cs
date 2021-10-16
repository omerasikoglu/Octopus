using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public static StatManager Instance { get; private set; }
    public enum StatType
    {
        None,
        //essentials
        Damage,
        Health,
        Speed,
        Energy,
        Charisma, //  10/10 all the time
        Dodge,  // yüzdelik oran dodge'un 5 ise yüzde 5 dodge'un olur
        Critical,
    }

    public event System.EventHandler OnStatValuesChanged;

    private PlayerStatListSO playerStatList;

    private void Awake()
    {
        Instance = this;
        playerStatList = Resources.Load<PlayerStatListSO>(typeof(PlayerStatListSO).Name);
        SetZero();
    }
  
    public void IncreaseStatAmount(StatType statType, int amount)
    {
        if (playerStatList.list.Contains(GetStatSO(statType)))
        {
            GetStatSO(statType).amount += amount;
            //PlayerPrefs.SetInt(GetStatSO(statType).name, GetStatSO(statType).amount); //save güncelleme
            OnStatValuesChanged?.Invoke(this, System.EventArgs.Empty);
        }
        
    }
    public void SetStatAmount(StatType statType,int amount)
    {
        if (playerStatList.list.Contains(GetStatSO(statType)))
        {
            GetStatSO(statType).amount = amount;
            //PlayerPrefs.SetInt(GetStatSO(statType).name, GetStatSO(statType).amount); //save güncelleme
            OnStatValuesChanged?.Invoke(this, System.EventArgs.Empty);
        }
    }
    public void SetZero()
    {
        foreach (PlayerStatSO ps in playerStatList.list)
        {
            ps.amount = 0;
        }
    }
    public int GetStatAmount(StatType statType)
    {
        switch (statType)
        {
            case StatType.Damage:   return playerStatList.list[0].amount;
            case StatType.Health:   return playerStatList.list[1].amount;
            case StatType.Energy:   return playerStatList.list[2].amount;
            case StatType.Speed:    return playerStatList.list[3].amount;
            case StatType.Dodge:    return playerStatList.list[4].amount;
            case StatType.Critical: return playerStatList.list[5].amount;
            case StatType.Charisma: return playerStatList.list[6].amount;
            default:                return playerStatList.list[0].amount;
        }
    }
    public PlayerStatSO GetStatSO(StatType statType)
    {
        switch (statType)
        {
            case StatType.Damage:   return playerStatList.list[0];
            case StatType.Health:   return playerStatList.list[1];
            case StatType.Energy:   return playerStatList.list[2];
            case StatType.Speed:    return playerStatList.list[3];
            case StatType.Dodge:    return playerStatList.list[4];
            case StatType.Critical: return playerStatList.list[5];
            case StatType.Charisma: return playerStatList.list[6];
            default:                return playerStatList.list[0];
        }
    }
    

}
