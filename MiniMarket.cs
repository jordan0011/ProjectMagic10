using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMarket : MonoBehaviour
{
    public PlayerStats playerstats;

    public Text PowerEssence;
    private int powerEssence;

    public Button[] buyicon;

    public ItemObject[] items;

    public InventoryObject inventory;
    // Start is called before the first frame update
    void Start()
    {
        buyicon = new Button[6];
        //items = new ItemObject[6];
    }

    // Update is called once per frame
    void Update()
    {
        powerEssence = playerstats.PowerEssence;

        PowerEssence.text = powerEssence.ToString();

        if(Input.GetKeyDown(KeyCode.I))
        {
            playerstats.AddEssence(200);
        }
        
    }
    public void Buy1stItem()
    {
        if(powerEssence>=100)
        {
            Item _item = new Item(items[0]);
            inventory.AddItem(_item, 1);
            //powerEssence -= 100;
            playerstats.AddEssence(-100);
        }
    }
    public void Buy2ndItem()
    {
        if(powerEssence>=800)
        {
            Item _item = new Item(items[1]);
            inventory.AddItem(_item, 1);
            playerstats.AddEssence(-800);
        }
    }
    public void Buy3rdItem()
    {
        if (powerEssence >= 550)
        {
            Item _item = new Item(items[2]);
            inventory.AddItem(_item, 1);
            playerstats.AddEssence(-550);
        }
    }
    public void Buy4thItem()
    {
        if (powerEssence >= 1320)
        {
            Item _item = new Item(items[3]);
            inventory.AddItem(_item, 1);
            playerstats.AddEssence(-1320);
        }
    }
    public void Buy5thItem()
    {
        if (powerEssence >= 1500)
        {
            Item _item = new Item(items[4]);
            inventory.AddItem(_item, 1);
            playerstats.AddEssence(-1500);
        }
    }
    public void Buy6thItem()
    {
        if (powerEssence >= 1700)
        {
            Item _item = new Item(items[5]);
            inventory.AddItem(_item, 1);
            playerstats.AddEssence(-1700);
        }
    }
}
