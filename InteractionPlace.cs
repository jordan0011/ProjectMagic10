using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPlace : MonoBehaviour
{
    public playerCanvas3 playerCanvas=null;
    public GameObject interactPanel;

    private void Start()
    {
        if (interactPanel.activeSelf)
            interactPanel.SetActive(false);
    }
    private void Update()
    {
        
        if(interactPanel.activeSelf==true&&Input.GetKeyDown(KeyCode.F)&&playerCanvas!=null)
        {
            ControlFunction();
        }        
                
    }
    public void OnTriggerEnter(Collider other)
    {
        if(interactPanel.activeSelf==false&&other.gameObject.layer == 9)
        {
            interactPanel.SetActive(true);
        }    

        if (other.gameObject.layer == 9&&other.GetComponent<player3>().photonView.IsMine)
        {
            if(playerCanvas == null)
            {
                playerCanvas = other.GetComponentInChildren<playerCanvas3>();
            }
            else
            {
                //ControlFunction();
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (interactPanel.activeSelf == true)
            interactPanel.SetActive(false);

        if(other.gameObject.layer == 9 && other.GetComponent<player3>().photonView.IsMine)
        {
            if(playerCanvas != null)
            {
                //playerCanvas = other.GetComponentInChildren<playerCanvas3>();
                ControlFunction2();
                playerCanvas = null;
            }
            else 
            {
            }
        }
    }
    public virtual void ControlFunction()
    {
        Debug.Log("Enchantment Table is Usable");
        playerCanvas.InventoryOpen();
        playerCanvas.EnchTableOpen();

        interactPanel.SetActive(false);

    }
    public virtual void ControlFunction2()
    {
        Debug.Log("Enchantment Table is too far");
        //playerCanvas.InventoryOpen();
        playerCanvas.EnchTableClose();
    }
}
