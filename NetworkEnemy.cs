using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkEnemy : MonoBehaviourPun, IPunObservable
{
    protected Enemy3 enemy3;
    protected Vector3 RemoteEnemyPosition;
    protected Quaternion RemoteEnemyRotation2;
    protected float RemoteEnemyRotation;
    //public int health;
    //public int maxhealth = 100;
   // public int playerid;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
        }
        else
        {
            Vector3 LagDistance = RemoteEnemyPosition - transform.position;

            if (LagDistance.magnitude > 3f)   // otan einai upervolika makria
            {
                transform.position = RemoteEnemyPosition;
            }

            if (LagDistance.magnitude > 0.15f)
            {
                transform.position = Vector3.Lerp(transform.position, RemoteEnemyPosition, 9f * Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, RemoteEnemyRotation2, 18f * Time.deltaTime);

            }
            else
            {
                transform.position = RemoteEnemyPosition;
                transform.rotation = RemoteEnemyRotation2;
            }
        }
    }
    private void Awake()
    {
        enemy3 = GetComponent<Enemy3>();
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }
        }
        else
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                RemoteEnemyPosition = (Vector3)stream.ReceiveNext();
                RemoteEnemyRotation2 = (Quaternion)stream.ReceiveNext();
            }
        }
    }

    float lerp(float a, float b, float f)
    {
        return a + f * (b - a);
    }
}
