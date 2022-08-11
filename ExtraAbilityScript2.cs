using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtraAbilityScript2 : MonoBehaviour
{
    public Button[] Abilities;

    public InventoryObject abilities;
    public ItemObject[] items;

    public Item _item0 = new Item();
    public Item _item1 = new Item();
    public Item _item2 = new Item(); 
    // Start is called before the first frame update
    void Awake()
    {
        _item0 = new Item(items[0]);

        _item1 = new Item(items[1]);

        _item2 = new Item(items[2]);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateSlot1()
    {
        Debug.Log("Ability1 Equiped");
        abilities.GetSlots[0].UpdateSlot(_item0, 1);
    }
    public void UpdateSlot2()
    {
        abilities.GetSlots[1].UpdateSlot(_item1, 1);
    }
    public void UpdateSlot3()
    {
        abilities.GetSlots[2].UpdateSlot(_item2, 1);
    }
    
}
