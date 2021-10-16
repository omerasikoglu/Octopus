using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RecourceType")]
public class ResourceTypeSO : ScriptableObject
{
    //public enum ResourceType
    //{
    //    None,
    //    Gold,
    //    Star,
    //}

    public string nameString;
    public Sprite sprite;
    public string colorHex;
    public Color bgColor; // renkleri kaybetmemek için henüz çıkarmadım TODO: çıkar zamanı gleince
    public string tooltipString;
    public int totalAmount;
    public int remainedAmount;

    public ResourceManager.ResourceType resourceType;
    //public ResourceType bgColorNormalize;


    public Color GetColor()
    {
        switch (resourceType)
        {
            default:
            case ResourceManager.ResourceType.Gold: return new Color(.15f, .05f, .04f);
            case ResourceManager.ResourceType.Star: return new Color(.62f, .43f, .03f);
            case ResourceManager.ResourceType.None: return new Color(1f, 1f, 1f);
        }
    }
}
