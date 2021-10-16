using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class StagesUI : MonoBehaviour
{
    [SerializeField] private Portal portal;

    [Header("Level BG Images")]
    [SerializeField] private Sprite lockedLevelBG; //gri
    [SerializeField] private Sprite playedLevelBG; //koyu mavi
    [SerializeField] private Sprite unplayedLevelBG; //açýk mavi
    [SerializeField] private Sprite allStarsDoneLevelBG; //turuncu

    //[Header("Icons")]
    //[SerializeField] private Sprite starIcon;
    //[SerializeField] private Sprite bossIcon;
    //[SerializeField] private Sprite lockIcon;

    //[Header("Dynamic Images")]
    //[SerializeField] private Sprite focus; //arkadaki current level ýþýðý


    private Transform stageTemplate;
    private Transform starTemplate;

    //Datalarýmýz
    private LevelListSO levelList;

    private void Awake()
    {
        levelList = Resources.Load<LevelListSO>(typeof(LevelListSO).Name);

        stageTemplate = transform.Find("StageContainer").Find("StageTemplate");
        stageTemplate.gameObject.SetActive(false);

        starTemplate = transform.Find("StageContainer").Find("StageTemplate").transform.Find("stars").Find("StarTemplate");
        starTemplate.gameObject.SetActive(false);

        //if (currentLevel == null) currentLevel = 1;
    }
    private void Start()
    {
        foreach (LevelSO levelData in levelList.list)
        {
            Transform stageTransform = Instantiate(stageTemplate, transform.Find("StageContainer").transform);


            #region Button Customization
            //level number
            stageTransform.Find("text").GetComponent<TextMeshProUGUI>().SetText(levelData.sceneNumber.ToString());

            //Have Enough Star For Play Check / BG Set
            if (HaveEnoughResource(levelData))
            {
                stageTransform.Find("lockImage").gameObject.SetActive(false);

                if (levelData.isPlayed)
                {
                    int achievedStarCount = 0;
                    foreach (bool st in levelData.achieveList)
                    {
                        if (st == true) achievedStarCount += 1;
                    }

                    for (int i = 0; i < achievedStarCount; i++)
                    {
                        Transform starTransform = Instantiate(starTemplate, stageTransform.Find("stars").transform);
                        starTransform.gameObject.SetActive(true);

                    }

                    if (achievedStarCount == 3)
                    {
                        stageTransform.GetComponent<Image>().sprite = allStarsDoneLevelBG;
                    }
                    else
                    {
                        stageTransform.GetComponent<Image>().sprite = playedLevelBG;
                    }
                }
                else
                {
                    stageTransform.GetComponent<Image>().sprite = unplayedLevelBG;
                }
            }
            else
            {
                stageTransform.Find("lockImage").gameObject.SetActive(true);
                stageTransform.GetComponent<Image>().sprite = lockedLevelBG;
            }

            //boss check
            if (levelData.isBossLevel == true)
            {
                stageTransform.Find("bossImage").gameObject.SetActive(true);
                stageTransform.Find("text").gameObject.SetActive(false);
            }
            else
            {
                stageTransform.Find("bossImage").gameObject.SetActive(false);
                stageTransform.Find("text").gameObject.SetActive(true);
            }
            #endregion

            #region Mouse hold over Events
            
            if (!HaveEnoughResource(levelData))
            {
                MouseEnterExitEvents mouseEvents = stageTransform.GetComponent<MouseEnterExitEvents>();
                mouseEvents.OnMouseEnter += (object sender, System.EventArgs e) =>
                {
                    TooltipUI.Instance.Show($"You need <b><mark=#ffff0030><color=#FFFFFF>" +
                        $"{levelData.requiedStarAmount - ResourceManager.Instance.GetTotalAchievedStarAmount()}</mark><b></color> more star");
                };
                mouseEvents.OnMouseExit += (object sender, System.EventArgs e) =>
                {
                    TooltipUI.Instance.Hide();
                };
            } 
            #endregion

            #region Button click events
            stageTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (HaveEnoughResource(levelData))
                {
                    portal.SetTargetLevel(levelData.sceneNumber);

                    foreach (Transform st in transform.Find("StageContainer")) //önce tüm focus Image'leri sýfýrlar
                    {
                        st.transform.Find("focusImage").gameObject.SetActive(false);
                    }
                    stageTransform.Find("focusImage").gameObject.SetActive(true);
                }
                else
                {
                    TooltipUI.Instance.Show("<color=#FFFFFF>You cannot!</color>",
                        new TooltipUI.TooltipTimer { timer=1f });
                }

            }

            );
            #endregion


            stageTransform.gameObject.SetActive(true);
        }


    }

    private bool HaveEnoughResource(LevelSO levelData)
    {
        return levelData.requiedStarAmount <= ResourceManager.Instance.GetTotalAchievedStarAmount() ? true : false;
    }
}
