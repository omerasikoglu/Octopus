using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip_ItemStatsUI : MonoBehaviour
{
    public static Tooltip_ItemStatsUI Instance { get; private set; }

    [SerializeField] private RectTransform canvasRectTransform;
    private RectTransform backgroundRectTransform;
    private RectTransform rectTransform;
    private TooltipTimer tooltipTimer;
    //private Image image;

    private GameObject itemStatTextContainer;

    private TextMeshProUGUI itemNameText;
    private TextMeshProUGUI descriptionText;
    private TextMeshProUGUI damageAmountText;
    private TextMeshProUGUI hpAmountText;
    private TextMeshProUGUI energyAmountText;
    private TextMeshProUGUI ArmorAmountText;
    private TextMeshProUGUI speedAmountText;
    private TextMeshProUGUI luckAmountText;
    private TextMeshProUGUI criticAmountText;

    private void Awake()
    {
        Instance = this;
        rectTransform = transform.GetComponent<RectTransform>();
        backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();
        //image = transform.Find("image").GetComponent<Image>();

        itemNameText = transform.Find("itemNameText").GetComponent<TextMeshProUGUI>();
        descriptionText = transform.Find("descriptionText").GetComponent<TextMeshProUGUI>();

        itemStatTextContainer = transform.Find("itemStatTextContainer").gameObject;

        damageAmountText = itemStatTextContainer.transform.Find("dmgText").GetComponent<TextMeshProUGUI>();
        hpAmountText = itemStatTextContainer.transform.Find("hpText").GetComponent<TextMeshProUGUI>();
        energyAmountText = itemStatTextContainer.transform.Find("energyText").GetComponent<TextMeshProUGUI>();
        ArmorAmountText = itemStatTextContainer.transform.Find("armorText").GetComponent<TextMeshProUGUI>();
        speedAmountText = itemStatTextContainer.transform.Find("speedText").GetComponent<TextMeshProUGUI>();
        luckAmountText = itemStatTextContainer.transform.Find("luckText").GetComponent<TextMeshProUGUI>();
        criticAmountText = itemStatTextContainer.transform.Find("criticText").GetComponent<TextMeshProUGUI>();

        Hide();
    }
    private void Update()
    {
        HandleFollowMouse();

        if (tooltipTimer != null)
        {
            tooltipTimer.timer -= Time.deltaTime;
            if (tooltipTimer.timer <= 0)
            {
                Hide();
            }
        }
    }
    private void HandleFollowMouse()
    {
        Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;  //flexibility ekran boyutlarına göre


        if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width)
        {
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        }
        if (anchoredPosition.y - backgroundRectTransform.rect.height > canvasRectTransform.rect.height)
        {
            anchoredPosition.y = canvasRectTransform.rect.height + backgroundRectTransform.rect.height;
        }

        rectTransform.anchoredPosition = anchoredPosition;
    }
    private void SetText(string itemName, string description, string dmg, string hp, string energy, string armor,
        string speed, string luck, string critic)
    {                                          //yazacak yazı
        this.itemNameText.SetText(itemName);
        this.itemNameText.ForceMeshUpdate();
        this.descriptionText.SetText(description);
        this.descriptionText.ForceMeshUpdate();
        this.damageAmountText.SetText(dmg);
        this.damageAmountText.ForceMeshUpdate();
        this.hpAmountText.SetText(hp);
        this.hpAmountText.ForceMeshUpdate();
        this.energyAmountText.SetText(energy);
        this.energyAmountText.ForceMeshUpdate();
        this.ArmorAmountText.SetText(armor);
        this.ArmorAmountText.ForceMeshUpdate();
        this.speedAmountText.SetText(speed);
        this.speedAmountText.ForceMeshUpdate();
        this.luckAmountText.SetText(luck);
        this.luckAmountText.ForceMeshUpdate();
        this.criticAmountText.SetText(critic);
        this.criticAmountText.ForceMeshUpdate();

        //arka planı çizdiririz
        //Vector2 padding = new Vector2(8, 8);
        //Vector2 textSize = this.itemNameText.GetRenderedValues(false);
        //backgroundRectTransform.sizeDelta = textSize + padding; //sizedelta: anchorlar arasındaki mesafe
    }
    public void Show(string itemName, string description, string dmg, string hp, string energy, string armor,
        string speed, string luck, string critic, TooltipTimer tooltipTimer = null)
    {
        this.tooltipTimer = tooltipTimer;

        //if (tooltipTimer == null)
        //{
        //    this.tooltipTimer = new TooltipTimer { timer = 5f };
        //}
        gameObject.SetActive(true);
        SetText(itemName, description, "+" + dmg, "+" + hp, "+" + energy, "+" + armor, "+" + speed, "+" + luck, "+" + critic);
        HandleFollowMouse();    // 1 frame kaçırmamak için bunu ekledik diğer türlü ilk show sonra update çalışıyordu
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public class TooltipTimer
    {
        public float timer;
    }
}
