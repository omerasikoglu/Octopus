using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsClass
{
    private static Camera mainCamera;
    public static Vector3 GetMouseWorldPosition()
    {
        if (mainCamera == null) mainCamera = Camera.main;

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        return mouseWorldPosition;
    }
    public static Vector3 GetRandomDir()    //x ekseninde random dir
    {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
    public static Vector3 GetRandomUpDir()
    {
        return new Vector3(Random.Range(-1f, 1f), 1f).normalized;
    }
    public static float GetAngleFromVector(Vector3 vector)  //okun yönü düşmana göre dönsün diye
    {
        float radians = Mathf.Atan2(vector.y, vector.x);    //diferansiyel.. en sevdiğim mmm..
        float degrees = radians * Mathf.Rad2Deg;
        return degrees;
    }
}