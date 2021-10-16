using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip_ItemInfo : MonoBehaviour
{
    public static Tooltip_ItemInfo Instance { get; private set; }

    [SerializeField] private RectTransform canvasRectTransform;
    private RectTransform backgroundRectTransform;
    private RectTransform rectTransform;
    private TooltipTimer tooltipTimer;
    //private Image image;
    private Image borderImage;
    private TextMeshProUGUI itemNameText;
    private TextMeshProUGUI featureText;
    private TextMeshProUGUI descriptionText;


    private void Awake()
    {
        Instance = this;
        rectTransform = transform.GetComponent<RectTransform>();
        backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();

        borderImage = transform.Find("background").GetComponent<Image>();
        itemNameText = transform.Find("itemNameText").GetComponent<TextMeshProUGUI>();
        featureText = transform.Find("featureText").GetComponent<TextMeshProUGUI>();
        descriptionText = transform.Find("descriptionText").GetComponent<TextMeshProUGUI>();

      
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
        Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;  //flexibility ekran boyutlarýna göre


        if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width)
        {
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        }
        if (anchoredPosition.y - backgroundRectTransform.rect.height > canvasRectTransform.rect.height)
        {
            anchoredPosition.y = canvasRectTransform.rect.height + backgroundRectTransform.rect.height;
        }
        if (anchoredPosition.y - backgroundRectTransform.rect.height < 0)
        {
            anchoredPosition.y = 0 + backgroundRectTransform.rect.height;
        }

        rectTransform.anchoredPosition = anchoredPosition;
    }
    private void SetText(string itemName, string featureText , string description)
    {                                          //yazacak yazý
        this.itemNameText.SetText(itemName);
        this.itemNameText.ForceMeshUpdate();
        this.featureText.SetText(featureText);
        this.featureText.ForceMeshUpdate();
        this.descriptionText.SetText(description);
        this.descriptionText.ForceMeshUpdate();
    }
    public void Show(string itemName, string featureText, string description,Sprite borderSprite, TooltipTimer tooltipTimer = null)
    {
        this.tooltipTimer = tooltipTimer;
        this.borderImage.sprite = borderSprite;

        gameObject.SetActive(true);
        SetText(itemName,featureText, description);
        HandleFollowMouse();    // 1 frame kaçýrmamak için bunu ekledik diðer türlü ilk show sonra update çalýþýyordu
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
