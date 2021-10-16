using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillTreeUI : MonoBehaviour
{
    //[SerializeField] private SkillUnlockPath[] skillUnlockPathArray;

    private PlayerSkills playerSkills;
    private List<SkillButton> skillButtonList;
    private TextMeshProUGUI skillPointText;
    private Button resetBtn;
    private void Awake()
    {
        skillPointText = transform.Find("skillPointText").GetComponent<TextMeshProUGUI>();
        resetBtn = transform.Find("resetBtn").GetComponent<Button>();
        resetBtn.onClick.AddListener(() =>
        {
            playerSkills.ResetSkills();
        });
    }

    public void SetPlayerSkills(PlayerSkills playerSkills)
    {
        this.playerSkills = playerSkills;

        skillButtonList = new List<SkillButton>();
        skillButtonList.Add(new SkillButton(transform.Find("skillContainer").Find("HealthBtn"), playerSkills, PlayerSkills.SkillType.Health));
        skillButtonList.Add(new SkillButton(transform.Find("skillContainer").Find("EnergyBtn"), playerSkills, PlayerSkills.SkillType.Energy));
        skillButtonList.Add(new SkillButton(transform.Find("skillContainer").Find("SpeedBtn"), playerSkills, PlayerSkills.SkillType.Speed));
        skillButtonList.Add(new SkillButton(transform.Find("skillContainer").Find("DashBtn"), playerSkills, PlayerSkills.SkillType.Dash));
        skillButtonList.Add(new SkillButton(transform.Find("skillContainer").Find("DamageBtn"), playerSkills, PlayerSkills.SkillType.Damage));
        skillButtonList.Add(new SkillButton(transform.Find("skillContainer").Find("DoubleJumpBtn"), playerSkills, PlayerSkills.SkillType.DoubleJump));

        playerSkills.OnSkillPointsChanged += PlayerSkills_OnSkillPointsChanged;
        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
        playerSkills.OnResetAllSkills += PlayerSkills_OnResetAllSkills;
        ResourceManager.Instance.OnResourceAmountChanged += Instance_OnResourceAmountChanged;

        UpdateVisuals();
        UpdateStarPoints();
    }

    private void PlayerSkills_OnResetAllSkills(object sender, System.EventArgs e)
    {
        UpdateStarPoints();
        UpdateVisuals();
        SetPlayerSkills(playerSkills);
    }

    private void Instance_OnResourceAmountChanged(object sender, System.EventArgs e)
    {
        UpdateStarPoints();
    }

    private void PlayerSkills_OnSkillUnlocked(object sender, PlayerSkills.OnSkillUnlockedEventArgs e)
    {
        UpdateVisuals();
        UpdateStarPoints();
    }

    private void PlayerSkills_OnSkillPointsChanged(object sender, System.EventArgs e)
    {
        UpdateStarPoints();
    }
    private void UpdateVisuals()
    {
        foreach (SkillButton skillButton in skillButtonList)
        {
            skillButton.UpdateVisual();

        }
    }
    private void UpdateStarPoints()
    {
        skillPointText.SetText(ResourceManager.Instance.GetRemainedResourceAmount(ResourceManager.ResourceType.Star).ToString()
            + "/" + ResourceManager.Instance.GetMaxResourceAmount(ResourceManager.ResourceType.Star).ToString());
    }

    private class SkillButton
    {
        private Transform backgroundLocked;
        private Transform backgroundUnlocked;
        private Transform imageLocked;
        private Transform imageUnlocked;
        private Transform skillBtn;
        private Transform starAmountText;
        private Transform lockImageTransform;
        private Transform starImageTransform;
        private Transform textTransform;

        private PlayerSkills playerSkills;
        private PlayerSkills.SkillType skillType;

        private MouseEnterExitEvents mouseEnterExitEvents;

        public SkillButton(Transform skillBtn, PlayerSkills playerSkills, PlayerSkills.SkillType skillType)
        {
            this.skillBtn = skillBtn;
            this.playerSkills = playerSkills;
            this.skillType = skillType;

            mouseEnterExitEvents = skillBtn.GetComponent<MouseEnterExitEvents>();

            backgroundLocked = this.skillBtn.Find("locked").Find("backgroundLocked");
            backgroundUnlocked = this.skillBtn.Find("unlocked").Find("backgroundUnlocked");
            imageLocked = this.skillBtn.Find("locked").Find("imageLocked");
            imageUnlocked = this.skillBtn.Find("unlocked").Find("imageUnlocked");
            starAmountText = this.skillBtn.Find("unlocked").Find("text");
            lockImageTransform = this.skillBtn.Find("locked").Find("lockImage");
            starImageTransform = this.skillBtn.Find("unlocked").Find("starImage");
            textTransform = this.skillBtn.Find("unlocked").Find("text");

            backgroundLocked.gameObject.SetActive(true);
            imageLocked.gameObject.SetActive(true);
            backgroundUnlocked.gameObject.SetActive(false);
            imageUnlocked.gameObject.SetActive(false);
            starAmountText.gameObject.SetActive(true);
            lockImageTransform.gameObject.SetActive(true);
            starImageTransform.gameObject.SetActive(false);
            textTransform.gameObject.SetActive(false);
            skillBtn.GetComponent<Button>().interactable = true;

            textTransform.GetComponent<TextMeshProUGUI>().SetText(playerSkills.GetSkillStarAmountForUnlock(skillType).ToString());

            mouseEnterExitEvents.OnMouseEnter += (object sender, System.EventArgs e) => { TooltipUI.Instance.Show(playerSkills.GetTooltipText(skillType)); };
            mouseEnterExitEvents.OnMouseExit += (object sender, System.EventArgs e) => { TooltipUI.Instance.Hide(); };
           
            if (playerSkills.IsSkillUnlocked(skillType)) UpdateInteractStuff();

            if (playerSkills.CanUnlock(skillType)) //açabilceğin sıradaysa ne kadar yıldıza açabilceğinin yazması için
            {
                lockImageTransform.gameObject.SetActive(false);
                starImageTransform.gameObject.SetActive(true);
                textTransform.gameObject.SetActive(true);
            }

            skillBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (!playerSkills.IsSkillUnlocked(skillType)) //skill'in önceden açılan skillerden değilse
                {
                    if (!playerSkills.TryUnlockSkill(skillType)) // Skill'i henüz açamıyorsan path'ten dolayı
                    {                                             //hata ver yoksa skill'i aç
                        TooltipUI.Instance.Show("You cannot!", new TooltipUI.TooltipTimer { timer = .4f });
                    }
                    else
                    {
                        UpdateInteractStuff();
                    }
                }

            });

        }

        internal void UpdateVisual()
        {
            if (playerSkills.CanUnlock(skillType)) //açabilceğin sıradaysa ne kadar yıldıza açabilceğinin yazması için
            {
                lockImageTransform.gameObject.SetActive(false);
                starImageTransform.gameObject.SetActive(true);
                textTransform.gameObject.SetActive(true);
            }
        }
        internal void UpdateInteractStuff()
        {
            backgroundLocked.gameObject.SetActive(false);
            imageLocked.gameObject.SetActive(false);
            backgroundUnlocked.gameObject.SetActive(true);
            imageUnlocked.gameObject.SetActive(true);
            starAmountText.gameObject.SetActive(false);
            lockImageTransform.gameObject.SetActive(false);
            starImageTransform.gameObject.SetActive(true);
            textTransform.gameObject.SetActive(true);
            skillBtn.GetComponent<Button>().interactable = false;
        }
    }

    //[System.Serializable]
    //public class SkillUnlockPath
    //{
    //    public PlayerSkills.SkillType skillType;
    //    public Image[] linkImageArray;
    //}

}


