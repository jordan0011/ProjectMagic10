using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class player3 : MonoBehaviourPunCallbacks
{
    public int playerID;
    public int health;
    public int maxheath;
    public bool IsMasterClient;
    private bool cursor = false;
    public bool IsInputEnabled = true;
    public Animator animator;
    private bool IsBinded = false;
    private Transform mainCameraTransform;
    private float turnSmoothVelocity;
    private float turnSmoothTime = 0.06f;
    [SerializeField] float movementSpeed = 5f;
    private CharacterController controller;
    private bool canRoll = true;
    private float rollTime = 0.5f;
    private float rollTime0 = 0f;
    private float attackSpeed = 0.85f;
    private float attackSpeed0 = 0;
    private int clicks = 0;
    private float doubleclickTime = 0.5f;
    private float doubleclickTime0 = 0;
    private bool doubleclick = false;
    private bool groundedPlayer = true;
    private bool canAttack = true;
    public bool canAttack2 = true;
    public bool jump = false;
    private float gravityValue = -9.81f * 3f;
    private Vector3 playerVelocity;
    private float jumpHeight = 0.7f;
    [SerializeField] private LayerMask ground;
    [SerializeField] private GameObject SphereforGroundDetection;
    [SerializeField] private LayerMask enemies;
    private Transform attackTarget;
    public Vector3 movement;
    [SerializeField] private Transform BasicAttackHitBox;
    private Transform targetTransform;
    private EnemyCanvas3 enemyCanvas;
    private Enemy3 enemy;
    private playerCanvas3 enemyPlayerCanvas;
    private player3 enemyPlayer;
    [SerializeField] private LayerMask camera1;

    private GameObject fireball01;
    public float A0cooldown0 = 0;
    public float A0cooldown = 1.2f;
    private bool readytoFire0 = false;

    private GameObject slash01;
    public float A1cooldown0 = 0;
    public float A1cooldown = 1.4f;
    private bool readytoFire1 = false;

    private GameObject earthshat;
    public float A2cooldown0 = 0;
    public float A2cooldown = 3;
    private bool readytoFire2 = false;

    private GameObject yasuoqripoff;
    public float A3cooldown0 = 0;
    public float A3cooldown = 2;
    private bool readytoFire3 = false;

    private GameObject RangedBasic;
    public float RBcooldown0 = 0;
    public float RBcooldown = 1;
    private bool readytoFireRB = false;


    public GameObject targetPointer;

    public GameObject targetPointer2;

    public Vector3 AimRotation;
    private bool aimhit = false;

    public bool warrior = true;
    public bool fire = false;
    private GameObject RangedBasicAttack;


    private ServerSide server;
    private ItemDropService ItemService;
    public Transform yasuoqripoff1;

    public LayerMask rayprojection;

    public int damage = 20;
    public int Ab0Damage = 50;
    public InventoryObject inventory;
    public InventoryObject equipment;
    public InventoryObject enctable;
    public InventoryObject weaponCrystal;
    public InventoryObject virtualBox;
    public InventoryObject abilities;
    public InventoryObject ClasicsAbilities;

    public PlayerStats playerstats;

    public LayerMask everything;

    private int value1, value2, value3, value4, value5;

    private bool Unarmed = false;

    public int ability1 = 0;
    public int ability2 = 0;

    private Vector3 aimdirection;

    public float deathtimer;

    public MeleeWeaponTrail swordtrail;

    public void TakeItem(int incomingID, int ItemID)
    {
        if(playerID == incomingID)
        {
            Item _item = new Item(ItemService.itemList[ItemID]);
            inventory.AddItem(_item, 1);
        }
    }
    public void OnApplicationQuit()
    {
        inventory.Clear();
        equipment.Clear();
        enctable.Clear();
        weaponCrystal.Clear();
        virtualBox.Clear();
        abilities.Clear();
        ClasicsAbilities.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCameraTransform = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        server = GameObject.Find("ServerSide").GetComponent<ServerSide>();
        ItemService = GameObject.Find("ServerSide").GetComponentInChildren<ItemDropService>();

        playerstats = GetComponent<PlayerStats>();

        A1cooldown = 1.4f;
        A0cooldown = 1.2f;

        
    }

    // Update is called once per frame
    void Update()
    {
        damage = playerstats.Damage;
        value1 = playerstats.value1;
        value2 = playerstats.value2;
        value3 = playerstats.value3;
        value4 = playerstats.value4;
        value5 = playerstats.value5;

        A0cooldown = playerstats.cooldown31; //Unstable Flame
        A2cooldown = playerstats.cooldown4;  //EarthBlazer
        A3cooldown = playerstats.cooldown1;  //Piercing strike
        A1cooldown = playerstats.cooldown2;  //Blue Slash

        if (PhotonNetwork.IsMasterClient)
            IsMasterClient = true;
        else
            IsMasterClient = false;


        if (photonView.IsMine)
        {
            TakeInput();
        }
        else
        {
            playerID = GetComponent<NetworkPlayer3>().playerid;
        }

    }


    private void TakeInput()
    {
        if(playerstats.BasicAttackID == 1)
        {
            if (Unarmed == false)
                warrior = true;
            else
                warrior = false;
            fire = false;
        }
        else if(playerstats.BasicAttackID == 2)
        {
            fire = true;
            warrior = false;
        }
        else
        {
            warrior = false;
            fire = false;
        }

        ability1 = playerstats.Ability1ID;
        ability2 = playerstats.Ability2ID;


        if (warrior)
            attackSpeed = 0.85f;
        if (fire)
            attackSpeed = 0.68f;

        if (playerstats.model3D == 0 || playerstats.model3D== 6 || playerstats.model3D == 7)
        {
            animator.SetBool("Unarmed", true);
            Unarmed = true;
        }
        else
        {
            animator.SetBool("Unarmed", false);
            Unarmed = false;
        }


        /// MOVEMENT ///

        movement = new Vector3
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = 0f,
            z = Input.GetAxisRaw("Vertical"),
        }.normalized;


        if (movement.magnitude >= 0.1f && !IsBinded && IsInputEnabled)
        {
            animator.SetBool("IsWalking", true);
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + mainCameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(moveDirection.normalized * Time.deltaTime * movementSpeed);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        if (movement.magnitude >= 0.1f && attackSpeed0 <= 0.1f && attackSpeed0 > 0f && groundedPlayer)
        {
            animator.SetBool("IsWalking", true);
        }
        if (fire && Input.GetKeyDown(KeyCode.Mouse0) && IsInputEnabled && cursor && Input.GetKey(KeyCode.Mouse1))
        {
            Aim();
        }
        /// BASIC ATTACK ///
        if (fire)
            animator.SetBool("IsAttacking", false);
        if (warrior)
            animator.SetBool("AttackRB", false);
        //if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack && canRoll && !A1attack)
        if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack && canAttack2 && canRoll && A3cooldown - A3cooldown0 > 0.5f && A1cooldown - A1cooldown0 > 0.5f&&IsInputEnabled)
        {
            if (warrior)
            {
                attackTarget = null;
                TargetFind();
            }
            attackSpeed0 = attackSpeed;
            if (warrior)
            {
                animator.SetBool("IsAttacking", true);
                animator.SetTrigger("Attack");
                StartCoroutine(AttackDamageDelay(0.36f));
            }
            if (fire)
            {
                animator.SetBool("AttackRB", true);
                readytoFireRB = true;
            }
            canAttack = false;
            if (warrior)
                IsBinded = true;
        }
        if (attackSpeed0 > 0)
        {
            if (canRoll)
                FaceTarget(attackTarget);
            attackSpeed0 -= Time.deltaTime;
            if (fire && animator.GetBool("AttackRB"))
            {
                //if (attackSpeed - attackSpeed0 >= 0.1f & readytoFireRB)
                if (readytoFireRB)
                {
                    Quaternion finalRotation;
                    if (enemy == null)
                    {
                        if(Input.GetKey(KeyCode.Mouse1))
                        {
                            if(aimhit)
                            {
                                //finalRotation = targetPointer.transform.rotation;
                                finalRotation = targetPointer.transform.rotation;
                            }
                            else
                                finalRotation = mainCameraTransform.rotation;
                        }
                        else
                        {
                            finalRotation = mainCameraTransform.rotation;
                        }
                    }
                    else
                    { 
                        finalRotation = targetPointer.transform.rotation;
                    }
                    object[] content = new object[] { transform.position, finalRotation, playerID, value3 };
                    //server.RangedBasicAttack1Cast(content);
                    server.photonView.RPC("RangedBasicAttack1Cast", RpcTarget.MasterClient, content as object);
                    readytoFireRB = false;
                }
                if (canRoll)
                {
                    float targetAngle;  // turn forward
                    float angle;
                    if (enemy == null)
                    {
                        targetAngle = mainCameraTransform.eulerAngles.y;  // turn forward
                        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                       
                    }
                    else
                    {
                        targetAngle = targetPointer.transform.eulerAngles.y;  // turn forward
                        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                    }
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                }
            }
            if (fire && movement.magnitude > 0.1f && attackSpeed0 < 0f)
            {
                animator.SetBool("AttackRB", false);
                //animator.SetBool("IsWalking", false);
            }
            if (warrior)
            {
                IsBinded = true;
            }
            if (fire)
            {
                if (attackSpeed - attackSpeed0 < 0.3f)
                    IsBinded = true;
            }
        }
        if (warrior)
        {
            if (attackSpeed - attackSpeed0 >= 0.2f || attackSpeed0 < 0.1f)
            {
                animator.SetBool("HasAttacked", true);
            }
            else
            {
                animator.SetBool("HasAttacked", false);
            }
        }

        if (attackSpeed0 <= 0)
        {
            canAttack = true;
            if (warrior)
            {
                animator.SetBool("IsAttacking", false);

            }
            if (fire)
            {
                animator.SetBool("AttackRB", false);
            }
            IsBinded = false;
        }
        if (fire && attackSpeed0 < 0.4f)
        {
            animator.SetBool("AttackRB", false);
        }

        /// BASIC ATTACK RANGED MAGE///

        /// JUMP !

        Vector3 startingDirection = transform.forward;

        float targetAngle2 = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + mainCameraTransform.eulerAngles.y;
        if (canRoll && rollTime0 <= 0.1f)
            startingDirection = Quaternion.Euler(0f, targetAngle2, 0f) * Vector3.forward;
        

        if (Input.GetKeyDown(KeyCode.Space) && attackSpeed0 < 0.5f && A2cooldown- A2cooldown0 >= 0.8f && IsInputEnabled)  // the last float is the time for auto cancel enable calculated from the end
        {
            if (clicks < 1)
                clicks++;
            else
            {
                if (rollTime - rollTime0 < 0.35f)
                    clicks++;
            }
            if (clicks == 1)
                doubleclickTime0 = doubleclickTime;
        }
        if (clicks > 2)
            clicks = 0;
        if (doubleclickTime0 > 0)
            doubleclickTime0 -= Time.deltaTime;
        if (clicks == 2 && doubleclickTime0 > 0)
        {
            doubleclick = true;
            clicks = 0;
        }
        else
        {
            doubleclick = false;
        }
        if (clicks == 1 && canRoll)
        {
            transform.rotation = Quaternion.Euler(0f, targetAngle2, 0f);
            startingDirection = transform.forward;
            rollTime0 = rollTime;
            animator.SetBool("IsRolling", true);
            animator.SetBool("IsAttacking", false); // na kanw kati gia ta RANGED DASH AFTER ATTACK BUGGED
            canRoll = false;
            IsBinded = true;
            photonView.RPC("rollColliderSet", RpcTarget.All);
        }
        if (rollTime0 > 0)
        {
            rollTime0 -= Time.deltaTime;
            //if (rollTime0 < rollTime - 0.2f && doubleclick == false)
            if (rollTime - rollTime0 < 0.4f && rollTime - rollTime0 > 0.15f && doubleclick == false)
            {
                controller.Move(startingDirection * Time.deltaTime * 10);
                IsBinded = true;
            }
            else
            {
                IsBinded = false;
            }
            if (clicks == 1 && rollTime0 < rollTime - doubleclickTime)
                clicks = 0;

        }

        if (rollTime0 <= 0 || doubleclick)
        {
            canRoll = true;
            if (groundedPlayer)
            {
                animator.SetBool("IsRolling", false);
             photonView.RPC("rollColliderDeset", RpcTarget.All);
            }
            rollTime0 = 0;
            if (canAttack || !groundedPlayer)
                IsBinded = false;

        }

        /// Jump! ///
        if (doubleclick && groundedPlayer)
        {
            jump = true;
            animator.SetBool("JumpPressed", true);
        }
        else
        {
            //if (jump == false && groundedPlayer)
            //animator.SetBool("IsJumping", false);
        }

        groundedPlayer = Physics.CheckSphere(SphereforGroundDetection.transform.position, SphereforGroundDetection.transform.localScale.y / 2, ground);
        animator.SetBool("IsJumping", !groundedPlayer);
        if (playerVelocity.y < 0 && groundedPlayer)
        {
            playerVelocity.y = 0f;
        }
        if (jump)
        {
            //animator.SetBool("IsJumping", true);
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            jump = false;
            animator.SetBool("JumpPressed", true);
        }
        else
        {
            animator.SetBool("JumpPressed", false);
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (transform.position.y < -20f)
        {
            transform.position = new Vector3(0, 0, 0);
        }


        // target alinment
        if (fire &&Input.GetKeyDown(KeyCode.Alpha3)&&Input.GetKey(KeyCode.Mouse1) && IsInputEnabled && cursor && A0cooldown0 <= 0)
        {
            Aim();
        }
        // ABILITY 0 Binded in 3 key

        if (Input.GetKeyDown(KeyCode.Alpha3) && A0cooldown0 <= 0 && ability1== 2 && attackSpeed0 < 0.4f && IsInputEnabled&& canRoll)
        {
            //fireball01 = PhotonNetwork.Instantiate("Fireball01", transform.position, transform.rotation);
            readytoFire0 = true;
            A0cooldown0 = A0cooldown;
            animator.SetBool("Ability04", true);
        }
        if (A0cooldown0 > 0)
            A0cooldown0 -= Time.deltaTime;
        if (A0cooldown - A0cooldown0 < 0.5f)
        {
            IsBinded = true;
        }
        if (A0cooldown - A0cooldown0 > 0.05f && readytoFire0) // FIREBALL
        {
            Quaternion finalRotation;
            if (enemy == null)
            {
                if (Input.GetKey(KeyCode.Mouse1))
                {
                    if (aimhit)
                    {
                        finalRotation = targetPointer.transform.rotation;
                    }
                    else
                        finalRotation = mainCameraTransform.rotation;
                }
                else
                {
                    finalRotation = mainCameraTransform.rotation;
                }
            }
            else
            {
                finalRotation = targetPointer.transform.rotation;
            }


            object[] content = new object[] { transform.position, finalRotation, playerID, value4 };
            //server.Ability00Cast(content);
            server.photonView.RPC("Ability00Cast", RpcTarget.MasterClient, content as object);
            readytoFire0 = false;
        }
        if (A0cooldown - A0cooldown0 < 0.4f)
        {
            IsBinded = true;

            if (canRoll)
            {
                float targetAngle;  // turn forward
                float angle;
                if (enemy == null)
                {
                    targetAngle = mainCameraTransform.eulerAngles.y;  // turn forward
                    angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                    
                }
                else
                {
                    targetAngle = targetPointer.transform.eulerAngles.y;  // turn forward
                    angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                }
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
            //transform.rotation = Quaternion.Euler(0f, mainCameraTransform.rotation.y, 0f);
        }
        if (A0cooldown - A0cooldown0 > 0.5f)
        {
            animator.SetBool("Ability04", false);
        }


        if (Input.GetKeyDown(KeyCode.Alpha3) && Input.GetKey(KeyCode.Mouse1) && IsInputEnabled && cursor && A3cooldown0 <= 0)
        {
            Aim();
        }

        // ABILITY 0 Binded in 3 key  (FireBall)

        if (Input.GetKeyDown(KeyCode.Alpha3) && A3cooldown0 <= 0 && ability1 == 1 && Unarmed == false && photonView.IsMine && attackSpeed0 < 0.4f && A1cooldown - A1cooldown0 > 0.5f && IsInputEnabled && rollTime0 <= 0)
        {
            //fireball01 = PhotonNetwork.Instantiate("Fireball01", transform.position, transform.rotation);
            readytoFire1 = true;
            A3cooldown0 = A3cooldown;
            animator.SetBool("Ability01", true);
        }
        if (A3cooldown0 > 0)
            A3cooldown0 -= Time.deltaTime;
        if (A3cooldown - A3cooldown0 > 0.1f && readytoFire1 && photonView.IsMine) // EPEMVASI
        {
            yasuoqripoff1 = targetPointer.transform;
            yasuoqripoff1.position = transform.position + transform.forward;

            Quaternion finalRotation;

            if (enemy == null)
            {
                if (Input.GetKey(KeyCode.Mouse1))
                {
                    if (aimhit)
                    {
                        finalRotation = targetPointer.transform.rotation;
                    }
                    else
                        finalRotation = mainCameraTransform.rotation;
                }
                else
                {
                    finalRotation = mainCameraTransform.rotation;
                   
                }
            }
            else
            {
                finalRotation = targetPointer.transform.rotation;
            }

            object[] content = new object[] { yasuoqripoff1.position, finalRotation, playerID, value1};
            server.photonView.RPC("Ability01Cast", RpcTarget.MasterClient, content as object);
            readytoFire1 = false;


        }
        if (A3cooldown - A3cooldown0 < 0.35f)
        {
            IsBinded = true;

            if (canRoll)
            {
                float targetAngle;  // turn forward
                float angle;
                if (enemy == null)
                {
                    targetAngle = mainCameraTransform.eulerAngles.y;  // turn forward
                    angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                }
                else
                {
                    targetAngle = targetPointer.transform.eulerAngles.y;  // turn forward
                    angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                }
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
            //transform.rotation = Quaternion.Euler(0f, mainCameraTransform.rotation.y, 0f);
        }
        else
            animator.SetBool("Ability01", false);

        if (A3cooldown - A3cooldown0 < 0.6f)
            IsBinded = true;

        // ABILITY 1 Binded in E key

        if (Input.GetKeyDown(KeyCode.E) && A2cooldown0 <= 0 && ability2 == 2&& attackSpeed0 < 0.4f && IsInputEnabled && rollTime0 <= 0)
        {
            //fireball01 = PhotonNetwork.Instantiate("Fireball01", transform.position, transform.rotation);
            readytoFire3 = true;
            A2cooldown0 = A2cooldown;
            animator.SetBool("Ability03", true);
        }
        if (A2cooldown0 > 0)
            A2cooldown0 -= Time.deltaTime;
        if (A2cooldown - A2cooldown0 > 0.05f && readytoFire3)
        {//GameObject shlashHitbox;
            if (canRoll)
            {
                Vector3 newposition;
                Quaternion finalrotation;
                if (enemy == null)
                {
                    newposition = transform.position + new Vector3(0, -0.9f, 0);
                    finalrotation = transform.rotation;
                    //shlashHitbox = PhotonNetwork.Instantiate("SlashWaveCollider", transform.position + new Vector3(0, -0.6f, 0), transform.rotation);
                }
                else
                {
                    Vector3 distance = transform.position + targetTransform.position;
                    newposition = transform.position + new Vector3(0, -0.9f, 0);
                    finalrotation = targetPointer.transform.rotation;
                    //shlashHitbox = PhotonNetwork.Instantiate("SlashWaveCollider", transform.position + new Vector3(0, -0.6f, 0), targetPointer.transform.rotation);
                }
                object[] content = new object[] { newposition, finalrotation, playerID, value5};
                //server.Ability03Cast(content);
                server.photonView.RPC("Ability03Cast", RpcTarget.MasterClient, content as object);
                readytoFire3 = false;
            }
        }
        if (A2cooldown - A2cooldown0 < 0.5f)    // katw apo 0. f
        {
            IsBinded = true;

            if (canRoll)
            {
                float targetAngle;  // turn forward
                float angle;
                if (enemy == null)
                {
                    //targetAngle = mainCameraTransform.eulerAngles.y;  // turn forward
                    //angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                }
                else
                {
                    targetAngle = targetPointer.transform.eulerAngles.y;  // turn forward
                    angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                }
            }

            //transform.rotation = Quaternion.Euler(0f, mainCameraTransform.rotation.y, 0f);
        }
        if (A2cooldown - A2cooldown0 > 0.5f)
            animator.SetBool("Ability03", false);

        // ABILITY 1 Binded in E key

        if (Input.GetKeyDown(KeyCode.E) && A1cooldown0 <= 0 && ability2 == 1 && photonView.IsMine && attackSpeed0 < 0.4f && A3cooldown - A3cooldown0 > 0.5f && IsInputEnabled)
        {
            readytoFire2 = true;
            A1cooldown0 = A1cooldown;
            animator.SetBool("Ability2", true);
        }
        if (A1cooldown0 > 0)
            A1cooldown0 -= Time.deltaTime;
        if (A1cooldown - A1cooldown0 > 0.15f && readytoFire2)
        {
            object[] content = new object[] { (transform.position + transform.forward), transform.rotation, playerID, value2 };
            server.photonView.RPC("Ability02Cast", RpcTarget.MasterClient, content as object);
            readytoFire2 = false;

            animator.SetBool("Ability2", false);
        }
        if (A1cooldown - A1cooldown0 < 0.5f)    // katw apo 0. f
        {
            IsBinded = true;

            if (canRoll)
            {
                float targetAngle;  // turn forward
                float angle;
                if (enemy == null)
                {
                    //targetAngle = mainCameraTransform.eulerAngles.y;  // turn forward
                    //angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                }
                else
                {
                    targetAngle = targetPointer.transform.eulerAngles.y;  // turn forward
                    angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                }
            }
            //transform.rotation = Quaternion.Euler(0f, mainCameraTransform.rotation.y, 0f);
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
            cursor = !cursor;

        //if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Mouse1))
        if (!cursor && Input.GetKeyDown(KeyCode.Mouse1) && IsInputEnabled)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~camera1))
            {
                if (hit.collider.gameObject.layer == 10)  // enemies
                {
                    if(enemyCanvas != null)
                    {
                        enemyCanvas.MarkDeset();
                    }
                    enemy = null;
                    if (enemyPlayerCanvas != null)
                        enemyPlayerCanvas.MarkDeset();
                    enemyPlayer = null;
                    Debug.Log("Ther is an enemy");
                    hit.collider.gameObject.GetComponentInParent<EnemyCanvas3>().MarkSet();
                    enemyCanvas = hit.collider.GetComponentInParent<EnemyCanvas3>();
                    enemy = hit.collider.GetComponentInParent<Enemy3>();
                }
                else
                {
                    if (hit.collider.gameObject.layer == 9 && hit.collider.gameObject.GetComponent<player3>().playerID != playerID || hit.collider.gameObject.layer == 10)
                    {
                        enemyCanvas.MarkDeset();
                        enemy = null;
                    }
                }
                if (hit.collider.gameObject.layer == 9 && hit.collider.gameObject.GetComponent<player3>().playerID != playerID) // players
                {
                    Debug.Log("Ther is an enemy player");
                    hit.collider.gameObject.GetComponentInParent<playerCanvas3>().MarkSet();
                    enemyPlayerCanvas = hit.collider.GetComponentInParent<playerCanvas3>();
                    enemyPlayer = hit.collider.GetComponentInParent<player3>();
                }
                else
                {
                    if (hit.collider.gameObject.layer == 9 && hit.collider.gameObject.GetComponent<player3>().playerID != playerID)
                    {
                        enemyPlayerCanvas.MarkDeset();
                        enemyPlayer = null;
                    }
                }
                if (hit.collider.gameObject.layer == 8)
                {
                    if (enemyCanvas != null)
                        enemyCanvas.MarkDeset();
                    enemy = null;
                    if (enemyPlayerCanvas != null)
                        enemyPlayerCanvas.MarkDeset();
                    enemyPlayer = null;
                }
            }
        }

        if (enemy != null)
        {
            targetTransform = enemy.transform;
            targetPointer.transform.LookAt(targetTransform);
        }
        if (enemyPlayer != null)
        {
            targetTransform = enemyPlayer.transform;
            targetPointer.transform.LookAt(targetTransform);
        }

        if (deathtimer > 0)
        {
            if(controller.enabled)
            controller.enabled = false;
        }
        else
        {
            if(!controller.enabled)
            controller.enabled = true;
        }
    }

    void Aim()
    {
        Ray ray2 = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit2;
        if (Physics.Raycast(ray2, out hit2, Mathf.Infinity, rayprojection))
        {
            Debug.Log("WE HIT!");
            Debug.Log(Input.mousePosition);
            aimhit = true;

            Ray ray31 = new Ray(hit2.point, Camera.main.transform.forward);
            RaycastHit hit31;
            if(Physics.Raycast(ray31, out hit31, Mathf.Infinity, everything))
            {
                aimdirection = Camera.main.transform.forward;
                transform.LookAt(hit31.point);
                targetPointer.transform.LookAt(hit31.point);
            }
        }
        else
        {
            aimhit = false;
        }
    }

    void TargetFind()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 2f, enemies);
        foreach (Collider enemy in hitEnemies)
        {
            if(enemy.gameObject.layer == 9) // Player entities
            {
                if(playerID != enemy.gameObject.GetComponent<player3>().playerID)
                    attackTarget = enemy.transform;
            }
            else   // propably Environment Enemies
            {
                attackTarget = enemy.transform;
            }
        }
    }

    void FaceTarget(Transform T_target)
    {
        if (T_target != null)
        {
            Vector3 direction = (T_target.position - transform.position).normalized;
            //movement = direction;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 15f);
        }
    }

    IEnumerator AttackDamageDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Attack();
    }
    [PunRPC]
    void Attack()
    {
        float critical = playerstats.Critical;
        int _damage = playerstats.Damage;
        int onhit = playerstats.OnHit;
        int lifesteal = playerstats.Lifesteal;
        int lifestealhealing;

        float randValue = Random.value;
        if (randValue < critical / 100f)
        {
            _damage = _damage * 2;
        }
        //lifesteal -> 
        lifestealhealing =(int) (_damage * (lifesteal / 100f));

        _damage = _damage + onhit;

        Collider[] hitEnemies = Physics.OverlapBox(BasicAttackHitBox.position, BasicAttackHitBox.localScale / 2, BasicAttackHitBox.rotation, enemies);
        foreach (Collider enemy in hitEnemies)
        {
            //enemy.GetComponent<Health>().photonView.RPC("TakeDamage", RpcTarget.All);   // 8: player , 10: enemies
            if (enemy.gameObject.layer == 9&& playerID != enemy.gameObject.GetComponent<player3>().playerID) // Players
            {
                enemy.GetComponent<NetworkPlayer3>().photonView.RPC("Damage", RpcTarget.All, _damage);
                server.GetComponent<ServerSide>().BasicAttackHitVFX(enemy.transform);
                GetComponent<NetworkPlayer3>().photonView.RPC("Damage", RpcTarget.All, -lifestealhealing);
            }
            else
            {
                if (enemy.gameObject.layer == 10) // Environment Enemies
                {
                    //enemy.GetComponentInParent<Enemy3>().perpetrator = transform;
                    enemy.GetComponentInParent<Enemy3>().photonView.RPC("DamageToEnemyBasicAttack", RpcTarget.All, _damage);
                    server.GetComponent<ServerSide>().BasicAttackHitVFX(enemy.transform);
                    GetComponent<NetworkPlayer3>().photonView.RPC("Damage", RpcTarget.All, -lifestealhealing); //lifesteal
                }
            }
        }
    }
    [PunRPC]
    public void rollColliderSet()
    {
        Vector3 center = new Vector3(0f, -0.45f, 0f);
        controller.center = center;
        controller.height = 0.96f;
    }
    [PunRPC]
    public void rollColliderDeset()
    {
        Vector3 center = new Vector3(0f, -0.13f, 0f);
        controller.center = center;
        controller.height = 1.6f;
    }

}


