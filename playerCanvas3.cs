using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using System;

public class playerCanvas3 : MonoBehaviour
{
    [SerializeField] private player3 myplayer;
    bool masterclient;
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject WCanvas;
    [SerializeField] private GameObject HUD;
    [SerializeField] private Image health;
    [SerializeField] private Image healthHUD;
    [SerializeField] private Text healthtextHUD;

    [SerializeField] private GameObject theMark;
    public bool MarkOn = false;

    [SerializeField] private Image A0cdimage;
    [SerializeField] private GameObject A0cooldownNumber;
    private float A0cooldown0;
    private float A0cooldown = 1.4f;
    [SerializeField] private Image A0Flash;
    private float flashdeltatime0 = 255;
    [SerializeField] private GameObject A0backshade;

    [SerializeField] private Image A1cdimage;
    [SerializeField] private GameObject A1cooldownNumber;
    private float A1cooldown0;
    private float A1cooldown = 2f;
    [SerializeField] private Image A1Flash;
    private float flashdeltatime1 = 255;
    [SerializeField] private GameObject A1backshade;


    private bool isMage = false;

    [SerializeField] private GameObject Inventory;
    private bool InventoryOpened = false;

    [SerializeField] private GameObject EncTable;
    private bool EnchTableOpened = false;

    [SerializeField] private GameObject CrystalInv;
    private bool CrystInvOpened = false;

    [SerializeField] private GameObject ClassicsPanel;
    private bool ClasicsPanelOpened = false;

    [SerializeField] private GameObject BasePanel;
    private bool BasePanelOpened = false;

    [SerializeField] private GameObject BasePanel1;
    private bool BasePanel1Opened = false;

    [SerializeField] private GameObject BasePanel2;
    private bool BasePanel2Opened = false;

    [SerializeField] private GameObject BasePanel3;
    private bool BasePanel3Opened = false;

    [SerializeField] private GameObject BasePanel4;
    private bool BasePanel4Opened = false;

    [SerializeField] private GameObject BasePanel5;
    private bool BasePanel5Opened = false;


    public GameObject AddItemWindow;
    public GameObject AIWBlocker;
    public TMP_InputField inputField;

    public GameObject TribesPanel;
    private bool TribesOpen = false;
    public GameObject ClassicsPanel2;
    public GameObject ClassicsButton;
    public bool classicsPanel2Open = false;
    public bool classicsDiscovered = false;

    public GameObject fractalsPanel;
    public GameObject fractalsPanel2;
    private bool fractalsOpen = true;
    public GameObject FractalsButton;
    public bool fractalsPanelOpen = false;
    public bool fractalsDiscovered = false;

    public GameObject MiniMarket1;
    private bool MiniMarketOpened = false;

    public GameObject woodensword;
    public GameObject woodenswordblocker;

    public GameObject abilityblocker;
    public GameObject ability1;

    public GameObject magicring;
    public GameObject magicringblocker;

    public GameObject ability2blocker;
    public GameObject ability2;

    [SerializeField]
    private InventoryObject inventory;
    [SerializeField]
    private InventoryObject equipment;

    public Item _item0 = new Item();
    public ItemObject item;

    [SerializeField]
    private InventoryObject ability;

    public Item _item2 = new Item();
    public ItemObject item2;


    public ItemObject item1;
    public Item _item1 = new Item();

    public ItemObject item3;
    public Item _item3 = new Item();

    public Text deathtimer;
    public GameObject deathtimertext;

    bool itempanefirsttime = true;

    ArrayList itemCodes;

