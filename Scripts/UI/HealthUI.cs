//using System;                                               //
using System.Collections;                                   //
using System.Collections.Generic;                           //<RectTransform>().localPosition --> posx
using UnityEngine;                                          //<RectTransform>().sizeDelta --> width height -->alttaki
using UnityEngine.UI;                                       //
                                                            //
public class HealthUI : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;  //TODO: Serialize'i kaldır , Instance'tan ulaş ?

    private Transform barTransform;
    private Transform lerpBarTransform;
    private Transform seperatorContainerTransform;
    private Transform bgTransform;
    private Transform borderTransform;

    private float lerpSpeed = 3f;

    private float barOffsetAmount = 20f;
    private float barsizeDeltaXAmount = 40f;
    private float barSizeDeltaYAmount = 20f;
    private float extraBorderAmount = 5f;

    private MouseEnterExitEvents mouseEnterExitEvents;
    private Dictionary<int, Transform> extraSeperatorDictionary; //fazladan oluşan seperatorları destroy etmek için

    private void Awake()
    {
        borderTransform = transform.Find("border");
        bgTransform = transform.Find("background");     //gri bar
        barTransform = transform.Find("bar");           //yeşil bar
        lerpBarTransform = transform.Find("lerpBar");   //beyaz bar
        extraSeperatorDictionary = new Dictionary<int, Transform>();
    }

    private void Start()
    {
        UpdateOutsideBarSizes(); // level atladıgında değişen barlar
        UpdateInsideBars(); // canın değiştiğinde değişen barlar

        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnMaxHealthAmountIncreased += HealthSystem_OnMaxHealthAmountIncreased;
        healthSystem.OnHealthAmountIncreased += HealthSystem_OnHealthAmountIncreased;

        mouseEnterExitEvents = transform.GetComponent<MouseEnterExitEvents>();
        mouseEnterExitEvents.OnMouseEnter += (object sender, System.EventArgs e) => { TooltipUI.Instance.Show($"Current Health: <color=#FF0000>{healthSystem.GetHealthAmount()+"/"+healthSystem.GetHealthAmountMax()}</color>"); };
        mouseEnterExitEvents.OnMouseExit += (object sender, System.EventArgs e) => { TooltipUI.Instance.Hide(); };
    }

    private void HealthSystem_OnHealthAmountIncreased(object sender, System.EventArgs e)
    {
        UpdateInsideBars();
    }

    private void HealthSystem_OnMaxHealthAmountIncreased(object sender, System.EventArgs e)
    {
        UpdateOutsideBarSizes();
        UpdateInsideBars();
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        UpdateInsideBars();
    }
    private void Update()
    {
        CheckLerp();
    }
    private void UpdateInsideBars()
    {
        barTransform.localScale = new Vector3(healthSystem.GetHealthAmountNormalized(), 1f, 1f);
        lerpBarTransform.localScale = new Vector3(healthSystem.GetLastHealthAmountNormalized(), 1f, 1f);
    }
    private void CheckLerp()
    {
        lerpBarTransform.localScale = new Vector3(Mathf.Lerp
        (lerpBarTransform.localScale.x, barTransform.localScale.x, lerpSpeed * Time.deltaTime), 1f, 1f);
    }
    private void UpdateOutsideBarSizes()
    {
        UpdateBorderSize();
        UpdateBackgroundSize();
        UpdateHealthBarSize();
        UpdateLerpBarSize();
        UpdateSeperatorContainerSize();
    }
    private void UpdateBorderSize()
    {
        int healthCount = healthSystem.GetHealthAmountMax();
        borderTransform.transform.GetComponent<RectTransform>().localPosition = new Vector3
            (barOffsetAmount * (int)healthCount, 0f, 0f);
        borderTransform.transform.GetComponent<RectTransform>().sizeDelta = new Vector2
            (barsizeDeltaXAmount * (int)healthCount, barSizeDeltaYAmount + extraBorderAmount);
    }
    private void UpdateBackgroundSize()
    {
        int healthCount = healthSystem.GetHealthAmountMax();
        bgTransform.transform.GetComponent<RectTransform>().sizeDelta = new Vector2
            (barsizeDeltaXAmount * (int)healthCount, barSizeDeltaYAmount);
    }
    private void UpdateHealthBarSize()
    {
        Transform bar = barTransform.Find("barImage");
        int healthCount = healthSystem.GetHealthAmountMax();
        bar.transform.GetComponent<RectTransform>().localPosition = new Vector3
            (barOffsetAmount * (int)healthCount, 0f, 0f);
        bar.transform.GetComponent<RectTransform>().sizeDelta = new Vector2
            (barsizeDeltaXAmount * (int)healthCount, barSizeDeltaYAmount);
    }
    private void UpdateLerpBarSize()
    {
        Transform bar = lerpBarTransform.Find("barImage");
        int healthCount = healthSystem.GetHealthAmountMax();
        bar.transform.GetComponent<RectTransform>().localPosition = new Vector3
            (barOffsetAmount * (int)healthCount, 0f, 0f);
        bar.transform.GetComponent<RectTransform>().sizeDelta = new Vector2
            (barsizeDeltaXAmount * (int)healthCount, barSizeDeltaYAmount);
    }
    private void UpdateSeperatorContainerSize()
    {
        seperatorContainerTransform = transform.Find("seperatorContainer");
        Transform seperatorTemplate = seperatorContainerTransform.Find("seperatorTemplate");
        int seperatorTemplateCount = healthSystem.GetHealthAmountMax();

        for (int i = 1; i < extraSeperatorDictionary.Keys.Count + 1; i++)
        {
            Transform istenmeyen = extraSeperatorDictionary[i];
            Destroy(istenmeyen.gameObject);
        }
        extraSeperatorDictionary.Clear();

        for (int i = 1; i < seperatorTemplateCount + 1; i++) //total'den bir eksik
        {
            Transform templateTransform = Instantiate(seperatorTemplate, seperatorContainerTransform);
            templateTransform.localPosition = new Vector3(barsizeDeltaXAmount * i, 0f, 0f);
            extraSeperatorDictionary[i] = templateTransform;
        }
    }

}
