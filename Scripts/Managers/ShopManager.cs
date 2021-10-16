using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    public event System.EventHandler OnItemSold;

    [SerializeField] private List<Item> itemList;

    private int totalMoney;

    private void Awake()
    {
        Instance = this;

    }

    public void TryToBuyItem(Item item)
    {
        if (totalMoney > item.GetPrice())
        {
            BuyItem(item);
        }
        else
        {
            Debug.Log("Not enough money");
        }

           
    }
    private void BuyItem(Item item)
    {
        totalMoney -= item.GetPrice();
        OnItemSold?.Invoke(this, System.EventArgs.Empty);
    }

}
