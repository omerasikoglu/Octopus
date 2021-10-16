using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionTargeting_ClosestEnemy : MonoBehaviour
{
    private Transform targetTransform;
    private Transform targetMinionTransform;
    private Transform targetBuildingTransform;

    private float lookForTargetTimer;
    private float lookForTargetTimerMax = .2f;

    private bool isMate;

    private void Start()
    {
        //isMate = GetComponent<Minion>().isMate;

        lookForTargetTimer = Random.Range(0f, lookForTargetTimerMax);

        if (isMate)
        {
            if (BuildingManager.Instance.GetEnemyHQBuilding() != null) targetTransform = BuildingManager.Instance.GetEnemyHQBuilding().transform;

            else Destroy(gameObject);
        }
        else
        {
            if (BuildingManager.Instance.GetYourHQBuilding() != null) targetTransform = BuildingManager.Instance.GetYourHQBuilding().transform;

            else Destroy(gameObject);
        }
    }
    private void Update()
    {
        HandleTargeting();
        GetComponent<IHandleTargeting>().HandleTargeting(targetTransform);
    }

    private void HandleTargeting()
    {

        lookForTargetTimer -= Time.deltaTime;
        if (lookForTargetTimer < 0f)
        {
            lookForTargetTimer += lookForTargetTimerMax;
           
            LookForTargets();

        }
    }
    private void LookForTargets()
    {
        LookForMinions();
        LookForBuildings();

        if (targetMinionTransform && targetBuildingTransform != null)
        {
            if (Vector3.Distance(transform.position, targetMinionTransform.position) <=
                    Vector3.Distance(transform.position, targetBuildingTransform.position))
            {
                targetTransform = targetMinionTransform;
            }
            else targetTransform = targetBuildingTransform;
        }
    }
    private void LookForBuildings()
    {
        float targetMaxRadius = 10f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);
        foreach (Collider2D collider2D in collider2DArray)  //Look for buildings
        {
            Building building = collider2D.GetComponent<Building>();    //etrafında düşman bina varsa 

            if (building != null && building.isFriendlyBuilding != isMate)
            {
                if (targetBuildingTransform == null)    //biz henüz biyere hedef almadıysak
                {
                    targetBuildingTransform = building.transform;
                }
                else
                {   //hedef bina varsa ama daha yakın bina da varsa
                    if (Vector3.Distance(transform.position, building.transform.position) <
                        Vector3.Distance(transform.position, targetBuildingTransform.position))
                    {
                        targetBuildingTransform = building.transform;
                    }
                }
            }
        }
        if (targetBuildingTransform != null) targetTransform = targetBuildingTransform;
    }
    private void LookForMinions()
    {
        float targetMaxRadius = 10f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        foreach (Collider2D collider2D in collider2DArray)  //Look for minions
        {
           // Minion otherMinion = collider2D.GetComponent<Minion>();
            //if (otherMinion != null && otherMinion.isMate != isMate)
            //{
            //    if (targetMinionTransform == null)
            //    {
            //        targetMinionTransform = otherMinion.transform;
            //    }
            //    else
            //    {   //hedef minyon varsa ama daha yakın minyon da varsa
            //        if (Vector3.Distance(transform.position, otherMinion.transform.position) <
            //            Vector3.Distance(transform.position, targetMinionTransform.position))
            //        {
            //            targetMinionTransform = otherMinion.transform;
            //        }
            //    }
            //}
        }
        if (targetMinionTransform != null) targetTransform = targetMinionTransform;

        //else //target minyon yoksa
        //{
        //    //if (targetTransform == null && targetMinionTransform == null)    //hareket halindeyken etrafta bina bulamadıysa
        //    //{
        //    //    //Destroy(gameObject);
        //    //    if (isMate) targetTransform = BuildingManager.Instance.GetEnemyHQBuilding().transform;
        //    //    else targetTransform = BuildingManager.Instance.GetYourHQBuilding().transform;
        //    //}


        //}

    }
}
