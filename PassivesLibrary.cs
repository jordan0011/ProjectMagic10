using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassivesLibrary : MonoBehaviour
{
    public string[] passive;

    public string[] abilityPassive;
    public string[] abilityExtra;

    public GameObject WieldingPoint;

    public GameObject sword_exertion;
    public GameObject black_horse;
    public GameObject undefiant;
    public GameObject PerisDream;
    public GameObject WoodenSword;


    public ItemObject item1;
    public ItemObject item2;
    public ItemObject item3;

    public InventoryObject aBILITY;

    public int[] cooldowns;

    public PlayerStats playerstats;

    int damage;
    int magicPower;

    string value1;
    string value2;

    string value3;
    string value4;
    string value5;
    // Start is called before the first frame update
    void Start()
    {
            Item _item1 = new Item(item1);
            Item _item2 = new Item(item2);
            Item _item3 = new Item(item3);
            aBILITY.AddItem(_item1, 1);
            aBILITY.AddItem(_item2, 1);
            aBILITY.AddItem(_item3, 1);

        passive = new string[10];

        passive[1] = "· Passive: + 20% Critical Strike Chance\n\n";
        passive[2] = "· Passive: + The Black Horse Passive\n\n";
        passive[3] = "· Passive: + 30 Attack Damage\n\n";
        passive[4] = "· Passive: + 20% Attack Damage\n\n";
        passive[5] = "· Passive: + 8% Attack Damage and + 15 On Hit damage\n\n";
        passive[6] = "· Passive: + 15% Magic Power\n\n";
        passive[7] = "· Passive: + 40 Magic Damage\n\n";
        passive[8] = "This Passive is wrong\n"; ;

        abilityPassive = new string[20];
        abilityExtra = new string[20];

        string name = 43.ToString();
        value1 = (30 + damage*0.8f).ToString();
        value2 = (60 + magicPower*0.1f + magicPower*0.7f).ToString();

        cooldowns = new int[3];
        cooldowns[0] = 2;
        cooldowns[1] = 2;
        cooldowns[2] = 6;
    }
    void Update()
    {
        damage = playerstats.Damage;
        magicPower = playerstats.MagicPower;

        value1 = ((int)(30 + damage * 0.8f)).ToString();
        value2 = ((int)(20 + damage * 0.1f + magicPower * 0.7f)).ToString();

        value3 = ((int)(15 + magicPower * 0.8f)).ToString();
        value4 = ((int)(30 + magicPower * 0.6f)).ToString();
        value5 = ((int)(40 + magicPower * 0.6f)).ToString();

        if (playerstats.BasicAttackID==1)
        {
            abilityPassive[0] = $"This ability allows you to execute melee basic attacks, striking for amount <b>equal</b> to your <color=#ff0000ff><b>Attack Damage + On-Hit Damage</b></color>. \n\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t(A Melee weapon is required)";
            cooldowns[0] = 2;
        }
        if(playerstats.BasicAttackID==2)
        {
            abilityPassive[0] = $"You can lauch a short projectile that burns the first thing it touches for <b><color=#00ffffff>{value3}</color> (15 <color=#00ffffff>+80% Magic Power</color>) damage. </b>";
            cooldowns[0] = 2;
        }

        if(playerstats.Ability1ID == 1)
        {
            abilityPassive[1] = $"You thurst your sword forward piercing the opponent for <b><color=#ff0000ff>{value1}</color> (30 <color=#ff0000ff>+80% Attack Damage</color>) damage. </b>\n\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t(A Melee weapon is required) ";
            cooldowns[1] =5;
        }
        if(playerstats.Ability1ID==2)
        {
            abilityPassive[1] = $"Unleash an Unstable flame that causes <b><color=#00ffffff>{value4}</color> (30 <color=#00ffffff>+60% Magic Power</color>) damage. </b> ";
            cooldowns[1] = 4;
        }

        if(playerstats.Ability2ID==1)
        {
            abilityPassive[2] = $"A blue energy wave is going out that strikes enemies for <b><color=#ff0000ff>{value2}</color> (20 <color=#ff0000ff>+10% Attack Damage</color> <color=#00ffffff>+70% Magic Power</color>) damage. </b>";
            cooldowns[2] = 7;
        }
        if(playerstats.Ability2ID==2)
        {
            abilityPassive[2] = $"Blazing the earth with scorching fire for <b><color=#00ffffff>{value5}</color> (40 <color=#00ffffff>+60% Magic Power</color>) damage. </b>";
            cooldowns[2] = 8;
        }
    }
}
