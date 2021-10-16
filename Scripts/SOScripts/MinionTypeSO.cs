using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MinionType")]
public class MinionTypeSO : ScriptableObject
{

    //[SerializeField] private MinionSetsListSO minionSetsList;

    [Header("ID")]
    public Transform prefab;
    public string nameString;
    public Sprite sprite;

    [Header("Combat")]
    public int healthAmountMax;
    public float movementSpeed;
    public int damageAmount;

    //knockback
    [Header("Knockback")]

    public bool knockbackable = true; //damage yiyince geri tepmesi var mı



}


//public bool isMate;
//public ResourceAmount[] summoningCostAmountArray;
//public DamageTypeSO[] damageTypeArray;

//public string GetSummoningCostsToString()
//{
//    string str = "";
//    foreach (ResourceAmount resourceAmount in summoningCostAmountArray)
//    {
//        str += "<color=#" + resourceAmount.resourceType.colorHex + ">" + resourceAmount.amount + " " + resourceAmount.resourceType.nameString + "</color>\n";
//    }
//    return str;
//}