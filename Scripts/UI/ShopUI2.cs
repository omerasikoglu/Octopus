using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopUI2 : MonoBehaviour
{
    private Transform shopItemTemplate;
    private MouseEnterExitEvents mouseEnterExitEvents;

    //Datalarýmýz
    private SaveSO saveFile;

    //Dependencies
    private IShopCustomer shopCustomer;


    private void Awake()
    {
        saveFile = Resources.Load<SaveSO>(typeof(SaveSO).Name);

        shopItemTemplate = transform.Find("ItemList").Find("ScrollView").Find("Viewport").Find("Content").Find("ShopItemTemplate");
        shopItemTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        foreach (Item item in saveFile.fullItemList)
        {
            Transform shopItemTransform = Instantiate(shopItemTemplate, transform.Find("ItemList").Find("ScrollView").Find("Viewport").Find("Content").transform);

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

    public void SetShopCustomer(IShopCustomer shopCustomer)
    {
        this.shopCustomer = shopCustomer;
    }



}

