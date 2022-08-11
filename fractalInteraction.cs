using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fractalInteraction : InteractionPlace
{
    public override void ControlFunction()
    {
        playerCanvas.FractalsPanelOpen();
        //playerCanvas.FractalsDiscovered();
        playerCanvas.TribesPanelClose();

        Debug.Log("it works");
    }
    public override void ControlFunction2()
    {
        playerCanvas.FractalsPanelClose();
        Debug.Log("Too Far...");
    }
}
