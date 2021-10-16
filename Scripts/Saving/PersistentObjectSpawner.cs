using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PersistentObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pfPersistenObject;
    private bool hasSpawned = false;
    private void Awake()
    {
        if (hasSpawned) return;

        SpawnPersistentObjects();
        hasSpawned = true;

    }
    private void SpawnPersistentObjects()
    {
        if (pfPersistenObject !=null)
        {
            GameObject persistentObject = Instantiate(pfPersistenObject);
            DontDestroyOnLoad(persistentObject); 
        }
    }
}
