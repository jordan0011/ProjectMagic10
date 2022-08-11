using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalVFX : MonoBehaviour
{
    public float timer = 0.4f;
    private float timer0 = 0f;
    public GameObject me;

    void Update()
    {
        timer0 += Time.deltaTime;
        if (timer0 >= timer)
        {
            Destroy(me);
        }
    }
}
