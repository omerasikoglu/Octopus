using System.Collections;                                    // TODO: hangi leveldeyiz gözükcek şekilde UI ekle
using System.Collections.Generic;                            // TODO: Abstrack class yap current levelları InStage'den kalıtım
using UnityEngine;                                           // alsınlar.
using UnityEngine.UI;                                        // TODO: DICLERI KALDIR BU SCRIPTTEN! SADECE RESOURCE MANAGERLA BAGLANTISINI SAGLAT!
                                                             // 
public class StarsUI : MonoBehaviour                       // 
{                                                            // 
    [SerializeField] private LevelSO currentLevel;
    private List<ResourceTypeSO> starTypesList;  //3 yıldızın türlerini tutan liste
    private List<bool> achieveList;     //stage başarımlarını tutan liste
    private Dictionary<ResourceTypeSO, Transform> starTransformDictionary; //yıldızların yerleri


    private void Awake()
    {
        starTypesList = currentLevel.starTypesList;
        achieveList = currentLevel.achieveList;

        starTransformDictionary = new Dictionary<ResourceTypeSO, Transform>();

        Transform starTemplate = transform.Find("StarTemplate");
        starTemplate.gameObject.SetActive(false);

        int index = 0;
        foreach (ResourceTypeSO starType in starTypesList)
        {
            Transform starTransform = Instantiate(starTemplate, transform);
            starTransform.gameObject.SetActive(true);

            float offsetAmount = -60f;
            starTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);
            // starTransform.Find("background").GetComponent<Image>().color = starType.bgColor; //material'siz sadece color gözükmüyor UI'da

            if (achieveList[index] == true)
            {
                starTransform.Find("image").GetComponent<Image>().sprite = starType.sprite;
            }
            else
            {
                starTransform.Find("image").GetComponent<Image>().sprite = currentLevel.noStar;
            }
            starTransformDictionary[starType] = starTransform;

            MouseEnterExitEvents mouseEnterExitEvents = starTransform.GetComponent<MouseEnterExitEvents>();
            mouseEnterExitEvents.OnMouseEnter += (object sender, System.EventArgs e) => { TooltipUI.Instance.Show(starType.tooltipString); };
            mouseEnterExitEvents.OnMouseExit += (object sender, System.EventArgs e) => { TooltipUI.Instance.Hide(); };

            index++;
        }
    }
    private void Start()
    {
        //StarManager.Instance.OnWhichStarAchieved += Instance_OnStarAchieved;
        ResourceManager.Instance.OnWhichStarAchieved += Instance_OnWhichStarAchieved;
    }

    private void Instance_OnWhichStarAchieved(object sender, ResourceManager.OnWhichStarAchievedEventArgs e)
    {
        UpdateAchievedStarImage(e.resourceType);
    }



    //private void Instance_OnStarAchieved(object sender, StarManager.OnStarAchievedEventArgs e)
    //{
    //    UpdateAchievedStarImage(e.achievedStarIndex);
    //}
    private void UpdateAchievedStarImage(ResourceTypeSO resourceType)
    {
        foreach (ResourceTypeSO starType in starTransformDictionary.Keys)
        {
            Transform starTransform = starTransformDictionary[starType];

            if (starType==resourceType)
            {
                starTransform.Find("image").GetComponent<Image>().sprite = starType.sprite;
            }
        }

    }

    public void ShowUI()
    {
        gameObject.SetActive(true);
    }
    public void HideUI()
    {
        gameObject.SetActive(false);
    }

}
