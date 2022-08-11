using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ExpCollisionDetection : MonoBehaviour
{
    private ServerSide server;
    private Enemy3 myEnemy;
    private int playerHitID;
    private int[] playerArray = new int[50];
    private int arraypointer = 0;
    private void OnEnable()
    {
        server = GameObject.Find("ServerSide").GetComponent<ServerSide>();
        myEnemy = GetComponentInParent<Enemy3>();
        arraypointer = 0;
        playerArray = null;
        playerArray = new int[50];
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            bool flag = false;
            if (other.gameObject.layer == 9)  // Player Entities
            {
                playerHitID = other.gameObject.GetComponent<player3>().playerID;
                for(int i =0; i<49; i++)
                {
                    if(playerArray[i]==playerHitID)
                    {
                        flag = true;
                    }
                }
                if(flag == false)
                {
                    server.FireBall01HitVFX(transform);
                    other.GetComponent<NetworkPlayer3>().photonView.RPC("Damage", RpcTarget.AllBuffered, 90);
                    playerArray[arraypointer] = playerHitID;
                }
                arraypointer++; 
            }
        }
    }
}
