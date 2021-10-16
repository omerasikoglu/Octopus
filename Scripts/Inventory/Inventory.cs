using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public event System.EventHandler OnItemListChanged;

    private SaveSO saveFile;
    private List<Item> itemList;
    private Action<Item> useItemAction; //kullandığında bi şey olcaklar
    private Action<Item, bool> passiveItemAction; //envantere aldığında bi şey olcaklar

    public Inventory(Action<Item> useItemAction, Action<Item, bool> passiveItemAction)
    {
        saveFile = Resources.Load<SaveSO>(typeof(SaveSO).Name);

        itemList = new List<Item>();

        this.useItemAction = useItemAction;
        this.passiveItemAction = passiveItemAction;

    }
    public void AddItem(Item item)
    {
        if (!item.IsClickable()) PassiveItem(item, true);

        if (item.IsStackable())
        {
            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }
            if (!itemAlreadyInInventory)
            {
                itemList.Add(item);
            }
        }
        else
        {
            itemList.Add(item);
        }

        OnItemListChanged?.Invoke(this, System.EventArgs.Empty);
    }

    public void RemoveItem(Item item)
    {
        if (!item.IsClickable()) PassiveItem(item, false);

        if (item.IsStackable() || item.IsClickable())
        {
                Item itemInInventory = null;

                foreach (Item inventoryItem in itemList)
                {
                    if (inventoryItem.itemType == item.itemType)
                    {
                        inventoryItem.amount -= 1; //item listten 1 item kaldırır
                        itemInInventory = inventoryItem;
                    }
                }
                if (itemInInventory != null && itemInInventory.amount <= 0)
                {
                    itemList.Remove(itemInInventory);
                    saveFile.gottenItemList.Remove(item);
                }
        }
        else
        {
            itemList.Remove(item);
            saveFile.gottenItemList.Remove(item);
        }

        OnItemListChanged?.Invoke(this, System.EventArgs.Empty);
    }

    public void UseItem(Item item)
    {
        useItemAction(item);
    }

    public void PassiveItem(Item item, bool isAdding)
    {
        passiveItemAction(item, isAdding);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }

    public int GetItemAmount()
    {
        return itemList.Count;
    }

}
//AddItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
//AddItem(new Item { itemType = Item.ItemType.ManaPotion, amount = 1 });
//AddItem(new Item { itemType = Item.ItemType.Medkit, amount = 1 });
//AddItem(new Item { itemType = Item.ItemType.Sword, amount = 1 });