using System;                                               // VİCDANIM KADAR TEMİZ
using System.Collections;                                   // VİCDANIM KADAR TEMİZ
using System.Collections.Generic;                           // VİCDANIM KADAR TEMİZ
using UnityEngine;                                          // VİCDANIM KADAR TEMİZ
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    private Transform shopItemTemplate;
    private MouseEnterExitEvents mouseEnterExitEvents;

    //Datalarımız
    private SaveSO saveFile;

    //Dependencies
    private IShopCustomer shopCustomer;


    private void Awake()
    {
        saveFile = Resources.Load<SaveSO>(typeof(SaveSO).Name);

        shopItemTemplate = transform.Find("ShopItemTemplate");
        shopItemTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        foreach (Item item in saveFile.fullItemList)
        {
            Transform shopItemTransform = Instantiate(shopItemTemplate, transform);

            shopItemTransform.Find("button").GetComponent<Image>().sprite = item.GetShopButtonBG();
            shopItemTransform.Find("button").Find("itemText").GetComponent<TextMeshProUGUI>().SetText(item.GetString());
            shopItemTransform.Find("button").Find("priceText").GetComponent<TextMeshProUGUI>().SetText(item.GetPrice().ToString() + " Gold");
            shopItemTransform.Find("button").Find("itemBG").GetComponent<Image>().sprite = item.GetShopButtonBG();
            shopItemTransform.Find("button").Find("itemBG").Find("itemImage").GetComponent<Image>().sprite = item.GetSprite();
            shopItemTransform.gameObject.SetActive(true);

            shopItemTransform.Find("button").GetComponent<Button>().onClick.AddListener(() =>
            {
                shopCustomer.ItemSold(new Item { itemType = item.itemType, amount = item.amount }); ;
            });

            mouseEnterExitEvents = shopItemTransform.Find("button").GetComponent<MouseEnterExitEvents>();
            mouseEnterExitEvents.OnMouseEnter += (object sender, System.EventArgs e) =>
            {
                Tooltip_ItemInfo.Instance.Show(item.GetString(), item.GetFeatureText(), item.GetDescription(), item.GetShopButtonBG());
                shopItemTransform.Find("button").GetComponent<Image>().color = Color.magenta;
            };
            mouseEnterExitEvents.OnMouseExit += (object sender, System.EventArgs e) =>
            {
                Tooltip_ItemInfo.Instance.Hide();
                shopItemTransform.Find("button").GetComponent<Image>().color = Color.white;
            };

        }

    }
    private void Update()
    {
        //hile hurda
        if (Input.GetKeyDown(KeyCode.K)) saveFile.gottenItemList.Clear();
    }
    //private void SaveToSaveFile(Item clickedItem)
    //{
    //    bool itemAlreadyInInventory = false;
    //    foreach (Item gottenItem in saveFile.gottenItemList)
    //    {
    //        if (gottenItem.itemType == clickedItem.itemType && clickedItem.IsStackable()) //hali hazırda bu item varsa sende
    //        {
    //            gottenItem.amount += 1;

    //            itemAlreadyInInventory = true;
    //        }
    //    }
    //    if (!itemAlreadyInInventory) //ilk defa ekliyorsan
    //    {
    //        saveFile.gottenItemList.Add(new Item { itemType = clickedItem.itemType, amount = 1 }); ;
    //    }
    //}
       
    
    public void SetShopCustomer(IShopCustomer shopCustomer)
    {
        this.shopCustomer = shopCustomer;
    }


    //internal void SetPlayer(PlayerController player)
    //{
    //    this.player = player;
    //}
}

//Tooltip_ItemStatsUI.Instance.Show("hasan", "asdasd", "5", "4", "3", "2", "1", "2", "3");
//Tooltip_ItemStatsUI.Instance.Hide();

//public event System.EventHandler<OnItemSoldedEventArgs> OnItemSolded;
//public class OnItemSoldedEventArgs
//{
//    public Item item;
//}
//OnItemSolded?.Invoke(this, new OnItemSoldedEventArgs { item = item }); ;