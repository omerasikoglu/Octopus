using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePositionSortingOrder : MonoBehaviour
{
    [SerializeField] private bool runOnce;
    [SerializeField] private float positionOffsetY;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void LateUpdate()
    {
        float precisionMultiplier = 5f; //same sortta olanlar için belirginlik, kesinlik
        spriteRenderer.sortingOrder = (int)(-(transform.position.y+positionOffsetY) * precisionMultiplier);
        if (runOnce) Destroy(this);
    }
}
