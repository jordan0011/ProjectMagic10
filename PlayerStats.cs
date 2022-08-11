using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerStats : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update

    public int Damage;
    private int basedamage = 22;
    public int damage3;

    public int OnHit;
    private int baseOnhit = 0;
    public int onhit3;

    public int Critical;
    public int critical3;

    public int Health;
    public int basehp = 98;
    private int health3;

    public int Armor;
    public int armor3;

    public int HeavyResistance;
    public int hr3;

    public int MagicPower;
    public int magicpower3;

    public int Madness;
    public int madness3;

    public int Cooldown;
    public int cooldown3;

    public int Lifesteal;
    public int lifesteal3;

    public int model3D = 0;

    public int model3DprevValue = -1;

    public int essense = 0;

    public int level = 1;
    public int currentlevel = 1;
    public int xpnumber = 0;
    public int maxXpnumber = 100;

    public int value1, value2, value3, value4, value5;
    public float cooldown1, cooldown2, cooldown31, cooldown4;

    public InventoryObject inventory;
    public InventoryObject equipment;
    public InventoryObject equippedCrystal;
    public Attribute[] attributes;

    public InventoryObject Abilities;

    public int BasicAttackID;
    public int Ability1ID;
    public int Ability2ID;

    public PassivesLibrary passiveLibrary;
    public GameObject EquippedWeapon;
    GameObject model = null;

    public int[] tribesXP;
    public TribesPanel tribes;

    public int PowerEssence = 0;
    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            TakeInfo();
        }
        else
        {
            // recieve info from a stream or an asker ...
        }
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            for (int i = 0; i < attributes.Length; i++)
            {
                attributes[i].SetParent(this);
            }
            for (int i = 0; i < equipment.GetSlots.Length; i++)
            {
                equipment.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
                equipment.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
            }
            for (int i = 0; i < equippedCrystal.GetSlots.Length; i++)
            {
                equippedCrystal.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
                equippedCrystal.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
            }
            for (int i = 0; i < Abilities.GetSlots.Length; i++)
            {
                Abilities.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
                Abilities.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
            }
        }

        tribesXP = new int[10];
    }
    public void OnBeforeSlotUpdate(InventorySlot _slot)
    {
        if (photonView.IsMine)
        {
            if (_slot.ItemObject == null)
                return;
            switch (_slot.parent.inventory.type)
            {
                case InterfaceType.Inventory:
                    break;
                case InterfaceType.Equipment:
                    print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, "Allowed Items: ", string.Join(", ", _slot.AllowedItems)));

                    for (int i = 0; i < _slot.item.buffs.Length; i++)
                    {
                        for (int j = 0; j < attributes.Length; j++)
                        {
                            if (attributes[j].type == _slot.item.buffs[i].attribute)
                                attributes[j].value.RemoveModifier(_slot.item.buffs[i]);
                        }
                    }
                    break;
                case InterfaceType.Chest:
                    break;
                default:
                    break;
            }
        }
    }
    public void OnAfterSlotUpdate(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Placed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, "Allowed Items: ", string.Join(", ", _slot.AllowedItems)));

                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                            attributes[j].value.AddModifier(_slot.item.buffs[i]);
                    }
                }
                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }
    }
    public void AttributeModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.type, "= ", attribute.value.ModifiedValue));

    }
    public void TakeInfo()
    {
        damage3 = attributes[0].value.ModifiedValue + basedamage;
        onhit3 = attributes[1].value.ModifiedValue + baseOnhit;
        critical3 = attributes[2].value.ModifiedValue;
        health3 = attributes[3].value.ModifiedValue + basehp;
        armor3 = attributes[4].value.ModifiedValue;
        hr3 = attributes[5].value.ModifiedValue;
        magicpower3 = attributes[6].value.ModifiedValue;
        madness3 = attributes[7].value.ModifiedValue;
        cooldown3 = attributes[8].value.ModifiedValue;
        lifesteal3 = attributes[9].value.ModifiedValue;
        model3D = attributes[10].value.ModifiedValue;
        BasicAttackID = attributes[12].value.ModifiedValue;
        Ability1ID = attributes[13].value.ModifiedValue;
        Ability2ID = attributes[14].value.ModifiedValue;

        value1 = (int)(30 + Damage * 0.8f);
        value2 = (int)(20 + Damage * 0.1f + MagicPower * 0.7f);

        value3 = (int)(15 + MagicPower * 0.8f);
        value4 = (int)(30 + MagicPower * 0.6f);
        value5 = (int)(40 + MagicPower * 0.6f);

        cooldown1 = passiveLibrary.cooldowns[1];  // piercing strike
        cooldown2 = passiveLibrary.cooldowns[2];  // blue shlash
        cooldown31 = passiveLibrary.cooldowns[1];   // unstable flame
        cooldown4 = passiveLibrary.cooldowns[2];  //earthblazer
         
        //model3D = 3;
        //model3D = equipment.GetSlots[0].item.buffs[10].value;
        if (Input.GetKeyDown(KeyCode.I))
            Debug.Log(model3D);

        /*if (model3D == 1)
        {
            EquippedWeapon = passiveLibrary.sword_exertion;
        }
        */
        if (model3D == 1 && model3DprevValue != model3D)
        {
            Debug.Log("The Sword Of Exertion is being equipped");
            photonView.RPC("WeaponDisplay", RpcTarget.AllBuffered, model3D);

            model3DprevValue = model3D;

        }
        if(model3D==1)
            Passive1();
        if (model3D == 3 && model3DprevValue != model3D)
        {
            Debug.Log("The Undefiant is being equipped");
            photonView.RPC("WeaponDisplay", RpcTarget.AllBuffered, model3D);

            model3DprevValue = model3D;

        }
        if (model3D == 3)
            Passive3();
        if (model3D == 4 && model3DprevValue != model3D)
        {
            Debug.Log("Peris Dream is being equipped");
            photonView.RPC("WeaponDisplay", RpcTarget.AllBuffered, model3D);

            model3DprevValue = model3D;

        }
        if (model3D == 4)
            Passive4();
        if (model3D == 5 && model3DprevValue != model3D)
        {
            Debug.Log("Wooden Sword is being equipped");
            photonView.RPC("WeaponDisplay", RpcTarget.AllBuffered, model3D);

            model3DprevValue = model3D;
        }
        if (model3D == 5)
            Passive5();
        if ((model3D == 0 || model3D == 6 || model3D == 7) && model3DprevValue != model3D)
        {
            Debug.Log("you hold NO Sword");
            photonView.RPC("WeaponDisplay", RpcTarget.AllBuffered, model3D);

            model3DprevValue = model3D;
        }

        if (model3D == 0)
            Passive0();
        else if (model3D == 6)
            Passive6();
        else if (model3D ==7)
            Passive7();
    }
    [PunRPC]
    void WeaponDisplay(int modelGlobal)
    {
        GameObject WeaponTemp;
        if (modelGlobal == 0)
        {
            if (EquippedWeapon.transform.childCount > 0)
                Destroy(EquippedWeapon.transform.GetChild(0).gameObject);
            model = null;
        }
        else
        {
            if (modelGlobal == 1)
            {
                WeaponTemp = passiveLibrary.sword_exertion;
                model = null;
                model = Instantiate(WeaponTemp, EquippedWeapon.transform);
                model.transform.SetParent(EquippedWeapon.transform, false); // true -> keeps its world pos

            }
            if (modelGlobal == 3)
            {
                WeaponTemp = passiveLibrary.undefiant;
                model = null;
                model = Instantiate(WeaponTemp, EquippedWeapon.transform);
                model.transform.SetParent(EquippedWeapon.transform, false); // true -> keeps its world pos

            }
            if (modelGlobal == 4)
            {
                WeaponTemp = passiveLibrary.PerisDream;
                model = null;
                model = Instantiate(WeaponTemp, EquippedWeapon.transform);
                model.transform.SetParent(EquippedWeapon.transform, false); // true -> keeps its world pos

            }
            if (modelGlobal == 5)
            {
                WeaponTemp = passiveLibrary.WoodenSword;
                model = null;
                model = Instantiate(WeaponTemp, EquippedWeapon.transform);
                model.transform.SetParent(EquippedWeapon.transform, false); // true -> keeps its world pos

            }
            //model = null;
            //model = Instantiate(WeaponTemp, EquippedWeapon.transform);
            //model.transform.SetParent(EquippedWeapon.transform, false); // true -> keeps its world pos
        }
    }
    [PunRPC]
    public void AddXP(int XpAmount)
    {
        int type = BasicAttackID;

        if(type != 0)
        {
            tribesXP[type] += XpAmount;
            tribes.UpdateXp(type, XpAmount);
        }
    }
    [PunRPC]
    public void AddEssence(int amount)
    {
        PowerEssence += amount;
    }
    public void Passive1()
    {
        Damage = damage3;
        OnHit = onhit3;
        Critical = critical3 + 20;
        Health = health3;
        Armor = armor3;
        HeavyResistance = hr3;
        MagicPower = magicpower3;
        Madness = madness3;
        Cooldown = cooldown3;
        Lifesteal = lifesteal3;

        /*Damage = attributes[0].value.ModifiedValue + basedamage;
        OnHit = attributes[1].value.ModifiedValue + baseOnhit;
        Critical = attributes[2].value.ModifiedValue;
        Health = attributes[3].value.ModifiedValue + basehp;
        Armor = attributes[4].value.ModifiedValue;
        HeavyResistance = attributes[5].value.ModifiedValue;
        MagicPower = attributes[6].value.ModifiedValue;
        Madness = attributes[7].value.ModifiedValue;
        Cooldown = attributes[8].value.ModifiedValue;
        Lifesteal = attributes[9].value.ModifiedValue;
        */
    }
    public void Passive3()
    {
        Damage = damage3 + 30;
        OnHit = onhit3;
        Critical = critical3;
        Health = health3;
        Armor = armor3;
        HeavyResistance = hr3;
        MagicPower = magicpower3;
        Madness = madness3;
        Cooldown = cooldown3;
        Lifesteal = lifesteal3;
    }
    public void Passive4()
    { 
        Damage =(int) (damage3 +  0.2f * damage3);
        OnHit = onhit3;
        Critical = critical3;
        Health = health3;
        Armor = armor3;
        HeavyResistance = hr3;
        MagicPower = magicpower3;
        Madness = madness3;
        Cooldown = cooldown3;
        Lifesteal = lifesteal3;
    }
    public void Passive5()
    {
        Damage = (int) (damage3 + 0.08f * damage3);
        OnHit = onhit3 + 15;
        Critical = critical3;
        Health = health3;
        Armor = armor3;
        HeavyResistance = hr3;
        MagicPower = magicpower3;
        Madness = madness3;
        Cooldown = cooldown3;
        Lifesteal = lifesteal3;
    }
    public void Passive6()
    {
        Damage = damage3;
        OnHit = onhit3;
        Critical = critical3;
        Health = health3;
        Armor = armor3;
        HeavyResistance = hr3;
        MagicPower =(int) (magicpower3 + 0.15f*magicpower3);
        Madness = madness3;
        Cooldown = cooldown3;
        Lifesteal = lifesteal3;
    }
    public void Passive7()
    {
        Damage = damage3;
        OnHit = onhit3;
        Critical = critical3;
        Health = health3;
        Armor = armor3;
        HeavyResistance = hr3;
        MagicPower = magicpower3 + 40;
        Madness = madness3;
        Cooldown = cooldown3;
        Lifesteal = lifesteal3;
    }
    public void Passive0()
    {
        Damage = damage3;
        OnHit = onhit3;
        Critical = critical3;
        Health = health3;
        Armor = armor3;
        HeavyResistance = hr3;
        MagicPower = magicpower3;
        Madness = madness3;
        Cooldown = cooldown3;
        Lifesteal = lifesteal3;
    }
}
[System.Serializable]
public class Attribute
{
    [System.NonSerialized]
    public PlayerStats parent;
    public Attributes type;
    public ModifiableInt value;

    public void SetParent(PlayerStats _parent)
    {
        parent = _parent;
        value = new ModifiableInt(AttributeModified); 
    }
    public void AttributeModified()
    {
        parent.AttributeModified(this);
    }
}
