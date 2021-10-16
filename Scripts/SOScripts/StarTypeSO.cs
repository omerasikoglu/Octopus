using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StarTypeSO", menuName = "LevelDesign/StarType")]

public class StarTypeSO : ScriptableObject
{
    public string nameString;
    public Sprite sprite;
    public string tooltipString;
    public Color bgColor;
}
