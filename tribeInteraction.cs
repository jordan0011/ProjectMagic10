using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tribeInteraction : InteractionPlace
{
    public override void ControlFunction()
    {
        playerCanvas.classicsPanelOpen();
        //playerCanvas.ClassicsDiscovered();
        playerCanvas.TribesPanelClose();

        Debug.Log("it works");
    }
    public override void ControlFunction2()
    {
        playerCanvas.classicsPanelclose();
        Debug.Log("Too Far...");
    }
}
