using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    //[SerializeField] private ResourcesUI resourcesUI;
    [SerializeField] private PlayerController player;
    [SerializeField] private SkillTreeUI skillTreeUI;
    //[SerializeField] private PlayerStatsUI playerStatsUI;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private ShopUI2 shopUI;
    [SerializeField] private SpawnPosition spawnPos;

    private void Awake()
    {
        
        if (player == null) player = FindObjectOfType<PlayerController>();
        if (skillTreeUI == null) skillTreeUI = FindObjectOfType<SkillTreeUI>();
        //shopUI = FindObjectOfType<ShopUI>();
        if (shopUI == null) shopUI = FindObjectOfType<ShopUI2>();
        if (inventoryUI == null) inventoryUI = FindObjectOfType<InventoryUI>();
        if (spawnPos ==null) spawnPos = FindObjectOfType<SpawnPosition>();


    }
    private void Start()
    {
        if (skillTreeUI != null) skillTreeUI.SetPlayerSkills(player.GetPlayerSkills());
        if (inventoryUI != null) inventoryUI.SetPlayer(player);
        if (shopUI != null) shopUI.SetShopCustomer(player);

        
        if (player != null) player.SetPosition(spawnPos.transform.position);

        //shopUI.SetPlayer(player);
        //inventoryUI.SetInventory(player.GetInventory());
        //playerStatsUI.SetPlayerStats(player.GetPlayerStats());

    }
}
