//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyUI : MonoBehaviour
{
    [SerializeField] private EnergySystem energySystem;

    private Transform barTransform;
    private Transform backgroundTransform;
    private Transform borderTransform;

    private float barOffsetAmount = 20f;
    private float barsizeDeltaXAmount = 40f;
    private float barSizeDeltaYAmount = 10f;
    private float extraBorderAmount = 5f;

    private MouseEnterExitEvents mouseEnterExitEvents;

    private void Awake()
    {
        borderTransform = transform.Find("border");
        backgroundTransform = transform.Find("background");
        barTransform = transform.Find("bar");
    }

    private void Start()
    {
        UpdateBarSizes(); // level atladıgında değişen barlar
        UpdateEnergyBar(); //current energy bar

        energySystem.OnEnergyAmountChanged += EnergySystem_OnEnergyGenerated;
        energySystem.OnMaxEnergyAmountIncreased += EnergySystem_OnMaxEnergyAmountIncreased;

        mouseEnterExitEvents = transform.GetComponent<MouseEnterExitEvents>();
        mouseEnterExitEvents.OnMouseEnter += (object sender, System.EventArgs e) => { TooltipUI.Instance.Show($"Current Energy: <color=#C86100>{energySystem.GetEnergyAmount()}</color>"); };
        mouseEnterExitEvents.OnMouseExit += (object sender, System.EventArgs e) => { TooltipUI.Instance.Hide(); };
    }

    private void EnergySystem_OnMaxEnergyAmountIncreased(object sender, System.EventArgs e)
    {
        UpdateBarSizes();
        UpdateEnergyBar();
    }

    private void EnergySystem_OnEnergyGenerated(object sender, System.EventArgs e)
    {
         UpdateEnergyBar();
    }
   
    private void UpdateBarSizes()
    {
        UpdateBorderSize();
        UpdateBackgroundSize();
        UpdateEnergyBarSize();
    }

    private void UpdateBorderSize()
    {
        float maxEnergyAmount = energySystem.GetEnergyAmountMax();
        borderTransform.transform.GetComponent<RectTransform>().localPosition = new Vector3
            (barOffsetAmount * maxEnergyAmount, 0f, 0f);
        borderTransform.transform.GetComponent<RectTransform>().sizeDelta = new Vector2
            (extraBorderAmount + barsizeDeltaXAmount * maxEnergyAmount, barSizeDeltaYAmount + extraBorderAmount);
    }

    private void UpdateBackgroundSize()
    {
        float maxEnergyAmount = energySystem.GetEnergyAmountMax();
        backgroundTransform.transform.GetComponent<RectTransform>().sizeDelta = new Vector2
            (barsizeDeltaXAmount * maxEnergyAmount, barSizeDeltaYAmount);
    }

    private void UpdateEnergyBarSize()
    {
        Transform bar = barTransform.Find("barImage");
        float maxEnergyAmount = energySystem.GetEnergyAmountMax();
        bar.transform.GetComponent<RectTransform>().localPosition = new Vector3
            (barOffsetAmount * maxEnergyAmount, 0f, 0f);
        bar.transform.GetComponent<RectTransform>().sizeDelta = new Vector3
            (barsizeDeltaXAmount * maxEnergyAmount, barSizeDeltaYAmount);
    }

    private void UpdateEnergyBar()
    {
        barTransform.localScale = new Vector3(energySystem.GetEnergyAmountNormalized(), 1f, 1f);
    }




}
