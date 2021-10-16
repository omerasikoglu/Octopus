using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    protected PlayerController playerController;
    public abstract void Execute();
}

public class VentIt : Command
{
    public VentIt(PlayerController playerController)
    {
        this.playerController = playerController;
    }
    public override void Execute()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("W'ya basıldı");
        }
    }
   
}
public class Jump : Command
{
    public override void Execute()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            //TODO: Sometin
        }
    }
}
//TODO: Hepsini ekle