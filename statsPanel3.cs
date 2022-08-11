using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class statsPanel3 : MonoBehaviour
{
    public Text damage;
    public Text onhit;
    public Text critical;
    public Text health;
    public Text armor;
    public Text heavyres;
    public Text magicpower;
    public Text cooldown;
    public Text madness;
    public Text lifesteal;

    public Text PowerEssence;

    public PlayerStats playerstats;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        damage.text = playerstats.Damage.ToString();
        onhit.text = playerstats.OnHit.ToString();
        critical.text = playerstats.Critical.ToString();
        health.text = playerstats.Health.ToString();
        armor.text = playerstats.Armor.ToString();
        heavyres.text = playerstats.HeavyResistance.ToString();
        magicpower.text = playerstats.MagicPower.ToString();
        cooldown.text = playerstats.Cooldown.ToString();
        madness.text = playerstats.Madness.ToString();
        lifesteal.text = playerstats.Lifesteal.ToString();

        PowerEssence.text = playerstats.PowerEssence.ToString();
    }
}
