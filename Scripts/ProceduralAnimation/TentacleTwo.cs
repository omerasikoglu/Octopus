using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleTwo : MonoBehaviour
{
    public int length;
    public LineRenderer lineRenderer;
    public Vector3[] segmentPoses; // tentacle node'ları
    private Vector3[] segmentV;

    public Transform targetDirection;
    public float targetDistance;
    public float smoothSpeed;

    public float wiggleSpeed;
    public float wiggleMagnitude;
    public Transform wiggleDirection;

    //public Transform tailEnd;
    public Transform[] bodyParts;

    private void Start()
    {
        lineRenderer.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentV = new Vector3[length];

        ResetPosition();
    }
    private void Update()
    {
        wiggleDirection.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude);

        segmentPoses[0] = targetDirection.position;
        for (int i = 1; i < segmentPoses.Length; i++)
        {
            Vector3 targetPosition = segmentPoses[i - 1] + (segmentPoses[i] - segmentPoses[i - 1]).normalized * targetDistance;
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], targetPosition, ref segmentV[i], smoothSpeed);
            bodyParts[i - 1].transform.position = segmentPoses[i];
        }                        
        lineRenderer.SetPositions(segmentPoses);

        //tailEnd.position = segmentPoses[segmentPoses.Length - 1];
    }
    private void ResetPosition() //başlangıçta tüm böcek tek bir noktadan genişlemez
    {
        for (int i = 1; i < length; i++)
        {
            segmentPoses[i] = segmentPoses[i - 1] + targetDirection.right * targetDistance;
        }
        lineRenderer.SetPositions(segmentPoses);
    }
}
