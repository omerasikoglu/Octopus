//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;

//public class MinionManager : MonoBehaviour
//{
//    public static MinionManager Instance { get; private set; }

//    public event EventHandler<OnActiveMinionTypeChangedEventArgs> OnActiveMinionTypeChanged;

//    public class OnActiveMinionTypeChangedEventArgs : EventArgs
//    {
//        public MinionTypeSO activeMinionType;
//    }
//    [SerializeField] private bool isFirstSet;

//    [SerializeField] private bool haveSummoningPortal = true;
//    [SerializeField] private Transform mateSummoningPortalPosition;


//    private Vector3 mateSummoningPosition;
//    private MinionSetSO minionSet;
//    private MinionTypeSO activeMinionType;

//    private void Awake()
//    {
//        Instance = this;

//        if (isFirstSet) minionSet = Resources.Load<MinionSetSO>("MinionSets/MinionSet1");  //hangi minyon setini çekeceğimiz
//        else minionSet = Resources.Load<MinionSetSO>("MinionSets/MinionSet2");

//        if (haveSummoningPortal) mateSummoningPosition = mateSummoningPortalPosition.position;   //minyon summonlama yerini setleme
//        else mateSummoningPosition = UtilsClass.GetMouseWorldPosition();
//    }

//    private void Update()
//    {
//        #region MinionShortcuts
//        if (Input.GetKeyDown(KeyCode.Q) && ResourceManager.Instance.CanAfford(minionSet.list[0].summoningCostAmountArray))
//        {
//            ResourceManager.Instance.SpendResources(minionSet.list[0].summoningCostAmountArray);
//            Minion.Create(minionSet.list[0], true, mateSummoningPosition);
//        }
//        if (Input.GetKeyDown(KeyCode.E) && ResourceManager.Instance.CanAfford(minionSet.list[1].summoningCostAmountArray))
//        {
//            ResourceManager.Instance.SpendResources(minionSet.list[1].summoningCostAmountArray);
//            Minion.Create(minionSet.list[1], true, mateSummoningPosition);
//        }
//        if (Input.GetKeyDown(KeyCode.R) && ResourceManager.Instance.CanAfford(minionSet.list[2].summoningCostAmountArray))
//        {
//            ResourceManager.Instance.SpendResources(minionSet.list[2].summoningCostAmountArray);
//            Minion.Create(minionSet.list[2], true, mateSummoningPosition);
//        }
//        #endregion

//        if (Input.GetMouseButtonDown(0) && activeMinionType != null && !EventSystem.current.IsPointerOverGameObject())
//        {
//            if (CanSummonMinion(activeMinionType, UtilsClass.GetMouseWorldPosition(), out string errorMessage))
//            {
//                if (ResourceManager.Instance.CanAfford(activeMinionType.summoningCostAmountArray))
//                {
//                    ResourceManager.Instance.SpendResources(activeMinionType.summoningCostAmountArray);
//                    //Instantiate(activeMinionType.prefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
//                    Minion.Create(activeMinionType, true, UtilsClass.GetMouseWorldPosition());
//                    SetActiveMinionType(null);
//                }
//                else
//                {
//                    TooltipUI.Instance.Show("You're poor! Need: \n" + activeMinionType.GetSummoningCostsToString(),
//                   new TooltipUI.TooltipTimer { timer = 2f });
//                }
//            }
//            else
//            {
//                TooltipUI.Instance.Show(errorMessage, new TooltipUI.TooltipTimer { timer = 2f });
//            }
//        }
//    }

//    public void SetActiveMinionType(MinionTypeSO minionType)
//    {
//        activeMinionType = minionType;
//        OnActiveMinionTypeChanged?.Invoke(this,
//            new OnActiveMinionTypeChangedEventArgs { activeMinionType = activeMinionType });
//    }
//    public MinionTypeSO GetActiveMinionType()
//    {
//        return activeMinionType;
//    }
//    private bool CanSummonMinion(MinionTypeSO minionType, Vector3 position, out string errorMessage)  //manuel minyon koyma
//    {
//        BoxCollider2D boxCollider2D = minionType.prefab.GetComponent<BoxCollider2D>();
//        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0f);
//        if (collider2DArray.Length != 0)                                                       //collider stacklenmesi hatası
//        {
//            errorMessage = "Dolu babacım";
//            return false;
//        }
//        float maxSummoningRadius = 8f;
//        collider2DArray = Physics2D.OverlapCircleAll(position, maxSummoningRadius);  //ana binadan uzakta summonlayamama

//        foreach (Collider2D collider2D in collider2DArray)
//        {
//            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();
//            if (buildingTypeHolder != null && buildingTypeHolder.isMate == true)
//            {
//                errorMessage = null;
//                return true;
//            }
//        }
//        errorMessage = "far far away..";
//        return false;
//    }
//    public MinionTypeSO GetRandomMinionFromFirstSet()
//    {
//        int minionIndex = UnityEngine.Random.Range(0, 3);
//        MinionTypeSO minion;
//        minion = minionSet.list[minionIndex];
//        return minion;

//    }
//    //public MinionTypeSO GetRandomMinionFromRandomSet()
//    //{
//    //    bool isFirstSet = UnityEngine.Random.Range(0, minionSetsList.list.Count) == 0;
//    //    int minionIndex = UnityEngine.Random.Range(0, 3);

//    //    if (isFirstSet)
//    //    {
//    //        minionSet = minionSetsList.list[0];
//    //    }
//    //    else
//    //    {
//    //        minionSet = minionSetsList.list[1];
//    //    }
//    //    MinionTypeSO minion = minionSet.list[minionIndex];
//    //    return minion;
//    //}

//}
