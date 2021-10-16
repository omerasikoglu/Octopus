using System;                                                                          //TODO: YOK ET BU SCRİPTİ!
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSystem
{
    public event System.EventHandler OnStarAmountChanged; //total star

    //private static readonly int[] requiedStarPerStage = new[] { 0, 1, 3, 5, 9 };

    private int currentStarAmount;
    private int maxStarAmount;
    //private int requiredStarForNextStage;

    //public StarSystem()
    //{
    //    currentStarAmount = 0;
    //}

    public void AddStar(int amount)
    {
        this.currentStarAmount += 1;

        if (OnStarAmountChanged != null)
        {
            OnStarAmountChanged?.Invoke(this, System.EventArgs.Empty);
        }
    }
    public int GetStarAmount()
    {
        return currentStarAmount;
    }

 




}
