using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentArrowTemplateUI : MonoBehaviour
{
    private Transform arrowTemplate;

    private List<Transform> arrowTransformList;

    private VentsSystem ventsSystem;

    int index = 0;

    private void Awake()
    {
        arrowTemplate = transform.Find("ArrowTemplate");
        arrowTemplate.gameObject.SetActive(false);

    }
    internal void ResetArrows()
    {
        index = 0;
        arrowTransformList.Clear();
    }
    internal void VentEntered(VentsSystem ventsSystem, int currentVentID, List<Vent> connectedVents)
    {
        this.ventsSystem = ventsSystem;
        //arrowTransformList.Clear();

        foreach (Vent vent in connectedVents)
        {
            if (vent.ID != currentVentID)
                SetArrow(connectedVents[currentVentID].GetPos(), vent.GetPos());
        }

    }
    private void SetArrow(Vector3 ventPos, Vector3 nextVentPos)
    {
        Transform arrowTransform = Instantiate(arrowTemplate, transform);
        arrowTransform.gameObject.SetActive(true);

        float offsetAmount = 50f;
        arrowTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 100f);

        Vector3 direction = nextVentPos - ventPos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        arrowTransform.transform.rotation = rotation;
        
        arrowTransformList.Add(arrowTransform);

        index++;

    }

}
