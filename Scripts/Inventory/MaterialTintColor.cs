using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialTintColor : MonoBehaviour
{
    private Material material;
    private Color materialTintColor;
    private float tintFadeSpeed = 5f;

    private void Awake()
    {
        materialTintColor = new Color(1, 0, 0, 0);
        SetMaterial(GetComponent<SpriteRenderer>().material);
    }
    private void Update()
    {
        if (materialTintColor.a > 0)
        {
            materialTintColor.a = Mathf.Clamp01(materialTintColor.a - tintFadeSpeed * Time.deltaTime);
            material.SetColor("_Tint", materialTintColor);
        }
    }
    public void SetMaterial(Material material)
    {
        this.material = material;
    }
    public void SetTintColor(Color color)
    {
        this.materialTintColor = color;
        material.SetColor("_Tint", materialTintColor);
    }
    public void SetTintFadeSpeed(float tintFadeSpeed)
    {
        this.tintFadeSpeed = tintFadeSpeed;
    }
}
