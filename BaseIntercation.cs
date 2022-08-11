using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseIntercation : InteractionPlace
{
    public int temple = 0;
    public override void ControlFunction()
    {
        Debug.Log("Base Table is Usable");
        playerCanvas.InventoryOpen();
        if (temple == 0)
            playerCanvas.BasePanelOpen();
        else if (temple == 1)
            playerCanvas.BasePanel1Open();
        else if (temple == 2)
            playerCanvas.BasePanel2Open();
        else if (temple == 3)
            playerCanvas.BasePanel3Open();
        else if (temple == 4)
            playerCanvas.BasePanel4Open();
        else if (temple == 5)
            playerCanvas.BasePanel5Open();

    }
    public override void ControlFunction2()
    {
        playerCanvas.InventoryClose();
        if (temple == 0)
            playerCanvas.BasePanelClose();
        else if (temple == 1)
            playerCanvas.BasePanel1Close();
        else if (temple == 2)
            playerCanvas.BasePanel2Close();
        else if (temple == 3)
            playerCanvas.BasePanel3Close();
        else if (temple == 4)
            playerCanvas.BasePanel4Close();
        else if (temple == 5)
            playerCanvas.BasePanel5Close();
    }
}

