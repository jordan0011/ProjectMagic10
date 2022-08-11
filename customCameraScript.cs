using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class customCameraScript : MonoBehaviour
{
    public GameObject damageTextCanvas;
    public GameObject XpTextCanvas;
    public GameObject HealCanvas;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Damagetext(Vector3 point, int number)
    {
        GameObject clone;
        clone = Instantiate(damageTextCanvas, point, Quaternion.identity);
        clone.GetComponentInChildren<Text>().text = number.ToString();
    }
    [PunRPC]
    public void Xptext(Vector3 point, int number)
    {
        GameObject clone;
        clone = Instantiate(XpTextCanvas, point, Quaternion.identity);
        clone.GetComponentInChildren<Text>().text = "+ " + number.ToString() + " Xp";
    }
    [PunRPC]
    public void Healtext(Vector3 point, int number)
    {
        GameObject clone;
        clone = Instantiate(HealCanvas, point, Quaternion.identity);
        clone.GetComponentInChildren<Text>().text = "+ " + number.ToString();
    }
}
