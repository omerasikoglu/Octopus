using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    [SerializeField] private Building friendlyHQBuilding;
    [SerializeField] private Building enemyHQBuilding;

    private Camera mainCamera;
    private BuildingTypeListSO buildingTypeList;

    private void Awake()
    {
        Instance = this;
        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }
    public Building GetYourHQBuilding()
    {
        return friendlyHQBuilding;

    }
    public Building GetEnemyHQBuilding()
    {
        return enemyHQBuilding;
    }


}
