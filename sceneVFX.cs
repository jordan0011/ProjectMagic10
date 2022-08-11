using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class sceneVFX : MonoBehaviourPun
{
    [SerializeField] private float timer = 0.4f;
    private float timer0 = 0f;
    [SerializeField] private bool move = false;
    [SerializeField] private bool large = true;
    [SerializeField] private bool isprojectile = false;
    [SerializeField] private float speed = 25f;
    [SerializeField] GameObject me;
    private ProjectileBasicLocal thescript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
            transform.Translate(Vector3.forward * Time.deltaTime * speed);

        timer0 += Time.deltaTime;
        if (timer0 >= timer&&PhotonNetwork.IsMasterClient)
        {
            photonView.RequestOwnership();
            PhotonNetwork.Destroy(me);
        }

        if (PhotonNetwork.IsMasterClient&&isprojectile)
        { 
            if(thescript==null)
            {
                photonView.RequestOwnership();
                PhotonNetwork.Destroy(me);
            }
        }
    }
    public void scriptSetter(GameObject simpleproj)
    {
        thescript = simpleproj.GetComponent<ProjectileBasicLocal>();
    }
}
