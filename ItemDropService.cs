using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemDropService : MonoBehaviourPunCallbacks
{
    public GameObject itemGraphic;
    public GameObject itemCollider;

    public GameObject GraphixHolder;
    public GameObject CollidersHolder;

    public ItemObject[] itemList = new ItemObject[30];

    private GameObject[] grounditems = new GameObject[1000];
    private GameObject[] graphicitems = new GameObject[1000];

    private int destroyitemID = 0;

    public player3 myplayer;
    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < 8; i++)
            {
                //Debug.Log("item dropped");
                Vector3 DropPosition = new Vector3(-2.0f, 0.5f, -1 - i * 1f);
                //photonView.RPC("ItemDrop", RpcTarget.AllBuffered, i, DropPosition);
                //ItemDropSystem(i, DropPosition);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient&&Input.GetKeyDown(KeyCode.Alpha8))
        {
            photonView.RPC("DestroyBuggedItems", RpcTarget.All);
        }
    }

    [PunRPC]
    public void ItemDrop(int i, Vector3 itemPosition)
    {
        GameObject itemgraph;
        
        itemgraph = Instantiate(itemGraphic, itemPosition, Quaternion.Euler(-226.4f, 1f, 1f));
        graphicitems[destroyitemID] = itemgraph;
        itemgraph.transform.parent = GraphixHolder.transform;
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject item;
            item = Instantiate(itemCollider, itemPosition, Quaternion.identity);
            item.transform.parent = CollidersHolder.transform;
            grounditems[destroyitemID] = item;
            item.GetComponent<groundItemServ>().item = itemList[i];
            item.GetComponent<groundItemServ>().itemID = i;
            item.GetComponent<groundItemServ>().groundItemID = destroyitemID;

            item.GetComponent<groundItemServ>().itemdropservice = GetComponent<ItemDropService>();

        }
        destroyitemID++;
    }
    [PunRPC]
    public void DestroyItem(int identity)
    {
        Destroy(graphicitems[identity]);
        if(PhotonNetwork.IsMasterClient)
        {
            Destroy(grounditems[identity]);
        }
    }


    public void GroundItemCall(int PlayeRID, int ItemID, int DestroyItemID)
    {
        photonView.RPC("PlayerEquip", RpcTarget.AllBuffered, PlayeRID, ItemID, DestroyItemID);
    }
    [PunRPC]
    public void PlayerEquip(int PLAYERID, int ITEMID, int DESTROYID)
    {
        myplayer.TakeItem(PLAYERID, ITEMID);
        //if(PhotonNetwork.IsMasterClient)
            DestroyItem(DESTROYID);
    }
    [PunRPC]
    public void ItemDropSystem(int i, Vector3 itemPosition)
    {
        photonView.RPC("ItemDrop", RpcTarget.AllBuffered, i, itemPosition);
    }
    [PunRPC]
    public void DestroyBuggedItems()
    {
        foreach(Transform child in GraphixHolder.transform)
        {
            Destroy(child.gameObject);
        }
        foreach(Transform child in CollidersHolder.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
