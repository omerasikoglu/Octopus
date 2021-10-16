﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("ScriptableObjects/CommanderType"))]
public class CommanderTypeSO : ScriptableObject
{
    public string nameString;
    public Transform prefab;
    public Sprite sprite;
    public int HealthAmountMax;
    public bool hasResourceGeneratorData;
    public ResourceGeneratorData resourceGeneratorData;
}
