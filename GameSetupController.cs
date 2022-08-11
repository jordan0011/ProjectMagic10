using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Cinemachine;
public class GameSetupController : MonoBehaviourPun
{
    private int layernumber = 12;
    public int playerNumber = 1;
    private bool cursor = false;
    public bool CreatedPlayer = false;
    public bool waitflag = false;
    // Start is called before the first frame update
    [SerializeField] private CinemachineFreeLook playerCamera = null;
    [SerializeField] private CinemachineVirtualCamera playerCameraAim = null;
    [SerializeField] private GameObject PlayerSpawnPoint;

    public GameObject itemDestribution;
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            CreatePlayer(1);
    }
    public void CreatePlayer(int number)
    {
        float x = -4;
        //if(number!= 1)
            x = 0.7f;
            Debug.Log("Creating Player " + playerNumber);
            var player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), PlayerSpawnPoint.transform.position + new Vector3(0, x, 0), Quaternion.identity);
            player.GetComponent<player3>().playerID = number;

            itemDestribution.GetComponent<ItemDropService>().myplayer = player.GetComponent<player3>();

            playerCamera.Follow = player.transform;
            playerCamera.LookAt = player.transform;

            playerCameraAim.Follow = player.transform;
            playerCameraAim.LookAt = player.transform;
            player = null;
        
        playerNumber++;
        photonView.RPC("playerSetter", RpcTarget.AllBuffered, playerNumber);

    }
    private void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Cursor.visible = true;
            playerCamera.m_XAxis.m_MaxSpeed = 0;
            playerCamera.m_YAxis.m_MaxSpeed = 0;
        }
        if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            Cursor.visible = false;
            playerCamera.m_XAxis.m_MaxSpeed = 300f;
            playerCamera.m_YAxis.m_MaxSpeed = 2f;
        }*/
        if(!PhotonNetwork.IsMasterClient&&!CreatedPlayer&&playerNumber!=1)
        {
            CreatePlayer(playerNumber);
            CreatedPlayer = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
            cursor = !cursor;

        if(cursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            playerCamera.m_XAxis.m_MaxSpeed = 300f;
            playerCamera.m_YAxis.m_MaxSpeed = 4f;
        }
        else
        {
            Cursor.visible = true;
            playerCamera.m_XAxis.m_MaxSpeed = 0;
            playerCamera.m_YAxis.m_MaxSpeed = 0;
            Cursor.lockState = CursorLockMode.None;
        }
        
    }
    [PunRPC]
    public void playerSetter(int ID)
    {
        playerNumber = ID;
    }
}
