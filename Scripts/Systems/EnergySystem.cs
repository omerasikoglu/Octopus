using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySystem : MonoBehaviour
{
    public event System.EventHandler OnEnergyAmountChanged;
    public event System.EventHandler OnNotEnoughEnergy;
    public event System.EventHandler OnMaxEnergyAmountIncreased;

    [SerializeField] private float energyAmountMax;

    private EnergyTimer energyWaitingTimer; //tekrar enerji yenilenmeye başlama bekleme süresi timer'ı
    private float energyTimer; //bu kadar süre geçince belli bir enerjin yenilenir
    private float energyTimerMax = .1f;
    private float energyGenerateAmount = .1f;

    private float currentEnergyAmount;
    private bool isEnergyGenerating = true;
    //private float lastEnergyStopTime = Mathf.NegativeInfinity; //generate'in durduğu an

    private void Awake()
    {
        currentEnergyAmount = energyAmountMax;
        energyTimer = energyTimerMax;

    }

    private void Update()
    {
        EnergyGenerator();

    }
    private void EnergyGenerator()
    {
        if (energyWaitingTimer != null && !isEnergyGenerating) //null kısmı doğru değiştirme
        {
            energyWaitingTimer.timer -= Time.deltaTime;
            if (energyWaitingTimer.timer <= 0)
            {
                isEnergyGenerating = true;
            }
        }
        if (!HasFullEnergy() && isEnergyGenerating)
        {
            // Generate Energy
            energyTimer -= Time.deltaTime;
            if (energyTimer <= 0)
            {
                currentEnergyAmount += energyGenerateAmount;
                currentEnergyAmount = Mathf.Clamp(currentEnergyAmount, 0f, energyAmountMax);
                energyTimer += energyTimerMax;
                OnEnergyAmountChanged?.Invoke(this, System.EventArgs.Empty);
            }
        }
    }


    public bool SpendEnergy(float energyAmount, EnergyTimer energyTimer = null) //enerjiyi harvamaya çalışır.yeterli yoksa false döndürür
    {
        if (HaveEnoughEnergy(energyAmount))
        {
            this.energyWaitingTimer = energyTimer;

            if (energyTimer == null)
            {
                this.energyWaitingTimer = new EnergyTimer { timer = 1f };
            }
            currentEnergyAmount -= energyAmount;
            currentEnergyAmount = Mathf.Clamp(currentEnergyAmount, 0, energyAmountMax);
            isEnergyGenerating = false;
            OnEnergyAmountChanged?.Invoke(this, System.EventArgs.Empty);
            return true;
        }
        else
        {
            OnNotEnoughEnergy?.Invoke(this, System.EventArgs.Empty);
            //TooltipUI.Instance.Show("Not enough energy!", new TooltipUI.TooltipTimer { timer = .2f }) ; //TODO: klavyeden karakterin old yerde gözükcek şekilde ayarla
            return false;
        }

    }
    public class EnergyTimer //coroutine'in canı cehenneme
    {
        public float timer = 1f;
    }
    public int GetEnergyAmount() { return (int)currentEnergyAmount; }
    public int GetEnergyAmountMax()
    {
        return (int)energyAmountMax;
    }
    public float GetEnergyAmountNormalized() { return currentEnergyAmount / energyAmountMax; }
    private bool HaveEnoughEnergy(float needingEnergyAmount)
    {
        return currentEnergyAmount >= needingEnergyAmount ? true : false;
    }
    public bool HasFullEnergy()
    {
        return currentEnergyAmount >= energyAmountMax;
    }

    public void IncreaseMaxEnergyAmount(int amount)
    {
        energyAmountMax += amount;
        currentEnergyAmount = energyAmountMax;
        StatManager.Instance.IncreaseStatAmount(StatManager.StatType.Energy, amount);
        OnMaxEnergyAmountIncreased?.Invoke(this, System.EventArgs.Empty);
    }
    public void HealEnergy(int energyAmount)
    {
        currentEnergyAmount += energyAmount;
        currentEnergyAmount = Mathf.Clamp(currentEnergyAmount, 0, energyAmountMax);
        OnEnergyAmountChanged?.Invoke(this, System.EventArgs.Empty);
    }
}
