using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarWorldSpawner : MonoBehaviour
{
    //public ResourceTypeSO starType;
    public ResourceTypeSO resourceType;

    private void Awake()
    {
        StarWorld.SpawnStarWorld(transform.position, resourceType);
        Destroy(gameObject);
    }
}
