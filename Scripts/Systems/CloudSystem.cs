using System.Collections;     //TODO UZUN S�REL�:Snow particle ile windDir'�n ayn� y�ne gitmesini sa�la
using System.Collections.Generic;
using UnityEngine;

public class CloudSystem : MonoBehaviour
{
    [SerializeField] public Vector3 windDirection = Vector2.left;
    [SerializeField] private float windSpeed = 1;
    [SerializeField] private float minSpeed = 0.5f;
    [SerializeField] private float resetRadius = 100;

    private Transform[] clouds;
    private float[] speeds;


    void Start()
    {
        clouds = new Transform[transform.childCount];
        speeds = new float[transform.childCount];

        for (var i = 0; i < transform.childCount; i++) //t�m child'lar�n Transformu ve h�z�
        {
            clouds[i] = transform.GetChild(i);
            speeds[i] = Random.value;
        }
    }

    void Update()
    {
        var r2 = resetRadius * resetRadius;

        for (var i = 0; i < speeds.Length; i++)
        {
            var cloud = clouds[i];
            var speed = Mathf.Lerp(minSpeed, windSpeed, speeds[i]);
            cloud.position += windDirection * speed; //bulutu hareket ettirme

            if (cloud.localPosition.sqrMagnitude >= r2)
            {
                cloud.localPosition = new Vector3(-cloud.localPosition.x, cloud.localPosition.y, cloud.localPosition.z);
                //bulut alandan ��k�nca en di�er taraftan geri ��kar
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, resetRadius);
    }
}
