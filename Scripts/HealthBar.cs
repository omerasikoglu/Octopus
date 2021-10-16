using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;

    private float effectSpeed = 1f;

    private Transform barTransform;
    private Transform effectTransform; 

    private void Awake()
    {
        barTransform = transform.Find("bar");
        effectTransform = transform.Find("effect");
    }
    private void Start()
    {
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        UpdateBar();
        UpdateHealthBarVisibilty();
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        UpdateBar();
        UpdateHealthBarVisibilty();
    }
    private void Update()
    {
        CheckEffectBar();
    }
    private void CheckEffectBar()
    {
        //efekt barın düşmesi
        if (effectTransform.localScale.x > barTransform.localScale.x)
        {
            float f = healthSystem.GetHealthAmountNormalized();
            effectTransform.localScale -= new Vector3(f, 1, 1) * effectSpeed * Time.deltaTime;
        }
        else
        {
            effectTransform.localScale = barTransform.localScale;
        }
    }
    private void UpdateBar()
    {
        barTransform.localScale = new Vector3(healthSystem.GetHealthAmountNormalized(), 1, 1);
    }
    private void UpdateHealthBarVisibilty()
    {
        if (healthSystem.HasFullHealth())
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
