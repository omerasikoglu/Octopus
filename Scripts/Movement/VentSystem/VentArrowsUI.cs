using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VentArrowsUI : MonoBehaviour
{
    [SerializeField] List<GameObject> arrows;
    int index = 0;

    //Dependency Inversion
    private VentsSystem ventsSystem;

    private void Awake()
    {
        ResetArrows();
    }
    internal void ResetArrows()
    {
        index = 0;
        foreach (GameObject arrow in arrows)
        {
            arrow.SetActive(false);
        }
    }

    internal void VentEntered(VentsSystem ventsSystem, int currentVentID, List<Vent> connectedVents, Vector2 playerPos)
    {
        this.ventsSystem = ventsSystem;

        foreach (Vent vent in connectedVents)
        {
            if (vent.ID != currentVentID)
                SetArrow(connectedVents[currentVentID].GetPos(), vent.GetPos(),playerPos);
        }
    }

    private void SetArrow(Vector3 ventPos, Vector3 nextVentPos, Vector3 playerPos)
    {
        arrows[index].SetActive(true);
        Vector3 direction = nextVentPos - ventPos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        arrows[index].transform.rotation = rotation;

        arrows[index].GetComponent<RectTransform>().anchoredPosition = playerPos;
        index++;

    }
    // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward); //z ekseni etrafonda açı kadar döner demek
    //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);


    //private void SetArrow(Vector3 ventPosition, Vector3 nextVentPosition)
    //{
    //    arrows[index].SetActive(true);

    //    //ventler arası açıyı belirler
    //    Vector3 arrowRotation = Vector3.zero; 
    //    arrowRotation.z = -AngleDegrees(ventPosition - nextVentPosition);
    //    arrows[index].GetComponent<RectTransform>().localEulerAngles = arrowRotation;

    //    //Move to the Next Arrow in the List
    //    index++;
    //}
    //public float AngleDegrees(Vector3 p_vector2) //ventler arasındaki açıyı hesaplar
    //{
    //    return (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg) + 180;
    //}
}
