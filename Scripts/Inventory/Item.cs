using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item 
{
    public ItemType itemType;
    public int amount;

    public enum ItemType
        {
            Sword,
            HealthPotion,
            ManaPotion,
            Coin,
            Medkit,
            LuckItem,
            Armor,
            EnergyPotion
        }
    
    public Sprite GetSprite()
        {
        switch (itemType)
        {
            default:
            case ItemType.Sword: return ItemAssets.Instance.swordSprite;
            case ItemType.HealthPotion: return ItemAssets.Instance.healthPotionSprite;
            case ItemType.ManaPotion: return ItemAssets.Instance.manaPotionSprite;
            case ItemType.Coin: return ItemAssets.Instance.coinSprite;
            case ItemType.Medkit: return ItemAssets.Instance.medkitSprite;
            case ItemType.LuckItem: return ItemAssets.Instance.luckSprite;
            case ItemType.Armor: return ItemAssets.Instance.armorSprite;
            case ItemType.EnergyPotion: return ItemAssets.Instance.energyPotionSprite;
        }
    }
    public Sprite GetSpriteBG()
    {
        switch (itemType)
        {
            default:
            case ItemType.Sword: return ItemAssets.Instance.swordSpriteBG;
            case ItemType.HealthPotion: return ItemAssets.Instance.healthPotionSpriteBG;
            case ItemType.ManaPotion: return ItemAssets.Instance.manaPotionSpriteBG;
            case ItemType.Coin: return ItemAssets.Instance.coinSpriteBG;
            case ItemType.Medkit: return ItemAssets.Instance.medkitSpriteBG;
            case ItemType.LuckItem: return ItemAssets.Instance.luckSpriteBG;
            case ItemType.Armor: return ItemAssets.Instance.armorSpriteBG;
            case ItemType.EnergyPotion: return ItemAssets.Instance.energyPotionSpriteBG;
        }
    }
    public Sprite GetShopButtonBG() //aktiflik pasiflik itemleri tanımlar
    {
        switch (itemType)
        {
            default:
            case ItemType.Sword: return ItemAssets.Instance.shopSwordBG;
            case ItemType.HealthPotion: return ItemAssets.Instance.shopHealthPotionBG;
            case ItemType.ManaPotion: return ItemAssets.Instance.shopManaPotionBG;
            case ItemType.Coin: return ItemAssets.Instance.shopCoinBG;
            case ItemType.Medkit: return ItemAssets.Instance.shopMedkitBG;
            case ItemType.LuckItem: return ItemAssets.Instance.shopLuckBG;
            case ItemType.Armor: return ItemAssets.Instance.shopArmorBG;
            case ItemType.EnergyPotion: return ItemAssets.Instance.shopEnergyPotionBG;
        }
    }

    public Color GetColor()
    {
        switch (itemType)
        {
            default:
            case ItemType.Sword:        return new Color(1, 1, 1);
            case ItemType.HealthPotion: return new Color(1, 0, 0);
            case ItemType.ManaPotion:   return new Color(0, 0, 1);
            case ItemType.Coin:         return new Color(1, 1, 0);
            case ItemType.Medkit:       return new Color(1, 0, 1);
        }
    }

    public int GetPrice()
    {
        return itemType switch
        {                                             //default:
            ItemType.HealthPotion => 1,               //    case ItemType.Sword:        return 3;
            ItemType.ManaPotion => 1,                 //    case ItemType.HealthPotion: return 1;
            ItemType.Medkit => 5,                     //    case ItemType.ManaPotion:    return 1;
            ItemType.Coin => 2,                       //    case ItemType.Medkit:       return 5;
            ItemType.LuckItem => 3,                       //    case ItemType.Coin:       return 2;
            ItemType.Armor => 5,                      //    case ItemType.Luck:       return 3;
            ItemType.EnergyPotion => 1,                     //    case ItemType.Armor:      return 5;
            ItemType.Sword => 3,
            _ => 3,                                   //    case ItemType.Energy:       return 1;
        };
    }
    public string GetString()
    {
        switch (itemType)
        {
            default:
            case ItemType.Sword:        return "Sword";
            case ItemType.HealthPotion: return "Health Potion";
            case ItemType.ManaPotion:   return "Mana Potion";
            case ItemType.Coin:         return "Para Para";
            case ItemType.Medkit:       return "MedKit";
            case ItemType.LuckItem:         return "Yonca";
            case ItemType.Armor:        return "Armor";
            case ItemType.EnergyPotion:       return "Ekstazi";
        }
    }
    public string GetDescription()
    {
        return itemType switch
        {
            ItemType.Sword => "Sivri ucu sapla",   
            ItemType.HealthPotion => "İçelim, kendimizden geçelim",
            ItemType.ManaPotion => "Oyunda mana yok ha :D",
            ItemType.Coin => "para para para",
            ItemType.Medkit => "sar beni",
            ItemType.LuckItem => "yonca evcimik",
            ItemType.Armor => "kaslıydı",
            ItemType.EnergyPotion =>"kullanmayınız",
            _ => "404 Error",
        };
    }
    public string GetFeatureText()
    {
        return itemType switch
        {
            ItemType.Sword =>           "You have +1 DMG",
            ItemType.HealthPotion =>    "Restore 1 HP",
            ItemType.ManaPotion =>      "Restore your emotions",
            ItemType.Coin =>            "+1 Counterfeit Coin",
            ItemType.Medkit =>          "Restore full HP",
            ItemType.LuckItem =>            "+%5 DODGE CHANCE",
            ItemType.Armor =>           "Take +1 less DMG",
            ItemType.EnergyPotion =>          "Restore full Mana",
            _ => "404 Error",
        };
    }



    public bool IsStackable()
        {
            switch (itemType)
            {
                default:
                case ItemType.HealthPotion:
                case ItemType.ManaPotion:
                case ItemType.EnergyPotion:
                case ItemType.Coin:

                return true;
                case ItemType.Sword:
                case ItemType.Medkit:
                case ItemType.LuckItem:
                case ItemType.Armor:
                    return false;
            }
        }
    public bool IsClickable()
    {
        switch (itemType)
        {
            default:
            case ItemType.HealthPotion:
            case ItemType.ManaPotion:
            case ItemType.EnergyPotion:
            case ItemType.Medkit:

                return true;
            case ItemType.Coin:
            case ItemType.Sword:
            case ItemType.LuckItem:
            case ItemType.Armor:
                return false;
        }
    }
    public int GetChangedHealthPoint()
    {
        switch (itemType)
        {
            case ItemType.Armor: return 3;

            case ItemType.Sword:
            case ItemType.HealthPotion:
            case ItemType.ManaPotion:
            case ItemType.Coin:
            case ItemType.Medkit:
            case ItemType.LuckItem:
            case ItemType.EnergyPotion:
            default:        
                        return 0;
        }
    }
}

//public PlayerStatSO.StatType statType;

//    public PlayerStatSO.StatType GetStatType()
//{
//    switch (itemType)
//    {
//        case ItemType.Sword:        return PlayerStatSO.StatType.Damage;
//        case ItemType.Medkit:       return PlayerStatSO.StatType.Health;
//        case ItemType.HealthPotion: return PlayerStatSO.StatType.Health;
//        case ItemType.ManaPotion:   return PlayerStatSO.StatType.Energy;
//        case ItemType.Coin:         return PlayerStatSO.StatType.None;
//        default:                    return PlayerStatSO.StatType.None;

//    }
//}

//public Color GetColor() //color32
//    {
//        switch (itemType)
//        {
//            default:
//            case ItemType.Sword: return new Color(140, 152, 58);
//            case ItemType.HealthPotion: return new Color(59, 152, 58);
//            case ItemType.ManaPotion: return new Color(59, 152, 58);
//            case ItemType.Coin: return new Color(111, 111, 111);
//            case ItemType.Medkit: return new Color(59, 152, 58);
//        }
//    }