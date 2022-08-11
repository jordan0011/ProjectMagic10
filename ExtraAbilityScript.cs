using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtraAbilityScript : MonoBehaviour
{
    public InventoryObject abilities;
    public ItemObject item;

    Item _item;
    InventorySlot invSlot = new InventorySlot();

    // Start is called before the first frame update
    void Start()
    {
        _item = new Item(item);
        invSlot.item = _item;
        //abilities.GetSlots[1] = invSlot;
        //.GetSlots[2] = invSlot;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
