using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionTargeting_LeftAndRight : MonoBehaviour
{
    private Transform targetTransform;
    private Vector2 targetPosition;
    private bool isMate;

    private void Start()
    {
        //isMate = GetComponent<Minion>().isMate;

        if (isMate)
        {
            targetTransform = BuildingManager.Instance.GetEnemyHQBuilding().transform;


        }
        else
        {
             targetTransform = BuildingManager.Instance.GetYourHQBuilding().transform;

           
        }
    }





}
