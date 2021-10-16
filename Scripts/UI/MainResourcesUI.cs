using System.Collections;                 // TODO: RESOURCE MANAGERDAN AYARLANIR YAP!
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
                                             //Toplam yıldız ve altın sayın
public class MainResourcesUI : MonoBehaviour
{
    private Dictionary<ResourceTypeSO, Transform> mainResourcesTransformDictionary;
    private ResourceTypeListSO mainResourcesList;

    private void Awake()
    {
        //Datalarımız
        mainResourcesList = Resources.Load<ResourceTypeListSO>("MainResourcesListSO");
        mainResourcesTransformDictionary = new Dictionary<ResourceTypeSO, Transform>();

        //UI - Prototype Mimarisi
        Transform resourceTemplate = transform.Find("ResourceTemplate");
        resourceTemplate.gameObject.SetActive(false);

        int index = 0;
        foreach (ResourceTypeSO resource in mainResourcesList.list) //kaynak miktarlarını 0'lama
        {
            Transform resourceTranform = Instantiate(resourceTemplate, transform);
            resourceTranform.gameObject.SetActive(true);

            resourceTranform.GetComponent<Image>().color = resource.GetColor();
            resourceTranform.Find("image").GetComponent<Image>().sprite = resource.sprite;
            resourceTranform.Find("text").GetComponent<TextMeshProUGUI>().text ="";/*ResourceManager.Instance.GetMaxResourceAmount(resource.resourceType).ToString()*/
                                                                                            //Gereksiz LUL Hatası - artık vermiyor nema problemo
            mainResourcesTransformDictionary[resource] = resourceTranform;

            MouseEnterExitEvents mouseEnterExitEvents = resourceTranform.GetComponent<MouseEnterExitEvents>();
            mouseEnterExitEvents.OnMouseEnter += (object sender, System.EventArgs e) => { TooltipUI.Instance.Show(resource.tooltipString); };
            mouseEnterExitEvents.OnMouseExit += (object sender, System.EventArgs e) => { TooltipUI.Instance.Hide(); };

            index++;
        }
    }
    private void Start()
    {
        ResourceManager.Instance.OnResourceAmountChanged += ResourceManager_OnResourceAmountChanged;
        UpdateMainResources();
    }

    private void ResourceManager_OnResourceAmountChanged(object sender, System.EventArgs e)
    {
        UpdateMainResources();
    }

    private void UpdateMainResources()
    {
        foreach (ResourceTypeSO resourceType in mainResourcesList.list)
        {
            if (mainResourcesTransformDictionary[resourceType] != null) { 
            Transform resourceTransform = mainResourcesTransformDictionary[resourceType];

            int resourceAmount = ResourceManager.Instance.GetMaxResourceAmount(resourceType.resourceType);
            resourceTransform.Find("text").GetComponent<TextMeshProUGUI>().SetText(resourceAmount.ToString());
            }
        }

    }
}
