using System.Collections;                                              //       useableItems=  new Color(59, 152, 58);
using System.Collections.Generic;                                      //       nonUseableItems = new Color(140, 152, 58);
using UnityEngine;                                                     //       specialItems = new Color(111, 111, 111);
using UnityEngine.UI;                                                  //
using TMPro;                                                           //                                                        
                                                                       //
public class InventoryUI : MonoBehaviour
{
    private PlayerController player;
    private Inventory inventory;

    private Transform itemSlot; //inventory sayfa 1

    private float itemSpacing;
    private float itemCellSizeX;
    private float borderCellSizeY = 88f; // TODO: daha sonra starttan ön tanımlı yapabilirsin, hard code alert
    private float bgQuadCellSizeY = 74f;

    private Transform background; //tüm borderları içine alan alan
    private Transform borderShadow;
    private Transform border;
    private Transform bgQuad; //itemların arkasındaki dolgulu çerçeve alan

    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;

    private void Awake()
    {
        itemSlot = transform.Find("itemSlot");

        background = itemSlot.Find("background");
        borderShadow = background.Find("borderShadow");
        border = background.Find("border");
        bgQuad = background.Find("bgQuad");

        itemSlotContainer = itemSlot.Find("itemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");

        itemCellSizeX = itemSlotContainer.GetComponent<GridLayoutGroup>().cellSize.x;
        itemSpacing = itemSlotContainer.GetComponent<GridLayoutGroup>().spacing.x;


    }

    public void SetPlayer(PlayerController player)
    {
        this.player = player; //item dropladıgımızda player'ın transformunu almak için
        SetInventory();
    }

    private void SetInventory()
    {
        inventory = player.GetInventory();
        inventory.OnItemListChanged += Inventory_OnItemListChanged;

        RefreshInventoryItems(); // ilk hangi inv bizim tanımladıgımızda çalışır
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue; //template dışındaki her şeyi yok ettik
            Destroy(child.gameObject);
        }

        foreach (Item item in inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            itemSlotRectTransform.transform.GetComponent<Button>().onClick.AddListener(() =>
            {
                inventory.UseItem(item);
            });
            itemSlotRectTransform.transform.GetComponent<MouseEnterExitEvents>().OnRightClickEnter += (object senter, System.EventArgs e) =>
              {
                  HealthSystem hs = player.GetComponent<HealthSystem>();

                  if (hs.GetHealthAmount() - item.GetChangedHealthPoint() <= 0) //itemi çıkarınca canın eksiye iniyorsa
                  { /*  o_o  */ }
                  else
                  {
                      Item duplicateItem = new Item { itemType = item.itemType, amount = 1 }; //envanterden 1 item kaldırır
                      inventory.RemoveItem(item);                                           //item.amount
                      ItemWorld.DropItem(player.GetPosition(), duplicateItem);
                      TooltipUI.Instance.Hide();
                  }
              };
            itemSlotRectTransform.transform.GetComponent<MouseEnterExitEvents>().OnMouseEnter += (object sender, System.EventArgs e) => { TooltipUI.Instance.Show($"<b><color=#FFFFFF>{item.itemType}</b></color>"); };
            itemSlotRectTransform.transform.GetComponent<MouseEnterExitEvents>().OnMouseExit += (object sender, System.EventArgs e) => { TooltipUI.Instance.Hide(); };

            Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
            image.sprite = item.GetSprite();

            TextMeshProUGUI uiText = itemSlotRectTransform.Find("amountText").GetComponent<TextMeshProUGUI>();
            if (item.amount > 1)
            {
                uiText.SetText(item.amount.ToString());
            }
            else
            {
                uiText.SetText("");
            }
        }
        UpdateSlotSizes();
    }

    private void UpdateSlotSizes()
    {
        UpdateBorderShadowSize();
        UpdateBGQuadSize();
        UpdateBorderSize();
    }
    private void UpdateBorderShadowSize()
    {
        int itemCount = inventory.GetItemAmount();
        if (itemCount == 0) borderShadow.gameObject.SetActive(false);
        else if (itemCount <= 10)
        {
            borderShadow.transform.GetComponent<RectTransform>().sizeDelta = new Vector2
            (itemCount * itemCellSizeX + (itemCount + 3) * itemSpacing, borderCellSizeY);
            borderShadow.gameObject.SetActive(true);
        }
    }
    private void UpdateBGQuadSize()
    {
        int itemCount = inventory.GetItemAmount();
        if (itemCount == 0) bgQuad.gameObject.SetActive(false);
        else if (itemCount <= 10)
        {
            bgQuad.transform.GetComponent<RectTransform>().sizeDelta = new Vector2
            (itemCount * itemCellSizeX + (itemCount + 1) * itemSpacing, bgQuadCellSizeY);
            bgQuad.gameObject.SetActive(true);
        }


    }
    private void UpdateBorderSize()
    {
        int itemCount = inventory.GetItemAmount();
        if (itemCount == 0) border.gameObject.SetActive(false);
        else if (itemCount <= 10)
        {
            border.transform.GetComponent<RectTransform>().sizeDelta = new Vector2
            (itemCount * itemCellSizeX + (itemCount + 3) * itemSpacing, borderCellSizeY);
            border.gameObject.SetActive(true);
        }
    }
}
