using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Enemy3 : MonoBehaviourPun
{
    public int UnitID;

    public int enemyType = 0;

    public int health;
    public int maxhealth = 100;

    [SerializeField]
    private GameObject me;

    public ServerSide server;
    public ItemDropService Itemdrop;

    public LayerMask camera1;

    [SerializeField]
    private GameObject canvas;
    public GameObject mesh;
    [SerializeField]
    private GameObject SphereforGroundDetection;
    [SerializeField]
    private bool walk = false;
    [SerializeField]
    private bool groundedEnemy;
    RaycastHit hit;
    [SerializeField]
    private LayerMask ground;
    private Vector3 EnemyVelocity;
    [SerializeField]
    private CharacterController controller;
    private float gravityValue = -9.81f * 3f;
    [SerializeField]
    private LayerMask players;
    public Transform target;
    private float turnSmoothVelocity;
    private float turnSmoothTime = 0.06f;
    private Vector3 movement;
    public float moveSpeed = 3f;
    public Animator animator;
    [SerializeField] private Transform BasicAttackHitBox;
    [SerializeField]
    private float attackSpeed = 1.5f;
    [SerializeField]
    private float attackSpeed0 = 0f;
    private bool hasAttacked = false;
    [SerializeField]
    private float range = 2f;
    public GameObject savepoint1;
    public GameObject savepoint2;
    public EnemyLibrary enemyLibrary;
    private bool isDead = false;
    public bool startRolling = false;
    public bool isRolling = false;
    public float Rolltime = 1.1f;
    public float Rolltime0 = 0;
    public float rollcooldown = 2f;
    public float rollcooldown0 = 0;
    public GameObject rollingHitBox;
    public float focusRange = 2f;
    Vector3 generaldirection;
    float rangedflameattack = 1f;
    float rangedflameattack0 = 0f;
    bool hasrangedattack = false;
    bool hasmuzzled = false;
    [SerializeField]
    private int fireballcount0 =0;
    [SerializeField]
    private int fireballcount = 4;
    [SerializeField]
    private float fireballcooldown0 = 0f;
    [SerializeField]
    private float fireballcooldown = 8f;
    [SerializeField]
    private bool canfireball = true;
    [SerializeField]
    private bool fireball = true;
    [SerializeField]
    private float lasercooldown0 = 0;
    [SerializeField]
    private float lasercooldown = 3f;
    [SerializeField]
    private int lasercount0 = 0;
    [SerializeField]
    private int lasercount = 2;
    public bool laserwarning = false;
    private Vector3 laserStart;
    private Vector3 laserDirection;

    public GameObject beamLineRendererPrefab;
    public GameObject beamStartPrefab;
    public GameObject beamEndPrefab; 

    [SerializeField]
    private GameObject beamStart;
    [SerializeField]
    private GameObject beamEnd;
    [SerializeField]
    private GameObject beam;
    private LineRenderer line;

    public float beamEndOffset = 1f;
    public float textureScrollSpeed = 8f;
    public float textureLengthScale = 3;

    float struttime0 = 0;
    float struttime = 6f;

    private bool isWalken = false;

    // Start is called before the first frame update
    void Start()
    {
        attackSpeed0 = 0;
        Rolltime = 0.8f;

        health = maxhealth;

        server = GameObject.Find("ServerSide").GetComponent<ServerSide>();
        Itemdrop = GameObject.Find("ServerSide").GetComponentInChildren<ItemDropService>();
        if(PhotonNetwork.IsMasterClient)
           photonView.RequestOwnership();
        if(enemyType == 1)
        {
            savepoint1 = GameObject.Find("savepoint1");
            savepoint2 = GameObject.Find("savepoint2");
        }

        enemyLibrary.TestFuction();
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            ShowCanvas();
        }
        else
        {
            if (health < maxhealth&&!isDead)
                canvas.SetActive(true);
        }
        if(health<=0 || transform.position.y < -3f)
        {
            if(PhotonNetwork.IsMasterClient&&!isDead)
            {
                Die();
            }
        }
        if(PhotonNetwork.IsMasterClient&&!isDead)
        {
            if(enemyType == 0) //Skeleton 
            {
                EnemyAI();
            }
            if(enemyType == 1) // Ball
            {
                Enemy enemy1 = new Enemy(34, 1.62f, 8f, 2f);
                //Debug.Log(enemy1.toString());
                EnemyAI1();
            }
            if(enemyType == 3) // FlameGuy
            {
                focusRange = 8f;
                EnemyAI2();
            }
            if(enemyType == 2) // cannon
            {
                focusRange = 9f;
                EnemyAI3();
            }
        }
    }

    [PunRPC]
    public void DamageToEnemy(int _damage)
    {
        if(_damage>=0)
        {
            if (health - _damage >= 0)
                health -= _damage;
            else
                health = 0;
        }
        else
        {
            if (health - _damage > maxhealth)
                health = maxhealth;
            else
                health -= _damage;
        }

        //Physics.Raycast(transform.position,   Camera.main.transform.position-transform.position, 100f, camera1);
        if (Physics.Raycast(transform.position, Camera.main.transform.position- transform.position, out hit, 300, camera1)&&_damage>=0)
        {
            Camera.main.GetComponent<customCameraScript>().Damagetext(hit.point, _damage);
            //Camera.main.GetComponent<customCameraScript>().Xptext(transform.position, _damage);
        }

    }
    [PunRPC]
    public void DamageToEnemyBasicAttack(int _damage)
    {
        if (health - _damage >= 0)
            health -= _damage;
        else
            health = 0;

        if (Physics.Raycast(transform.position, Camera.main.transform.position - transform.position, out hit, 300, camera1))
        {
            Camera.main.GetComponent<customCameraScript>().Damagetext(hit.point, _damage);
        }

    }

    [PunRPC]
    public void EnemyHeal()
    {
        health = maxhealth;
    }
    public void Die()
    {
        photonView.RPC("DeadXpBounty", RpcTarget.All, 60);

        server.photonView.RPC("EnemyDied", RpcTarget.MasterClient, UnitID);

        if(enemyType==0)
        {
            isDead = true;
            animator.SetBool("Died", true);
            controller.enabled = false;
            canvas.SetActive(false);
            rollingHitBox.SetActive(false);

            //Itemdrop.photonView.RPC("ItemDropSystem", RpcTarget.MasterClient, itemcode, transform.position + new Vector3(0, 1f, 0));
            int randomint = (int)Random.Range(11, 18);
            Itemdrop.photonView.RPC("ItemDropSystem", RpcTarget.MasterClient, randomint, transform.position + new Vector3(0.3f, 1.2f, 0.3f));

            target.gameObject.GetComponent<NetworkPlayer3>().photonView.RPC("Damage", RpcTarget.AllBuffered, -80); // heal

            //target.gameObject.GetComponent<PlayerStats>().AddEssence(2000);
            target.gameObject.GetComponent<PhotonView>().RPC("AddEssence", RpcTarget.AllBuffered, 92);

            GetComponent<PhotonView>().RPC("ShowXp", RpcTarget.AllBuffered);  // new XP Show method

            PhotonNetwork.Destroy(me);
        }
        else
        {
            StartCoroutine(DeathCoroutine(1f));

            isDead = true;
            animator.SetBool("Died", true);
            controller.enabled = false;
            canvas.SetActive(false);
            rollingHitBox.SetActive(false);

            //Itemdrop.photonView.RPC("ItemDropSystem", RpcTarget.MasterClient, itemcode, transform.position + new Vector3(0, 1f, 0));
            int randomint = (int)Random.Range(11, 20);
            Itemdrop.photonView.RPC("ItemDropSystem", RpcTarget.MasterClient, randomint, transform.position + new Vector3(0.3f, 1.2f, 0.3f));

            target.gameObject.GetComponent<NetworkPlayer3>().photonView.RPC("Damage", RpcTarget.AllBuffered, -80); // heal

            //target.gameObject.GetComponent<PlayerStats>().AddEssence(2000);
            target.gameObject.GetComponent<PhotonView>().RPC("AddEssence", RpcTarget.AllBuffered, 92);

            GetComponent<PhotonView>().RPC("ShowXp", RpcTarget.AllBuffered);  // new XP Show method
        }

    }
    [PunRPC]
    public void DeadXpBounty(int xpAmount)
    {
        target.gameObject.GetComponent<PhotonView>().RPC("AddXP", RpcTarget.All, xpAmount);
    }
    [PunRPC] void ShowXp()
    {
        Camera.main.GetComponent<customCameraScript>().Xptext(transform.position, 60);
    }
    private void EnemyAI()
    {
        if (Input.GetKeyDown(KeyCode.I))
            focusRange = 40;
        if (health < maxhealth)
        {
            focusRange = 30f;
            if(isWalken == false)
            {
                CrewAct(10f);
                isWalken = true;
            }
        }
        groundedEnemy = Physics.CheckSphere(SphereforGroundDetection.transform.position, SphereforGroundDetection.transform.localScale.y / 2, ground);

        if (EnemyVelocity.y < 0 && groundedEnemy)
        {
            EnemyVelocity.y = 0f;
        }
        
        EnemyVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(EnemyVelocity * Time.deltaTime);


        Vector3 direction;
        float angle, targetAngle;

        if(target == null)
        {
            TargetFind();
        }
        else
        { // When it is on a target
            direction = target.position - transform.position;
            generaldirection = direction;
            
            
            targetAngle =Mathf.Atan2(direction.x, direction.z)*Mathf.Rad2Deg; // turn forward
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            movement = direction;
            movement.Normalize();

            


            if (direction.magnitude>range) //out of range
            {
                if(attackSpeed0<0.4f)
                {
                    attackSpeed0 = 0;
                    animator.SetBool("IsWalking", true);
                    moveEnemy(movement);
                }
            }
            else // in range
            {
                animator.SetBool("IsWalking", false);

                if(attackSpeed0 <=0f)
                {
                    attackSpeed0 = attackSpeed;
                    animator.SetBool("IsAttacking", true);
                    hasAttacked = false;
                }

                if(attackSpeed0<0.8f&&!hasAttacked&&animator.GetBool("IsAttacking"))
                {
                    Attack();
                    hasAttacked = true;
                }
            }
            if(direction.magnitude>4.5f)
                animator.SetBool("IsAttacking", false);

            if (attackSpeed0 > 0)
            {
                attackSpeed0 -= Time.deltaTime;
            }

            if (direction.magnitude>14)  // when to lose target
            {
                if (animator.GetBool("IsWalking"))
                    animator.SetBool("IsWalking", false);
                target = null;
                focusRange = 10f;

                canvas.SetActive(false);

                GetComponent<PhotonView>().RPC("DamageToEnemy", RpcTarget.All, -maxhealth);
            }
        }
    }
    private void EnemyAI1()
    {
        if(health< maxhealth)
        {
            focusRange = 30f;
        }
        if (walk)
        {

        }
        groundedEnemy = Physics.CheckSphere(SphereforGroundDetection.transform.position, SphereforGroundDetection.transform.localScale.y / 2, ground);

        if (EnemyVelocity.y < 0 && groundedEnemy)
        {
            EnemyVelocity.y = 0f;
        }

        EnemyVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(EnemyVelocity * Time.deltaTime);

        Vector3 direction;
        float angle, targetAngle;

        if (target == null)
        {
            TargetFind();
        }
        else
        { // When it is on a target
            direction = target.position - transform.position;
            generaldirection = direction;

            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg; // turn forward
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            if(isRolling == false)
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

            movement = direction;
            movement.Normalize();

            if(rollcooldown0>0)
            {
                rollcooldown0 -= Time.deltaTime;
            }

            if (direction.magnitude > range) //out of range
            {
                //animator.SetBool("IsAttacking", false);
                /*if(direction.magnitude>)
                {
                    attackSpeed0 = attackSpeed;
                    animator.SetBool("IsAttacking", false);
                }  */
                // attackSpeed0 = attackSpeed;

                if (direction.magnitude > 3.5f && rollcooldown0 <= 0f)
                {
                    // start rolling/dashing
                    if (isRolling == false&&Rolltime0<=0)
                    {
                        startRolling = true;
                        rollcooldown0 = rollcooldown;
                    }
                }


                if (attackSpeed0 < 0.1f)
                {
                    attackSpeed0 = 0;
                    animator.SetBool("IsWalking", true);
                    animator.SetBool("IsAttacking", false);
                    moveEnemy1(movement);

                    hasAttacked = true;
                }
            }
            else // in range
            {
                if (isRolling == false)
                {
                    animator.SetBool("IsWalking", false);

                    if (attackSpeed0 <= 0f)
                    {
                        attackSpeed0 = 0.56f;
                        animator.SetBool("IsAttacking", true);
                        hasAttacked = false;
                    }

                    if (attackSpeed0 < 0.38f && !hasAttacked && animator.GetBool("IsAttacking"))
                    {
                        Attack();
                        hasAttacked = true;
                        animator.SetBool("IsAttacking", false);
                    }

                }
            }
            if (direction.magnitude > 4.5f)
                animator.SetBool("IsAttacking", false);

            if (attackSpeed0 > 0)
            {
                attackSpeed0 -= Time.deltaTime;
            }

            if (direction.magnitude > 14f)  // when to lose target
            {
                if (animator.GetBool("IsWalking"))
                    animator.SetBool("IsWalking", false);
                target = null;

                GetComponent<PhotonView>().RPC("DamageToEnemy", RpcTarget.All, -maxhealth);

            }

        }

        Vector3 startposition;
        //startposition = generaldirection.normalized;
        if (startRolling&&target != null)
        {
            animator.SetBool("IsRolling", true);
            startRolling = false;
            isRolling = true;
            Rolltime0 = Rolltime;
            startposition = (transform.position - target.transform.position).normalized;
            rollingHitBox.SetActive(true);
        }
        else
        {
            startposition = transform.forward;
            //startposition = generaldirection.normalized;
        }
        if (isRolling && animator.GetBool("IsRolling")&&generaldirection.magnitude <=14f) // diarkeia gegonotos, event duration
        {
            controller.Move(startposition * Time.deltaTime*15f);
        }
        if(Rolltime0>0)
        {
            Rolltime0 -= Time.deltaTime;
        }
        else
        {
            isRolling = false;
            animator.SetBool("IsRolling", false);
            rollingHitBox.SetActive(false);
        }


    }
    private void EnemyAI2()
    {
        if(fireballcooldown0 >0f)
        {
            fireballcooldown0 -= Time.deltaTime;
        }
        else
        {
            canfireball = true;
        }

        if(fireballcount0>=fireballcount)
        {
            canfireball = false;
            fireballcount0 = 0;
            fireballcooldown0 = fireballcooldown;
            lasercount0 = 0;
        }

        if(lasercooldown0 >0f)
        {
            lasercooldown0 -= Time.deltaTime;
        }
        if (health < maxhealth)
        {
            focusRange = 30f;
        }
        groundedEnemy = Physics.CheckSphere(SphereforGroundDetection.transform.position, SphereforGroundDetection.transform.localScale.y / 2, ground);

        if (EnemyVelocity.y < 0 && groundedEnemy)
        {
            EnemyVelocity.y = 0f;
        }

        EnemyVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(EnemyVelocity * Time.deltaTime);


        Vector3 direction;
        float angle, targetAngle;

        if (target == null)
        {
            TargetFind();
        }
        else
        { // When it is on a target

            direction = target.position - transform.position;
            generaldirection = direction;


            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg; // turn forward
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            movement = direction;
            movement.Normalize();



            if (direction.magnitude > 10f)
            {// out of range ... walk
                animator.SetBool("FlameAttack", false);
                if (rangedflameattack0 < 0.4f)
                {
                    rangedflameattack0 = 0;
                    animator.SetBool("IsWalking", true);
                    moveEnemy(movement);
                }
            }
            else
            {// ranged attack
                if (canfireball && fireball)
                {
                    animator.SetBool("IsWalking", false);

                    if (rangedflameattack0 <= 0f)
                    {
                        rangedflameattack0 = rangedflameattack;
                        animator.SetBool("FlameAttack", true);
                        hasrangedattack = false;
                        hasmuzzled = false;
                    }
                    if (rangedflameattack0 < 0.9f && !hasmuzzled)
                    {
                        //server.FireMuzzleVFX(transform.position+ new Vector3(-0.5f, 1f, 0f), transform.rotation);
                        server.FireMuzzle1VFX(rollingHitBox.transform.position, transform.rotation);

                        hasmuzzled = true;
                    }

                    if (rangedflameattack0 < 0.8f && !hasrangedattack && animator.GetBool("FlameAttack"))
                    {
                        Vector3 FlameThrowerPoint = rollingHitBox.transform.position;
                        //Quaternion finalRotation = transform.rotation;
                        Quaternion finalRotation = transform.rotation;
                        finalRotation = Quaternion.LookRotation(new Vector3(0, -1f, 0) + (target.position - transform.position), new Vector3(0, 1, 0));
                        //Vector3 FlameThrownerPoint = L
                        //finalRotation = Quaternion.LookRotation(target.transform.position);
                        //finalRotation = finalRotation * Quaternion.Euler(new Vector3(-1, 0 -1));
                        int playerID = -1;
                        int Ab0Damage = 50;
                        object[] content = new object[] { FlameThrowerPoint, finalRotation, playerID, Ab0Damage };
                        //server.Ability00Cast(content);
                        server.EnemyFlame00Cast(content);
                        fireballcount0++;

                        hasrangedattack = true;
                    }
                }
                else if (fireballcooldown0>3f&& lasercount0<lasercount)
                {
                    Vector3 firePosition;
                    Quaternion fireRotation;
                    if(lasercooldown0 <=0.8f&&!laserwarning)
                    {
                        laserStart = rollingHitBox.transform.position;
                        laserDirection = target.position - rollingHitBox.transform.position;
                        animator.SetBool("IsWalking", false);
                        animator.SetBool("FlameAttack", true);
                        server.WarningAreaHorVFX(laserStart, Quaternion.LookRotation(laserDirection));
                        laserwarning = true;
                        
                        //server.WarningAreaVFX(target.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        //animator.SetBool("FlameAttack", false);
                    }
                    if(lasercooldown0<=0)
                    {
                        Vector3 position1, position2;
                        position1 = laserStart;
                        position2 = laserDirection;
                        object[] content1 = new object[] { position1, position2 };
                        photonView.RPC("MakeLaserBeam", RpcTarget.All, content1 as object);
                        //MakeLaserBeam();
                        object[] content = new object[] { laserStart, Quaternion.LookRotation(laserDirection), -1, 40};

                        server.EnemyLaserCast(content);
                        StartCoroutine(TempDes(1f));

                        lasercooldown0 = lasercooldown;
                        laserwarning = false;
                        lasercount0++;
                    }
                }
                else
                {
                    rangedflameattack0 = 0;
                    lasercooldown0 = 2f;
                    animator.SetBool("FlameAttack", false);

                    if(direction.magnitude>1.5f)
                    {
                        animator.SetBool("IsWalking", true);
                        moveEnemy(movement);
                    }
                    else
                    {
                        animator.SetBool("IsWalking", false);
                    }
                }

            }
            //if(Input.GetKeyDown(KeyCode.I))
                //server.FireMuzzleVFX(FlameThrowerPoint, transform.rotation);

            if (rangedflameattack0 > 0)
            {
                rangedflameattack0 -= Time.deltaTime;
            }



           /* if (direction.magnitude > range) //out of range
            {
                if (attackSpeed0 < 0.4f)
                {
                    attackSpeed0 = 0;
                    animator.SetBool("IsWalking", true);
                    moveEnemy(movement);
                }
            }
            else // in range
            {
                animator.SetBool("IsWalking", false);

                if (attackSpeed0 <= 0f)
                {
                    attackSpeed0 = attackSpeed;
                    animator.SetBool("IsAttacking", true);
                    hasAttacked = false;
                }

                if (attackSpeed0 < 0.8f && !hasAttacked && animator.GetBool("IsAttacking"))
                {
                    Attack();
                    hasAttacked = true;
                }
            }
            if (direction.magnitude > 4.5f)
                animator.SetBool("IsAttacking", false);

            if (attackSpeed0 > 0)
            {
                attackSpeed0 -= Time.deltaTime;
            }*/

            if (direction.magnitude > 14)  // when to lose target
            {
                if (animator.GetBool("IsWalking"))
                    animator.SetBool("IsWalking", false);
                target = null;
                focusRange = 10f;

                canvas.SetActive(false);
                GetComponent<PhotonView>().RPC("DamageToEnemy", RpcTarget.All, -maxhealth);

            }
            //Debug.Log(direction.magnitude);
        }
    }
    private void EnemyAI3()
    {
        if (fireballcooldown0 > 0f)
        {
            fireballcooldown0 -= Time.deltaTime;
        }
        else
        {
            canfireball = true;
        }

        if (fireballcount0 >= fireballcount)
        {
            canfireball = false;
            fireballcount0 = 0;
            fireballcooldown0 = fireballcooldown;
            lasercount0 = 0;
        }

        if (health < maxhealth)
        {
            focusRange = 30f;
        }
        groundedEnemy = Physics.CheckSphere(SphereforGroundDetection.transform.position, SphereforGroundDetection.transform.localScale.y / 2, ground);

        if (EnemyVelocity.y < 0 && groundedEnemy)
        {
            EnemyVelocity.y = 0f;
        }

        EnemyVelocity.y += gravityValue*30f * Time.deltaTime;
        controller.Move(EnemyVelocity * Time.deltaTime);



        Vector3 direction;
        float angle, targetAngle;

        

        if (target == null)
        {
            TargetFind();

            if(struttime0 >0f)
            {
                moveEnemy(transform.forward);
                struttime0 -= Time.deltaTime;

            }
            else
            {
                transform.rotation *= Quaternion.Euler(0, 180, 0);
                struttime0 = struttime;
            }
        }
        else
        { // When it is on a target

            direction = target.position - transform.position;
            generaldirection = direction;


            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg; // turn forward
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            movement = direction;
            movement.Normalize();



            if (direction.magnitude > 8f)
            {// out of range ... walk
                animator.SetBool("IsAttacking", false);
                if (rangedflameattack0 < 0.4f)
                {
                    rangedflameattack0 = 0;
                    animator.SetBool("IsWalking", true);
                    moveEnemy(movement);
                }
            }
            else
            {// ranged attack
                if (fireball)
                {
                    animator.SetBool("IsWalking", false);

                    if (rangedflameattack0 <= 0f)
                    {
                        rangedflameattack0 = rangedflameattack;
                        animator.SetBool("IsAttacking", true);
                        hasrangedattack = false;
                        hasmuzzled = false;
                    }
                    if (rangedflameattack0 < 0.9f && !hasmuzzled)
                    {
                        //server.FireMuzzleVFX(transform.position+ new Vector3(-0.5f, 1f, 0f), transform.rotation);
                        server.FireMuzzle1VFX(rollingHitBox.transform.position, transform.rotation);

                        hasmuzzled = true;
                    }

                    if (rangedflameattack0 < 0.8f && !hasrangedattack && animator.GetBool("IsAttacking"))
                    {
                        Vector3 FlameThrowerPoint = rollingHitBox.transform.position;
                        //Quaternion finalRotation = transform.rotation;
                        Quaternion finalRotation = transform.rotation;
                        //finalRotation = Quaternion.LookRotation(new Vector3(0, -1f, 0) + (target.position - transform.position), new Vector3(0, 1, 0));
                        //Vector3 FlameThrownerPoint = L
                        //finalRotation = Quaternion.LookRotation(target.transform.position);
                        //finalRotation = finalRotation * Quaternion.Euler(new Vector3(-1, 0 -1));
                        int playerID = -1;
                        int Ab0Damage = 50;
                        object[] content = new object[] { FlameThrowerPoint, finalRotation, playerID, Ab0Damage };
                        //server.Ability00Cast(content);
                        server.EnemyCannonBall(content);
                        fireballcount0++;

                        hasrangedattack = true;
                    }

                    if(rangedflameattack0 >0f && rangedflameattack0 <0.1f)
                        animator.SetBool("IsAttacking", false);
                        
                }
                else
                {
                    rangedflameattack0 = 0;
                    lasercooldown0 = 2f;
                    animator.SetBool("IsAttacking", false);

                    if (direction.magnitude > 1.5f)
                    {
                        animator.SetBool("IsWalking", true);
                        moveEnemy(movement);
                    }
                    else
                    {
                        animator.SetBool("IsWalking", false);
                    }
                }

            }
            

            if (rangedflameattack0 > 0)
            {
                rangedflameattack0 -= Time.deltaTime;
            }


            if (direction.magnitude > 16)  // when to lose target
            {
                if (animator.GetBool("IsWalking"))
                    animator.SetBool("IsWalking", false);
                target = null;
                focusRange = 13f;
                canvas.SetActive(false);

                GetComponent<PhotonView>().RPC("DamageToEnemy", RpcTarget.All, -maxhealth);

            }
            //Debug.Log(direction.magnitude);
        }
    }
        IEnumerator TempDes(float delay)
        {
            yield return new WaitForSeconds(delay);

            Destroy(beamStart);
            Destroy(beamEnd);
            Destroy(beam);
        }
    [PunRPC]
    void MakeLaserBeam(object[] content)
    {
        Vector3 place1 = (Vector3)content[0];
        Vector3 place2 = (Vector3)content[1];
        beamStart = Instantiate(beamStartPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        beamEnd = Instantiate(beamEndPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        beam = Instantiate(beamLineRendererPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        line = beam.GetComponent<LineRenderer>();

        Ray ray = new Ray(place1, place2);
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, ground))
        {
            //Debug.Log(hit);
            //server.EnemyLaserHitVFX(hit.point, Quaternion.identity);
            Vector3 tdir = hit.point - transform.position;
            ShootBeamInDir(place1, place2);
        }
        StartCoroutine(TempDes(1f));
    }
    void TargetFind()
    {
        Collider[] hitPlayers = Physics.OverlapSphere(transform.position, focusRange, players);
        foreach (Collider player in hitPlayers)
        {
            if (player.gameObject.layer == 9) // Player entities
            {
                //if (playerID != enemy.gameObject.GetComponent<player3>().playerID)
                    target = player.transform;
            }
            else   // propably Environment Enemies
            {
                //attackTarget = enemy.transform;
            }
        }
    }
    void ShowCanvas()
    {
        Collider[] hitPlayers = Physics.OverlapSphere(transform.position, 10f, players);
        foreach (Collider player in hitPlayers)
        {
            if (player.gameObject.layer == 9 && player.gameObject.GetComponent<player3>().photonView.IsMine) // Player entities
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);
               // Debug.Log("player in range: " + distance.ToString());
                //distance
                if (distance < 10f || (health < maxhealth && !isDead))
                {
                    canvas.SetActive(true);
                }
                else
                    canvas.SetActive(false);
            }
        }
    }
    void moveEnemy(Vector3 direction)
    {
        transform.position = transform.position + direction * moveSpeed * Time.deltaTime;
    }
    void moveEnemy1(Vector3 direction)
    {
        controller.Move(direction * moveSpeed * Time.deltaTime);
    }
    [PunRPC]
    void Attack()
    {
        Collider[] hitPlayers = Physics.OverlapBox(BasicAttackHitBox.position, BasicAttackHitBox.localScale / 2, BasicAttackHitBox.rotation, players);
        foreach (Collider player in hitPlayers)
        {
            //enemy.GetComponent<Health>().photonView.RPC("TakeDamage", RpcTarget.All);   // 9: player , 10: enemies
            if (player.gameObject.layer == 9) // Players
            {
                player.GetComponent<NetworkPlayer3>().photonView.RPC("Damage", RpcTarget.AllBuffered, 30);
                server.GetComponent<ServerSide>().BasicAttack0HitVFX(player.transform);
               // Debug.Log(player);
            }
        }
    }
    public void CrewAct(float radius)
    {
        LayerMask enemies = 10;
        Collider[] hitCrew = Physics.OverlapSphere(transform.position, radius);
        foreach(Collider enemy in hitCrew)
        {
            if(enemy.gameObject.GetComponent<Enemy3>()!=null)
                enemy.gameObject.GetComponent<Enemy3>().Wake();
        }
    }
    public void Wake()
    {
        focusRange = 40;
        isWalken = true;
        if (canvas.activeSelf == false)
            canvas.SetActive(true);

    }
    public void DyingFade()
    {
        mesh.GetComponent<SkinnedMeshRenderer>().material.color = new Color(1f, 1f, 1f, 0f);
    }
    IEnumerator DeathCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        PhotonNetwork.Destroy(me);
    }
    
    void ShootBeamInDir(Vector3 start, Vector3 dir)
    {
        line.positionCount = 2;
        line.SetPosition(0, start);
        beamStart.transform.position = start;

        Vector3 end = Vector3.zero;
        RaycastHit hit;
        if (Physics.Raycast(start, dir, out hit, ground))
            end = hit.point - (dir.normalized * beamEndOffset);
        else
            end = transform.position + (dir * 100);

        server.EnemyLaserHitVFX(hit.point, Quaternion.identity);
        beamEnd.transform.position = end;
        line.SetPosition(1, end);

        beamStart.transform.LookAt(beamEnd.transform.position);
        beamEnd.transform.LookAt(beamStart.transform.position);

        float distance = Vector3.Distance(start, end);
        line.sharedMaterial.mainTextureScale = new Vector2(distance / textureLengthScale, 1);
        line.sharedMaterial.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0);
    }

}
[SerializeField]
public class Enemy
{
    public int type;
    public float attackspeed;
    public float focusrange;
    public float attackrange;

    public Enemy(int type, float attackspeed, float focusrange, float attackrange)
    {
        this.type = type;
        this.attackspeed = attackspeed;
        this.focusrange = focusrange;
        this.attackrange = attackrange;
    }

    public int getType()
    {
        return type;
    }

    public float getAttackSpeed()
    {
        return attackspeed;
    }

    public float getFocusRange()
    {
        return focusrange;
    }

    public float getAttackrange()
    {
        return attackrange;
    }

    public string toString()
    {
        return "Type: " + this.getType().ToString() + "  attackspeed: " + this.getAttackSpeed().ToString() + "  focusRange: " + this.getFocusRange().ToString() + "  attackrange: " + this.getAttackrange().ToString();
    }

    
}
