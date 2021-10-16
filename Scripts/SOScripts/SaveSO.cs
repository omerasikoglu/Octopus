using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Save")]

public class SaveSO : ScriptableObject
{
    public List<PlayerSkills.SkillType> unlockedSkillList;
    public List<Item> gottenItemList;
    public List<Item> fullItemList;

    public List<Item.ItemType> fullItemTypeList;
}
