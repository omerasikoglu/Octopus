using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    [Header("Stars")]
    public Transform pfStarWorld;

    [Header("Items")]
    public Transform pfItemWorld;

    [Header("Items")]
    public Sprite swordSprite;
    public Sprite armorSprite;
    public Sprite healthPotionSprite;
    public Sprite manaPotionSprite;
    public Sprite energyPotionSprite;
    public Sprite coinSprite;
    public Sprite medkitSprite;
    public Sprite luckSprite;

    [Header("ItemBG")]
    public Sprite swordSpriteBG;
    public Sprite armorSpriteBG;
    public Sprite healthPotionSpriteBG;
    public Sprite manaPotionSpriteBG;
    public Sprite energyPotionSpriteBG;
    public Sprite coinSpriteBG;
    public Sprite medkitSpriteBG;
    public Sprite luckSpriteBG;

    [Header("ShopItemBG")]
    public Sprite shopSwordBG;
    public Sprite shopArmorBG;
    public Sprite shopHealthPotionBG;
    public Sprite shopManaPotionBG;
    public Sprite shopEnergyPotionBG;
    public Sprite shopCoinBG;
    public Sprite shopMedkitBG;
    public Sprite shopLuckBG;

}
