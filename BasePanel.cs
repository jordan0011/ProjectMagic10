using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasePanel : MonoBehaviour
{
    public int powerEssence = 0;
    public int maxpowerEssence = 100;

    public InventoryObject stash;

    public Text essence;

    public Image fillbar;
    public Image fillbar2;

    bool bountclaimed = false;

    public GameObject progressbar;

    public InventoryObject inventory;

    public Item _item0 = new Item();
    public ItemObject item;

    public GameObject gift;

    public GameObject giftbarrier;

    // Start is called before the first frame update
    void Start()
    {
        stash.Clear();

        _item0 = new Item(item);
    }

    // Update is called once per frame
    void Update()
    {
        powerEssence = 0;
        for(int i=0; i < stash.Container.Slots.Length; i++)
        {
            if(stash.GetSlots[i].item.Id>=45 && stash.GetSlots[i].item.Id<=54)
            {
                int x = 0, ID;
                ID = stash.GetSlots[i].item.Id;
                if (ID == 45)
                    x = 10;
                if (ID == 46)
                    x = 20;
                if (ID == 47)
                    x = 30;
                if (ID == 48)
                    x = 50;
                if (ID == 49)
                    x = 40;
                if (ID == 50)
                    x = 80;
                if (ID == 51)
                    x = 120;
                if (ID == 52)
                    x = 100;
                if (ID == 53)
                    x = 160;
                if (ID == 54)
                    x = 200;

                powerEssence += x;
            }
        }

        fillbar.fillAmount = (float) powerEssence / maxpowerEssence;
        fillbar2.fillAmount = (float)powerEssence / maxpowerEssence;


        if(bountclaimed)
        {
            if (progressbar.activeSelf)
            {
                progressbar.SetActive(false);
                essence.text = powerEssence.ToString() + "/ - ";
                gift.SetActive(true);
            }

        }
        else
        {
            if (powerEssence >= maxpowerEssence)
                bountclaimed = true;

            if(!progressbar.activeSelf)
            {
                progressbar.SetActive(true);
            }

            essence.text = powerEssence.ToString() + "/" + maxpowerEssence.ToString();
        }
    }
    public void TakeGift()
    {
        inventory.AddItem(_item0, 1);
        giftbarrier.SetActive(true);
    }
}
