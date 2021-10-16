using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : MonoBehaviour
{
    [SerializeField] private List<GameObject> arrows;
    public VentsSystem ventsSystem;
    public int ID = 0;

    private void Awake()
    {
        DeactivateAllArrows();
    }
    public Vector3 GetPos()
    {
        return this.transform.position + new Vector3(0, 1.4f); //1.4 player'ın boyu
    }

    public void EnableVent(PlayerController player)
    {
        ventsSystem.CanEnterVentSystem(player, ID); // ventler içinde hareket edebilir
    }
    public int GetVentID()
    {
        return ID;
    }
    public void ActivateAllArrows()
    {
        foreach (GameObject item in arrows)
        {
            item.SetActive(true);
        }
    }
    public void DeactivateAllArrows()
    {
        foreach (GameObject item in arrows)
        {
            item.SetActive(false);
        }
    }
}
