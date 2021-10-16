using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentsSystem : MonoBehaviour
{
    [SerializeField] private List<Vent> connectedVents;
    [SerializeField] private VentArrowsUI ventArrowsUI;

    private int currentVentID;

    private PlayerController playerController;

    #region Awake Functions
    private void Awake()
    {
        SetupVents();
    }

    private void SetupVents()
    {
        int index = 0;
        foreach (Vent vent in connectedVents)
        {
            vent.ID = index;
            vent.ventsSystem = this;
            index++;
        }
    }
    #endregion

    #region Trigger Functions
    public void CanEnterVentSystem(PlayerController playerController, int ventID)
    {
        this.playerController = playerController;
        currentVentID = ventID;
        playerController.SetVentSystem(this);

    }
    #endregion

    #region Vent içine girmeden, ventten çıkmaya kadar

    public void PlayerInVent() //giriş animasyonu tamamlanınca arrowları çizdirme
    {
       // ventArrowsUI.VentEntered(this, currentVentID, connectedVents, playerController.GetPosition());
        connectedVents[currentVentID].ActivateAllArrows();

    }
    public void ExitTheVentSystem()
    {
        // ventArrowsUI.ResetArrows();
        connectedVents[currentVentID].DeactivateAllArrows();
    }
    public void MoveToRightVent()
    {
        
        if (currentVentID + 2 > connectedVents.Count) //list 0dan başladığı için +2
        {
            //currentVentID = 0;
        }
        else
        {
            connectedVents[currentVentID].DeactivateAllArrows();
            currentVentID += 1;
            playerController.SetPosition(connectedVents[currentVentID].GetPos());

            connectedVents[currentVentID].ActivateAllArrows();
        }
       
        //ventArrowsUI.ResetArrows();
        //ventArrowsUI.VentEntered(this, currentVentID, connectedVents,playerController.GetPosition());
    }
    public void MoveToLeftVent()
    {
        
        if (currentVentID -1 < 0) //list 0dan başladığı için +2
        {
            
        }
        else
        {
            connectedVents[currentVentID].DeactivateAllArrows();
            currentVentID -= 1;
            playerController.SetPosition(connectedVents[currentVentID].GetPos());

            connectedVents[currentVentID].ActivateAllArrows();
        }
        
    }
    //public void MoveToVent(int ventID)
    //{
    //    //Set the Current Vent ID to the new Vent ID
    //    currentVentID = ventID;

    //    //Move the Player to that Vent
    //    playerController.SetPosition(connectedVents[ventID].GetPos());

    //    //Reset the Arrows for the next Vent
    //    ventArrowsUI.ResetArrows();
    //    ventArrowsUI.VentEntered(this, currentVentID, connectedVents);
    //}
    public void EnterIntoTheVentSystem()
    {
        //Make the Player Enter the Vent
        //playerController.EnterVent(this);
        playerController.SetPosition(connectedVents[currentVentID].GetPos());
    }
    #endregion
    private void OnDrawGizmos()
    {
        for (int i = 0; i < connectedVents.Count; i++)
            for (int j = 0; j < connectedVents.Count; j++)
                if (connectedVents[j].ID != connectedVents[i].ID)
                    Debug.DrawLine(connectedVents[i].GetPos(), connectedVents[j].GetPos(), Color.red);
    }
    //public void TPClosestVent(Vent currentVent, Vector3 direction)
    //{
    //    Vent closer = FindClosestVent(currentVent, direction);
    //    playerController.SetPosition(closer.GetPos());
    //}
    //public Vent FindClosestVent(Vent currentVent, Vector3 direction)
    //{
    //    /*TEST*/
    //    direction = Vector3.right;

    //    float mesafe;
    //    float closest = 0;
    //    Vent targetVent = currentVent;

    //    foreach (Vent vent in connectedVents)
    //    {
    //        if (vent == currentVent)
    //        {
    //            continue;
    //        }
    //        //eğer vent bastığımız yöndeyse döndür
    //        //sign  poz or 0 ise 1 ; negatif ise -1 döndürür
    //        if (Mathf.Sign(vent.GetPos().x - currentVent.GetPos().x) == Mathf.Sign(direction.x))
    //        {
    //            mesafe = HipotenusBul(currentVent.GetPos(), vent.GetPos());
    //            if (closest == 0) closest = mesafe; //ilk değer ata
    //            //daha yakın bulursan en yakını değiştir
    //            if (mesafe < closest)
    //            {
    //                closest = mesafe;
    //                targetVent = vent;
    //            }
    //        }
    //    }
    //    Debug.Log(targetVent);
    //    if (targetVent == null) return null;
    //    else return targetVent;

    //}

    //private float HipotenusBul(Vector3 x, Vector3 y)
    //{
    //    float hipX = Mathf.Abs(x.x - y.x);
    //    float hipY = Mathf.Abs(x.y - y.y);

    //    return Mathf.Round(Mathf.Sqrt((hipX * hipX) + (hipY * hipY)));
    //}

    #region Test
    public void Yazdir()
    {
        foreach (Vent item in connectedVents)
        {
            Debug.Log(item);
        }
    }
    #endregion
}
