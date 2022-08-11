using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkPlayer3 : MonoBehaviourPun, IPunObservable
{
    protected player3 player3;
    protected Vector3 RemotePlayerPosition;
    protected Quaternion RemotePlayerRotation2;
    protected float RemotePlayerRotation;
    public int health;
    public int maxhealth= 100;
    public int playerid;

    public int deathtimer = 5;
    public float timer = 0;
    public bool deathswitch = true;
    ItemDropService Itemdrop;

    bool deathmotion = false;

    [SerializeField] private LayerMask camera1;
    RaycastHit hit;

    void Start()
    {
        health = maxhealth;

        deathtimer = 5;

        deathmotion = false;

        Itemdrop = GameObject.Find("ServerSide").GetComponentInChildren<ItemDropService>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if(health<=0&&deathswitch == false)
            {
                int x = 0;
                //health = maxhealth;
                //transform.position = new Vector3(-5, 5, -15);

                //photonView.RPC("Heal", RpcTarget.AllBuffered);

                //edw xaneis ta items
                //Die();
                timer = deathtimer;

                for(int i =0; i < player3.inventory.Container.Slots.Length; i++)
                {
                    if( player3.inventory.GetSlots[i].item.Id >= 45 && player3.inventory.GetSlots[i].item.Id <=52)
                    {
                        x++;
                        player3.inventory.GetSlots[i].RemoveItem();
                    }
                }
                if(x>=3)
                    Itemdrop.photonView.RPC("ItemDropSystem", RpcTarget.MasterClient, 18, transform.position + new Vector3(1f, 0.5f, 1f));

            }

            if(timer >0)
            {
                timer -= Time.deltaTime;

                deathswitch = true;


                player3.IsInputEnabled = false;
                if (player3.animator.GetBool("Die") == false)
                    player3.animator.SetBool("Die", true);
                if(health >0)
                    photonView.RPC("Damage", RpcTarget.AllBuffered, health);

                if(timer <0.5f)
                    transform.position = new Vector3(-9.4f, 1f, -25f);

                deathmotion = true;
            }
            else
            {
                deathswitch = false;

                if(deathmotion == true)
                    DeadtoAlive();

            }
            if(Input.GetKeyDown(KeyCode.Alpha4))
            {
                photonView.RPC("Heal", RpcTarget.AllBuffered);
                timer = 0;
            }
            player3.deathtimer = timer;
        }
        else
        {
            Vector3 LagDistance = RemotePlayerPosition - transform.position;

            

            if(LagDistance.magnitude > 3f)   // otan einai upervolika makria
            {
                transform.position = RemotePlayerPosition;
            }
        
            if(LagDistance.magnitude>0.15f)
            {
                transform.position = Vector3.Lerp(transform.position, RemotePlayerPosition, 9 * Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, RemotePlayerRotation2, 18 * Time.deltaTime);

            }
            else
            {
                transform.position = RemotePlayerPosition;
                transform.rotation = RemotePlayerRotation2;
            }
            //transform.position = Vector3.MoveTowards(transform.position, RemotePlayerPosition, 7f*Time.deltaTime);
            //player3.jump = RemotePlayerPosition.y - transform.position.y > 0.2f;  //adjusting the height
            //if (RemotePlayerPosition.y - transform.position.y > 0.3f)
            //{
            //    transform.position += new Vector3(0, -RemotePlayerPosition.y, 0);
            //    transform.position += new Vector3(0, lerp(transform.position.y, RemotePlayerPosition.y, 7 * Time.deltaTime), 0);
            //}
            //else
            //{
            //    transform.position += new Vector3(0, -RemotePlayerPosition.y, 0);
            //    transform.position += new Vector3(0, RemotePlayerPosition.y, 0);
            //}
        }
    }

    private void Awake()
    {
        player3 = GetComponent<player3>();

        if (!photonView.IsMine && GetComponent<CharacterController>() != null)
        {
            //Destroy(GetComponent<CharacterController>());
            //GetComponent<CapsuleCollider>().isTrigger = true; // questionable
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            if(photonView.IsMine)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);

                stream.SendNext(player3.playerID);
            }
           /* if (photonView.IsMine)
            {
                stream.SendNext(health);
                stream.SendNext(maxhealth);
            }*/
        }
        else
        {
            if (!photonView.IsMine)
            {
                RemotePlayerPosition = (Vector3)stream.ReceiveNext();
                RemotePlayerRotation2 = (Quaternion)stream.ReceiveNext();

                playerid = (int)stream.ReceiveNext();
            }
            /*if(!photonView.IsMine)
            {
                health =(int)stream.ReceiveNext();
                maxhealth = (int)stream.ReceiveNext();
            }*/
        }
    }

    float lerp(float a, float b, float f)
    {
        return a + f * (b - a);
    }
    [PunRPC]
    void Damage(int _damage)
    {
        if (health - _damage >= 0)
        {
            if (_damage < 0)
            {
                if (health + (-_damage) >= maxhealth)
                    health = maxhealth;
                else
                    health -= _damage;

                GetComponent<PhotonView>().RPC("ShowHeal", RpcTarget.AllBuffered, -_damage);  // new XP Show method
            }
            else
            {
                health -= _damage;
            }
        }
        else
            health = 0;

        //Physics.Raycast(transform.position,   Camera.main.transform.position-transform.position, 100f, camera1);
        if (Physics.Raycast(transform.position, Camera.main.transform.position - transform.position, out hit, 300, camera1))
        {
            Camera.main.GetComponent<customCameraScript>().Damagetext(hit.point, _damage);
        }
    }
    [PunRPC]
    void Heal()
    {
        health = maxhealth;
    }
    void Die()
    {
        transform.position = new Vector3(-5, 5, -15);
        photonView.RPC("Heal", RpcTarget.AllBuffered);
    }
    [PunRPC]
    void ShowHeal(int heal)
    {
        Camera.main.GetComponent<customCameraScript>().Healtext(transform.position, heal);
    }
    public void DeadtoAlive()
    {
        if (player3.IsInputEnabled == false && deathmotion == true)
        {
            if (deathmotion == true && player3)
            {
                player3.IsInputEnabled = true;
                deathmotion = false;
            }
            player3.animator.SetBool("Die", false);

            photonView.RPC("Heal", RpcTarget.AllBuffered);

            transform.position = new Vector3(-9.4f, 1f, -25f);
        }
    }
}
