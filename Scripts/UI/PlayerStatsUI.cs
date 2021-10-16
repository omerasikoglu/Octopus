using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatsUI : MonoBehaviour
{
    private Dictionary<PlayerStatSO, Transform> statTransformDictionary;
    private PlayerStatListSO playerStatList;

    private void Awake()
    {
        playerStatList = Resources.Load<PlayerStatListSO>(typeof(PlayerStatListSO).Name);
        statTransformDictionary = new Dictionary<PlayerStatSO, Transform>();

        Transform statTemplate = transform.Find("PlayerStatTemplate");
        statTemplate.gameObject.SetActive(false);
        
        int index = 0;
        foreach (PlayerStatSO ps in playerStatList.list)
        {
            Transform statTransform = Instantiate(statTemplate, transform);
            statTransform.gameObject.SetActive(true);

            statTransform.Find("image").GetComponent<Image>().sprite = ps.sprite;
            statTransform.Find("text").GetComponent<TextMeshProUGUI>().text = ps.amount.ToString();
            statTransformDictionary[ps] = statTransform;

            MouseEnterExitEvents mouse = statTransform.GetComponent<MouseEnterExitEvents>();
            mouse.OnMouseEnter += (object sender, System.EventArgs e) => { TooltipUI.Instance.Show(ps.tooltipString); };
            mouse.OnMouseExit += (object sender, System.EventArgs e) => { TooltipUI.Instance.Hide(); };

            index++;
        }
    }
    private void Start()
    {
        StatManager.Instance.OnStatValuesChanged += StatManager_OnStatValuesChanged;
        UpdateStats();
    }

    private void StatManager_OnStatValuesChanged(object sender, System.EventArgs e)
    {
        UpdateStats();
    }
    private void UpdateStats()
    {
        foreach (PlayerStatSO playerStat in playerStatList.list)
        {
            Transform statTransform = statTransformDictionary[playerStat];

            int statAmount = playerStat.amount;
            statTransform.Find("text").GetComponent<TextMeshProUGUI>().SetText(statAmount.ToString());

        }

    }
}
