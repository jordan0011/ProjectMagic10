using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthAdditive : MonoBehaviour
{
    public Text healthtext;
    public Enemy3 enemy;
    int health = 0;
    int maxhealth = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        health = enemy.health;
        maxhealth = enemy.maxhealth;

        healthtext.text = health.ToString() + "/" + maxhealth.ToString();
    }
}
