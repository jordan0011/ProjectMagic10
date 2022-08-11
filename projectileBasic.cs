using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class projectileBasic : MonoBehaviourPun
{
    private Vector3 movement1;
    [SerializeField] private bool BlueHit = false;
    [SerializeField] private bool move = false;

    [SerializeField] private float speed = 25f;

    [SerializeField] private float timer = 0.4f;
    private float timer0 = 0f;
    [SerializeField] GameObject me;
    [SerializeField] private bool IsGrowing = false;
    [SerializeField] private bool isLarge = false;
    [SerializeField] private bool DestroyOnLanding = false;
    [SerializeField] private bool Fireball01hit = false;
    [SerializeField] private bool isHitBox = false;
    private ServerSide server;
    public int myPlayerID = -99;
    public int playerHitID;
    // Start is called before the first frame update
    void OnEnable()
    {
        server = GameObject.Find("ServerSide").GetComponent<ServerSide>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(photonView.IsMine)
        { 
            if(other.gameObject.layer == 8)  // Ground
            {
                if (BlueHit)
                {
                    //server.BlueHitVFX(transform);
                }
                else
                {
                    server.FireBall01HitVFX(transform);
                }
                if(!isLarge&&!isHitBox)
                {
                    photonView.RequestOwnership();
                    PhotonNetwork.Destroy(me);
                }
            }
            
            if(other.gameObject.layer == 9)  // Player Entities
            {
                playerHitID = other.gameObject.GetComponent<player3>().playerID;
                if(myPlayerID!=playerHitID)
                {
                    if (BlueHit)
                    {
                        if (isLarge)
                            server.BLueHitVFXLarge(transform, other.transform.position);
                        else
                            server.BlueHitVFX(transform);
                    }
                    else
                    {

                        server.FireBall01HitVFX(transform);  
                    }
                    if(!isLarge&&PhotonNetwork.IsMasterClient&&!isHitBox)
                    {
                        photonView.RequestOwnership();
                        PhotonNetwork.Destroy(me);
                    }
                    if(PhotonNetwork.IsMasterClient)
                        other.GetComponent<NetworkPlayer3>().photonView.RPC("Damage", RpcTarget.AllBuffered);
                }
            }

            if (other.gameObject.layer ==10)  // Environment Enemies
            {
                if (BlueHit)
                {
                    if (isLarge)
                        server.BLueHitVFXLarge(transform, other.transform.position);
                    else
                        server.BlueHitVFX(transform);
                }
                else
                    server.FireBall01HitVFX(transform);
                if(!isLarge&&PhotonNetwork.IsMasterClient&&!isHitBox)
                {
                    photonView.RequestOwnership();
                    PhotonNetwork.Destroy(me);
                }
                other.GetComponentInParent<Enemy3>().photonView.RPC("DamageToEnemy", RpcTarget.AllBuffered);
            }
        }
    }
    public void Update()
    {
        if (move)
            transform.Translate(Vector3.forward * Time.deltaTime * speed);

        timer0 += Time.deltaTime;
        if (timer0 >= timer)
        {
            
                photonView.RequestOwnership();
                if(photonView.AmOwner)
                    PhotonNetwork.Destroy(me);
        }

        if (IsGrowing)
        {
            transform.localScale += new Vector3(Time.deltaTime * 15, 0, Time.deltaTime * 10);
        }
    }
    public void movement(Vector3 direction)
    {
        movement1 = direction;
        move = true;
    }
    [PunRPC]
    public void PlayerIDSet(int playerID)
    {
        myPlayerID = playerID;
    }
}
