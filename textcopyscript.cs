using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textcopyscript : MonoBehaviour
{
    public Text start;
    public Text end;

    public GameObject text;
    public GameObject shadow;

    private float timer = 0;

    private Vector3 textstartposition;
    private Vector3 shadowstartposition;

    private float x;
    // Start is called before the first frame update
    void Start()
    {
        textstartposition = text.transform.position;
        shadowstartposition = shadow.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        end.text = start.text;
        timer += Time.deltaTime;

        //if (timer >0.18f)
        // {
        x = 0.5f * Mathf.Sin((timer + 0.2f) * 3f) ;
            text.transform.position = new Vector3(0, x, 0) + textstartposition;
            shadow.transform.position = new Vector3(0, x, 0) + shadowstartposition;
        //}
    }
}
