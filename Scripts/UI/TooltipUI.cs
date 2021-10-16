using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance { get; private set; }

    [SerializeField] private RectTransform canvasRectTransform;
    private TextMeshProUGUI textMeshPro;
    private RectTransform backgroundRectTransform;
    private RectTransform rectTransform;
    private TooltipTimer tooltipTimer;
    private void Awake()
    {
        Instance = this;
        rectTransform = transform.GetComponent<RectTransform>();
        textMeshPro = transform.Find("text").GetComponent<TextMeshProUGUI>();
        backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();
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
        if (anchoredPosition.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height)
        {
            anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;
        }

        rectTransform.anchoredPosition = anchoredPosition;
    }
    private void SetText(string tooltipText)
    {                                          //yazacak yazı
        textMeshPro.SetText(tooltipText);
        textMeshPro.ForceMeshUpdate();
        //arka planı çizdiririz
        Vector2 padding = new Vector2(8, 8);
        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        backgroundRectTransform.sizeDelta = textSize + padding; //sizedelta: anchorlar arasındaki mesafe
    }
    public void Show(string tooltipText, TooltipTimer tooltipTimer = null)
    {
        this.tooltipTimer = tooltipTimer;

        if (tooltipTimer == null)
        {
            this.tooltipTimer = new TooltipTimer { timer = 3f };
        }
        gameObject.SetActive(true);
        SetText(tooltipText);
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
