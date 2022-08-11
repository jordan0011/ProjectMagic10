using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class groundItemServ : MonoBehaviour
{
    public NetworkPlayer3 player;

    public ItemObject item;

    public int itemID;

    public int groundItemID;
    public ItemDropService itemdropservice;

    public GameObject parent1;
    public GameObject parent2;

    public int playerID = 0;

    public LayerMask players;
    //public int theplayerid =0;
    // Start is called before the first frame update
    
    // Update is called once per frame
    public void Update()
    {
        // itemdropservice = GameObject.Find("ServerSide").GetComponentInChildren<ItemDropService>();
        //parent1 = transform.parent.gameObject;
        //parent2 = parent1.transform.parent.gameObject;

        //itemdropservice = parent2.GetComponent<ItemDropService>();
       // itemdropservice = GameObject.Find("ItemDropDeliveryService").GetComponent<ItemDropService>();
        //Debug.Log(itemdropservice);
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9&&PhotonNetwork.IsMasterClient)
        {
            Debug.Log(other);
            var playerinfo = other.GetComponent<player3>();
            if(playerinfo)
            {
                playerID = other.GetComponent<player3>().playerID;
                itemdropservice.GroundItemCall(playerID, itemID, groundItemID);
            }
        }
    }
}
