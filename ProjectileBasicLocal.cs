using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ProjectileBasicLocal : MonoBehaviour
{
    [SerializeField] private bool BlueHit = false;
    [SerializeField] private bool move = false;

    [SerializeField] private float speed = 25f;

    [SerializeField] private float timer = 0.4f;
    private float timer0 = 0f;
    [SerializeField] GameObject me;
    [SerializeField] private bool IsGrowing = false;
    [SerializeField] private bool isLarge = false;
    public bool isON = true;
    private ServerSide server;
    public int myPlayerID = -99;
    public int playerHitID;

    public int damage = -1;
    public bool enemySkillshot = false;
    public int UnitID = 1;
    // Start is called before the first frame update
    void OnEnable()
    {
        server = GameObject.Find("ServerSide").GetComponent<ServerSide>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(enemySkillshot)
        {
            if (other.gameObject.layer == 8)  // Ground
            {
                if (BlueHit)
                {
                    //server.BlueHitVFX(transform);
                }
                else
                {
                    if(UnitID == 10)
                    {
                        //server.EnemyLaserHitVFX(other.transform.position, transform.rotation);
                    }
                    else
                    {
                        server.FireBall01HitVFX(transform);

                    }
                }
                if (!isLarge)
                {
                    isON = false;
                    Destroy(me);
                }
            }
            if (other.gameObject.layer == 9)  // Player Entities
            {
                playerHitID = other.gameObject.GetComponent<player3>().playerID;
                if (myPlayerID != playerHitID)
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
                        if(UnitID == 10)
                        {
                            server.EnemyLaserHitVFX(other.transform.position, transform.rotation);

                        }
                        else
                        {
                            server.FireBall01HitVFX(transform);

                        }
                    }
                    other.GetComponent<NetworkPlayer3>().photonView.RPC("Damage", RpcTarget.AllBuffered, damage);
                    if (!isLarge)
                    {
                        isON = false;
                        Destroy(me);
                    }
                }
            }
        }
        else
        {
            if (other.gameObject.layer == 8)  // Ground
            {
                if (BlueHit)
                {
                        //server.BlueHitVFX(transform);
                }
                else
                {
                    if (UnitID == 10)
                    {
                        //server.EnemyLaserHitVFX(other.transform.position, transform.rotation);
                    }
                    else
                    {
                        server.FireBall01HitVFX(transform);

                    }
                }
                if (!isLarge)
                {
                    isON = false;
                    Destroy(me);
                }
            }

            if (other.gameObject.layer == 9)  // Player Entities
            {
                playerHitID = other.gameObject.GetComponent<player3>().playerID;
                if (myPlayerID != playerHitID)
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
                    other.GetComponent<NetworkPlayer3>().photonView.RPC("Damage", RpcTarget.AllBuffered, damage);
                    if (!isLarge)
                    {
                        isON = false;
                        Destroy(me);
                    }
                }
            }

            if (other.gameObject.layer == 10)  // Environment Enemies
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
                    other.GetComponentInParent<Enemy3>().photonView.RPC("DamageToEnemy", RpcTarget.AllBuffered, damage);
                if (!isLarge)
                {
                    isON = false;
                    Destroy(me);
                }
            }
        }
    }
    void Update()
    {
        if (move)
            transform.Translate(Vector3.forward * Time.deltaTime * speed);

        timer0 += Time.deltaTime;
        if (timer0 >= timer)
        {
            Destroy(me);
        }

        if (IsGrowing)
        {
            transform.localScale += new Vector3(Time.deltaTime * 15, 0, Time.deltaTime * 10);
        }
    }
    public void movement(Vector3 direction)
    {
        //movement1 = direction; if it is independent movement
        move = true;
    }
    [PunRPC]
    public void PlayerIDSet(int playerID)
    {
        myPlayerID = playerID;
    }
    public void DamageSet(int _damage)
    {
        damage = _damage;
    }
    
}