    public GameObject CameraImage;
    public bool cameratoggle = false;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponentInParent<player3>().photonView.IsMine)
        {
            WCanvas.SetActive(false);
            HUD.SetActive(true);
        }
        else
        {
            WCanvas.SetActive(true);
            HUD.SetActive(false);
        }

        AdItWinClose();

        _item0 = new Item(item);
        _item1 = new Item(item1);
        _item2 = new Item(item2);
        _item3 = new Item(item3);

        itemCodes = new ArrayList();
        itemCodes.Add("JORDAN");
    }

    // Update is called once per frame
    void Update()
    {
            masterclient = myplayer.IsMasterClient;

            if (masterclient)
                text.SetActive(true);
            else
                text.SetActive(false);

            if(!myplayer.photonView.IsMine)
            {
                health.fillAmount = (float)GetComponentInParent<NetworkPlayer3>().health / GetComponentInParent<NetworkPlayer3>().maxhealth;
                healthtextHUD.text = GetComponentInParent<NetworkPlayer3>().health.ToString() + "/" + GetComponentInParent<NetworkPlayer3>().maxhealth.ToString();

                if (MarkOn)
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
        if (myplayer.photonView.IsMine)
        {
            if(myplayer.deathtimer>0)
            {
                if (deathtimertext.activeSelf == false)
                    deathtimertext.SetActive(true);
                if (myplayer.deathtimer > 0.5f)
                    deathtimer.text = ((int)myplayer.deathtimer + 1).ToString();
                else
                    deathtimer.text = ((int)myplayer.deathtimer).ToString();
            }
            else
            {
                if (deathtimertext.activeSelf == true)
                    deathtimertext.SetActive(false);
            }
                

            healthHUD.fillAmount = (float)GetComponentInParent<NetworkPlayer3>().health / GetComponentInParent<NetworkPlayer3>().maxhealth;
            //isMage = GetComponentInParent<player3>().mage;
            isMage = myplayer.fire;

            if (Input.GetKeyDown(KeyCode.LeftControl))
                cameratoggle =  !cameratoggle;

            if(cameratoggle == true)
            {
                CameraImage.SetActive(false);
            }
            else
            {
                CameraImage.SetActive(true);
            }

            if (myplayer.ability1 == 1)
            {
                A0cooldown0 = myplayer.A3cooldown0;   // piercing strike
                A0cooldown = myplayer.A3cooldown;
            }
            else if (myplayer.ability1 ==2)
            {
                A0cooldown0 = myplayer.A0cooldown0;  // unstable flame
                A0cooldown = myplayer.A0cooldown;
            }

            if (myplayer.ability2 == 1)
            {
                A1cooldown0 = myplayer.A1cooldown0;   // blue shlash
                A1cooldown = myplayer.A1cooldown;
            }
            else if (myplayer.ability2 == 2)
            {
                A1cooldown0 = myplayer.A2cooldown0;  // earthblazer
                A1cooldown = myplayer.A2cooldown;
            }


            if (A0cooldown0 > 0f) // IsCooldowned
            {
                A0cooldown0 -= Time.deltaTime;
                if (A0cooldownNumber.gameObject.activeSelf == false)
                {
                    A0cooldownNumber.SetActive(true);
                    A0backshade.SetActive(true);
                }
                if (A0cooldown0 > 1f)
                    A0cooldownNumber.GetComponent<Text>().text = A0cooldown0.ToString("N0");
                else
                    A0cooldownNumber.GetComponent<Text>().text = A0cooldown0.ToString("N1");


            }
            else // IsNOTCooldowned
            {
                if (A0cooldownNumber.gameObject.activeSelf)
                {
                    A0cooldownNumber.SetActive(false);
                    A0backshade.SetActive(false);
                    flashdeltatime0 = 255;
                }
            }
            A0cdimage.fillAmount = (float)A0cooldown0 / A0cooldown;

            if (flashdeltatime0 >= 0)
                flashdeltatime0 -= 500 * Time.deltaTime;
            Color flashColor6 = new Color(255, 255, 255);
            flashColor6.a = (float)flashdeltatime0 / 255;
            A0Flash.color = flashColor6;

            if (A1cooldown0 > 0f) // IsCooldowned
            {
                A1cooldown0 -= Time.deltaTime;
                if (A1cooldownNumber.gameObject.activeSelf == false)
                {
                    A1cooldownNumber.SetActive(true);
                    A1backshade.SetActive(true);
                }
                if (A1cooldown0 > 1f)
                    A1cooldownNumber.GetComponent<Text>().text = A1cooldown0.ToString("N0");
                else
                    A1cooldownNumber.GetComponent<Text>().text = A1cooldown0.ToString("N1");


            }
            else // IsNOTCooldowned
            {
                if (A1cooldownNumber.gameObject.activeSelf)
                {
                    A1cooldownNumber.SetActive(false);
                    A1backshade.SetActive(false);
                    flashdeltatime1 = 255;
                }
            }
            A1cdimage.fillAmount = (float)A1cooldown0 / A1cooldown;

            if (flashdeltatime1 >= 0)
                flashdeltatime1 -= 500 * Time.deltaTime;
            Color flashColor1 = new Color(255, 255, 255);
            flashColor1.a = (float)flashdeltatime1 / 255;
            A1Flash.color = flashColor1;

            if (InventoryOpened)
            {
                if (!Inventory.activeSelf)
                {
                    Inventory.SetActive(true);
                    myplayer.canAttack2 = false;
                    //GetComponentInParent<player3>().canAttack2 = false;
                }
            }
            else
            {
                if (Inventory.activeSelf)
                {
                    Inventory.SetActive(false);
                    //GetComponentInParent<player3>().canAttack2 = true;
                    myplayer.canAttack2 = true;
                }
            }

            if (EnchTableOpened)
            {
                if (!EncTable.activeSelf)
                    EncTable.SetActive(true);
            }
            else
            {
                if (EncTable.activeSelf)
                    EncTable.SetActive(false);
            }

            if (BasePanelOpened)
            {
                if (!BasePanel.activeSelf)
                    BasePanel.SetActive(true);
            }
            else
            {
                if (BasePanel.activeSelf)
                    BasePanel.SetActive(false);
            }
            if (BasePanel1Opened)
            {
                if (!BasePanel1.activeSelf)
                    BasePanel1.SetActive(true);
            }
            else
            {
                if (BasePanel1.activeSelf)
                    BasePanel1.SetActive(false);
            }
            if (BasePanel2Opened)
            {
                if (!BasePanel2.activeSelf)
                    BasePanel2.SetActive(true);
            }
            else
            {
                if (BasePanel2.activeSelf)
                    BasePanel2.SetActive(false);
            }
            if (BasePanel3Opened)
            {
                if (!BasePanel3.activeSelf)
                    BasePanel3.SetActive(true);
            }
            else
            {
                if (BasePanel3.activeSelf)
                    BasePanel3.SetActive(false);
            }
            if (BasePanel4Opened)
            {
                if (!BasePanel4.activeSelf)
                    BasePanel4.SetActive(true);
            }
            else
            {
                if (BasePanel4.activeSelf)
                    BasePanel4.SetActive(false);
            }
            if (BasePanel5Opened)
            {
                if (!BasePanel5.activeSelf)
                    BasePanel5.SetActive(true);
            }
            else
            {
                if (BasePanel5.activeSelf)
                    BasePanel5.SetActive(false);
            }

            if (MiniMarketOpened)
            {
                if (!MiniMarket1.activeSelf)
                    MiniMarket1.SetActive(true);
            }
            else
            {
                if (MiniMarket1.activeSelf)
                    MiniMarket1.SetActive(false);
            }

            if (ClasicsPanelOpened)
            {
                if (!ClassicsPanel.activeSelf)
                    ClassicsPanel.SetActive(true);
            }
            else
            {
                if (ClassicsPanel.activeSelf)
                    ClassicsPanel.SetActive(false);
            }
            if (fractalsPanelOpen)
            {
                if (!fractalsPanel.activeSelf)
                    fractalsPanel.SetActive(true);
            }
            else
            {
                if (fractalsPanel.activeSelf)
                    fractalsPanel.SetActive(false);
            }
            if (classicsDiscovered)
            {
                if (!ClassicsPanel2.activeSelf)
                    ClassicsPanel2.SetActive(true);
            }
            else
            {
                if (ClassicsPanel2.activeSelf)
                    ClassicsPanel2.SetActive(false);
            }
            if (classicsDiscovered)
            {
                if (!ClassicsButton.activeSelf)
                    ClassicsButton.SetActive(true);
            }
            else
            {
                if (ClassicsButton.activeSelf)
                    ClassicsButton.SetActive(false);
            }
            if (fractalsDiscovered && fractalsOpen)
            {
                if (!fractalsPanel2.activeSelf)
                    fractalsPanel2.SetActive(true);
            }
            else
            {
                if (fractalsPanel2.activeSelf)
                    fractalsPanel2.SetActive(false);
            }
            if (fractalsDiscovered)
            {
                if (!FractalsButton.activeSelf)
                    FractalsButton.SetActive(true);
            }
            else
            {
                if (FractalsButton.activeSelf)
                    FractalsButton.SetActive(false);
            }
            if (TribesOpen)
            {
                if (!TribesPanel.activeSelf)
                {
                    TribesPanel.SetActive(true);
                    //GetComponentInParent<player3>().canAttack2 = false;
                    myplayer.canAttack2 = false;
                }
            }
            else
            {
                if (TribesPanel.activeSelf)
                {
                    TribesPanel.SetActive(false);
                    //GetComponentInParent<player3>().canAttack2 = true;
                    myplayer.canAttack2 = true;
                }
            }

            // AddItemEnterisPressed
            if (Input.GetKeyDown(KeyCode.Return))
            {
                AddItemOK();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                AddItemCancel();
            }

            if(classicsDiscovered)
            {
                woodensword.SetActive(true);
                ability1.SetActive(true);
            }
            if(fractalsDiscovered)
            {
                magicring.SetActive(true);
                ability2.SetActive(true);
            }
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
    public void InventoryOpen()
    {
        InventoryOpened = true;
        //GetComponentInParent<player3>().canAttack2 = false;
        myplayer.canAttack2 = false;

        if(TribesOpen==true)
            TribesPanelClose();
    }
    public void InventoryClose()
    {
        InventoryOpened = false;
        //GetComponentInParent<player3>().canAttack2 = false;
        myplayer.canAttack2 = true;
    }
    public void EnchTableOpen()
    {
        EnchTableOpened = true;
    }
    public void EnchTableClose()
    {
        EnchTableOpened = false;
    }
    public void BasePanelOpen()
    {
        BasePanelOpened = true;
    }
    public void BasePanelClose()
    {
        BasePanelOpened = false;
    }
    public void BasePanel1Open()
    {
        BasePanel1Opened = true;
    }
    public void BasePanel1Close()
    {
        BasePanel1Opened = false;
    }
    public void BasePanel2Open()
    {
        BasePanel2Opened = true;
    }
    public void BasePanel2Close()
    {
        BasePanel2Opened = false;
    }
    public void BasePanel3Open()
    {
        BasePanel3Opened = true;
    }
    public void BasePanel3Close()
    {
        BasePanel3Opened = false;
    }
    public void BasePanel4Open()
    {
        BasePanel4Opened = true;
    }
    public void BasePanel4Close()
    {
        BasePanel4Opened = false;
    }
    public void BasePanel5Open()
    {
        BasePanel5Opened = true;
    }
    public void BasePanel5Close()
    {
        BasePanel5Opened = false;
    }
    public void MiniMarketOpen()
    {
        MiniMarketOpened = true;
    }
    public void MiniMarketClose()
    {
        MiniMarketOpened = false;
    }
    public void CrystInvOpen()
    {
        CrystInvOpened = true;
    }
    public void classicsPanelOpen()
    {
        ClasicsPanelOpened = true;
    }
    public void classicsPanelclose()
    {
        ClasicsPanelOpened = false;
    }
    public void TribesPanelOpen()
    {
        TribesOpen = true;
        if(InventoryOpened==true)
            InventoryClose();
    }
    public void TribesPanelClose()
    {
        TribesOpen = false;
    }
    public void ClassicsPanel2Open()
    {
        classicsPanel2Open = true;
        fractalsOpen = false;
    }
    public void ClassicsPanel2close()
    {
        classicsPanel2Open = false;
    }
    public void ClassicsDiscovered()
    {
        classicsDiscovered = true;
    }
    public void AddWoodenSword()
    {
        woodenswordblocker.SetActive(true);
        inventory.AddItem(_item0, 1);
        //equipment.AddItem(_item0, 1);
    }
    public void EquipWoodenSword()
    {
        InventoryOpen();
        woodenswordblocker.SetActive(true);
        //inventory.AddItem(_item0, 1);
        equipment.AddItem(_item0, 1);
    }
    public void Addability()
    {
        abilityblocker.SetActive(true);
        ability.GetSlots[0].UpdateSlot(_item1, 1);
    }
    public void Addmagicring()
    {
        magicringblocker.SetActive(true);
        inventory.AddItem(_item2, 1);
        //equipment.AddItem(_item2, 1);
    }
    public void EquipRing()
    {
        InventoryOpen();
        woodenswordblocker.SetActive(true);
        //inventory.AddItem(_item0, 1);
        equipment.AddItem(_item2, 1);
    }
    public void addability2()
    {
        ability2blocker.SetActive(true);
        ability.GetSlots[0].UpdateSlot(_item3, 1);
    }
    public void FractalsPanelOpen()
    {
        fractalsPanelOpen = true;
    }
    public void FractalsPanelClose()
    {
        fractalsPanelOpen = false;
    }
    public void FractalsPanel2Open()
    {
        fractalsOpen = true;
    }
    public void FractalsPanel2Close()
    {
        fractalsOpen = false;
    }
    public void FractalsDiscovered()
    {
        fractalsDiscovered = true;
    }
    public void AdItWinOpen()
    {
        myplayer.IsInputEnabled = false;
        inputField.text = "\0";
        AddItemWindow.SetActive(true);
        AIWBlocker.SetActive(true);

    }
    public void AdItWinClose()
    {
        myplayer.IsInputEnabled = true;
        AddItemWindow.SetActive(false);
        AIWBlocker.SetActive(false);

    }
    public void AddItemOK()
    {
        AdItWinClose();
        AdItWinOkButton("ABCDEFGHIJKLMNOPQRSTUVXYWZ0123456789- ", (string inputText) => { Debug.Log("OK: !" + inputText); });
    }
    public void AddItemCancel()
    {
        AdItWinClose();
        AdItWinCalncelButton(() => { Debug.Log("Cancel"); });
    }
    public void AdItWinOkButton(string validCharacters, Action<string> onOk)
    {
        inputField.onValidateInput = (string text, int charIndex, char addedChar) => { return ValidateChar(validCharacters, addedChar); };

        onOk(inputField.text);

        ItemCodeInput(inputField.text, myplayer.playerID);
    }

    private char ValidateChar(string validCharacters, char addedChar)
    {
        if(validCharacters.IndexOf(addedChar) != -1)
        {
            return addedChar;
        }
        else
        {
            return '\0';
        }
    }

    public void AdItWinCalncelButton(Action OnCancel)
    {
        OnCancel();
    }
    public void ItemCodeInput(string text1, int playerID)
    {
        GetComponent<PhotonView>().RPC("ItemVerification", RpcTarget.MasterClient, text1, playerID);
    }
    [PunRPC]
    public void ItemVerification(string text1, int playerID)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for(int i = 0; i<itemCodes.Count; i++)
            {
                if (text1 == (string) itemCodes[i])
                {
                    itemCodes.RemoveAt(i);
                    GetComponent<PhotonView>().RPC("ItemDelivery", RpcTarget.All, 1, playerID);
                }
            }
        }
    }
    [PunRPC]
    public void ItemDelivery(int itemID, int playerID)
    {
        if(playerID == myplayer.playerID)
        {
            if(itemID == 1)
                inventory.AddItem(_item0, 1);
        }
    }
}
