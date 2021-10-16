using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    public int length;
    public LineRenderer lineRenderer;
    private Vector3[] segmentPoses; // tentacle node'ları
    private Vector3[] segmentV;

    public Transform targetDirection;
    public float targetDistance;
    public float smoothSpeed;
    public float trailSpeed;

    public float wiggleSpeed;
    public float wiggleMagnitude;
    public Transform wiggleDirection;

    private void Start()
    {
        lineRenderer.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentV = new Vector3[length];
    }
    private void Update()
    {
        wiggleDirection.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude);

        segmentPoses[0] = targetDirection.position;
        for (int i = 1; i < segmentPoses.Length; i++)
        {
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i],
                segmentPoses[i - 1] + targetDirection.right * targetDistance, ref segmentV[i],
                smoothSpeed + i / trailSpeed );
        }                             // tüm tentacle'lar üst üste binmesin diye right ile topladık - görünmesi için
        lineRenderer.SetPositions(segmentPoses);
    }
}
