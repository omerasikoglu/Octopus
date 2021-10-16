using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills
{
    public event System.EventHandler OnSkillPointsChanged;
    public event System.EventHandler OnResetAllSkills;
    public event EventHandler<OnSkillUnlockedEventArgs> OnSkillUnlocked;
    public class OnSkillUnlockedEventArgs : EventArgs
    {
        public SkillType skillType;
    }

    public enum SkillType
    {
        None,
        //Passives
        Damage,
        Health,
        Energy,
        Speed,
        MoreDamage,
        MoreEnergy,
        MoreSpeed,
        MostDamage,
        MostEnergy,
        MostSpeed,
        //Skills
        Dash,
        DoubleJump,
    }

    //private SaveSO saveFile;
    private List<SkillType> unlockedSkillTypeList;

    //private int remainStarPoints; //toplam yıldız sayın

    public PlayerSkills(/*List<SkillType> skillTypeList*/)
    {
        //saveFile = Resources.Load<SaveSO>(typeof(SaveSO).Name);
        unlockedSkillTypeList = new List<SkillType>();

        //foreach (PlayerSkills.SkillType skill in saveFile.unlockedSkillList)
        //{
        //    UnlockSkill(skill);
        //}

        //foreach (SkillType skillType in skillTypeList) //önceden açtığımız skiller için
        //{
        //    UnlockSkill(skillType);
        //}
        


        //foreach (SkillType skillType in unlockedSkillTypeList)  Debug.Log(skillType);

    }
    //public void SetStarAmount(int starAmount)
    //{
    //    //this.remainStarPoints = starAmount;
    //    this.remainStarPoints =ResourceManager.Instance.GetRemainedResourceAmount(ResourceManager.Instance.GetResourceTypeSO(ResourceManager.ResourceType.Star));
    //}
    //public void AddStarPoint()
    //{
    //    remainStarPoints++;
    //    OnSkillPointsChanged?.Invoke(this, EventArgs.Empty);
    //}
    //public int GetStarPoints()
    //{
    //    return remainStarPoints;
    //}

    public bool IsSkillUnlocked(SkillType skillType) //liste skill'i içeriyor mu
    {
        if (unlockedSkillTypeList.Contains(skillType)) { return true; }
        else return false;
    }
    public bool CanUnlock(SkillType skillType) //skill'in önceki aşaması açılmış mı
    {
        SkillType skillRequirement = GetSkillRequirement(skillType);

        if (skillRequirement != SkillType.None)
        {
            if (IsSkillUnlocked(skillRequirement))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }
    public int GetSkillStarAmountForUnlock(SkillType skillType)
    {
        switch (skillType)
        {
            case SkillType.Damage: return 3;
            case SkillType.Health: return 1;
            case SkillType.Energy: return 2;
            case SkillType.Speed: return 3;
            case SkillType.MoreDamage: return 2;
            case SkillType.MoreEnergy: return 2;
            case SkillType.MoreSpeed: return 2;
            case SkillType.MostDamage: return 3;
            case SkillType.MostEnergy: return 3;
            case SkillType.MostSpeed: return 3;
            case SkillType.Dash: return 7;
            case SkillType.DoubleJump: return 5;
            default: return 0;
        }
    }
    public SkillType GetSkillRequirement(SkillType skillType)
    {
        switch (skillType)
        {
            case SkillType.Energy: return SkillType.Health;
            case SkillType.Damage: return SkillType.Energy;
            case SkillType.DoubleJump: return SkillType.Speed;
            case SkillType.Dash: return SkillType.DoubleJump;
        }
        return SkillType.None;
    }

    public string GetTooltipText(SkillType skilltype)
    {
        switch (skilltype)
        {
            case SkillType.None: return ("nope");
            case SkillType.Damage: return ("+1 damage");
            case SkillType.Health: return ("+1 health");
            case SkillType.Energy: return ("+1 energy");
            case SkillType.Speed: return ("+3 speed");
            case SkillType.MoreDamage: return ("+1 damage too");
            case SkillType.MoreEnergy: return ("nope");
            case SkillType.MoreSpeed: return ("nope");
            case SkillType.MostDamage: return ("nope");
            case SkillType.MostEnergy: return ("nope");
            case SkillType.MostSpeed: return ("nope");
            case SkillType.Dash: return ("dash ability");
            case SkillType.DoubleJump: return ("double jump ability");
            default: return ("nope");
        }
    }
    public void ResetSkills()
    {
        unlockedSkillTypeList.Clear();
        OnResetAllSkills?.Invoke(this, System.EventArgs.Empty);
        OnSkillPointsChanged?.Invoke(this, System.EventArgs.Empty);
    }
    public bool TryUnlockSkill(SkillType skillType)
    {
        if (CanUnlock(skillType))
        {
            if (ResourceManager.Instance.GetRemainedResourceAmount(ResourceManager.ResourceType.Star)
                - GetSkillStarAmountForUnlock(skillType) >= 0) //yeterli yıldızın var mı skill'i açmak için
            {
                ResourceManager.Instance.SpendResources(ResourceManager.ResourceType.Star, GetSkillStarAmountForUnlock(skillType));
                OnSkillPointsChanged?.Invoke(this, System.EventArgs.Empty);
                UnlockSkill(skillType);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    public void UnlockSkill(SkillType skillType) //bedelsiz skilli açmak için
    {
        if (!IsSkillUnlocked(skillType))
        {
            if (!unlockedSkillTypeList.Contains(skillType)) unlockedSkillTypeList.Add(skillType);
        }
        OnSkillUnlocked?.Invoke(this, new OnSkillUnlockedEventArgs { skillType = skillType });
    }


    /**** TECH FUNCTIONS ******************************************************/


    public int GetTotalUnlockedSkillPoint()
    {
        int total = 0;
        foreach (SkillType st in unlockedSkillTypeList)
        {
            total += GetSkillStarAmountForUnlock(st);
        }
        ResourceManager.Instance.SetTotalUnlockedSkillPoint(total);
        return total;
    }
    public SkillType GetNextSkill(SkillType skillType) //bi skill'i açtıktan sonraki skill butonunu döndürür
    {
        switch (skillType)
        {
            case SkillType.Health: return SkillType.Energy;
            case SkillType.Energy: return SkillType.Damage;
            case SkillType.Speed: return SkillType.DoubleJump;
            case SkillType.DoubleJump: return SkillType.Dash;
        }
        return SkillType.None;
    }
  

    /*TEST*/

    public void GetUnlockedSkills()
    {
        foreach (SkillType st in unlockedSkillTypeList)
        {
            Debug.Log(st);
        }
    }

}
