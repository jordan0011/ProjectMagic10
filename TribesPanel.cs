using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribesPanel : MonoBehaviour
{
    public PlayerStats playerstats;
    public TheClassics classics;
    public TheClassics fractals;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateXp(int type, int amount)
    {
        if (type == 1)
            classics.AddXp(amount);
        if (type == 2)
            fractals.AddXp(amount);
    }
}
