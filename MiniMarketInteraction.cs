using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMarketInteraction : InteractionPlace
{
    public override void ControlFunction()
    {
        playerCanvas.MiniMarketOpen();
        Debug.Log("Welcome to the minimarket");
    }
    public override void ControlFunction2()
    {
        playerCanvas.MiniMarketClose();
        Debug.Log("Too Far...");
    }
}
