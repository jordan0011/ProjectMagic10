using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCanvas3 : MonoBehaviour
{
    [SerializeField] private Enemy3 enemy;

    [SerializeField] private Image healthbar;
    private int health;
    private int maxHealth;

    [SerializeField] private GameObject theMark;
    public bool MarkOn = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        health = enemy.health;
        maxHealth = enemy.maxhealth;
        healthbar.fillAmount = (float)health / maxHealth;
        
        if(MarkOn)
        {
            if (!theMark.activeSelf)
                theMark.SetActive(true);
        }
        else
        {
            if (theMark.activeSelf)
                theMark.SetActive(false);
        }
    }

    public void MarkSet()
    {
        MarkOn = true;
    }
    public void MarkDeset()
    {
        MarkOn = false;
    }
}
