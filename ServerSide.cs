using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ServerSide : MonoBehaviourPun
{
    [SerializeField] private GameObject skeletonSpawnPoint;
    [SerializeField] private GameObject skeletonSpawnPoint1;
    [SerializeField] private GameObject skeletonSpawnPoint1A;
    [SerializeField] private GameObject skeletonSpawnPoint2;
    [SerializeField] private GameObject ballspawnpoint1;
    [SerializeField] private GameObject cannonspawnpoint1;
    [SerializeField] private GameObject multspawnpoint1;
    [SerializeField] private GameObject multspawnpoint2;
    [SerializeField] private GameObject multspawnpoint3;


    private bool ismasterclient;
    private GameObject clone;
    public GameObject fireball01Hitbox;
    public GameObject Prick1HitBox;
    public GameObject SlashWaveHitBox;
    public GameObject RangedMagicHithitBox;
    public GameObject EarthShattererHitBox;
    public GameObject EnemyLaserHitBox;
    Transform place1;
    player3 player3MasterClient;
    GameObject player;

    private GameObject[] EnemiesPlaying = new GameObject[100];
    private int enemiesplayingpointer = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Vector3 StartSpawnPosition = new Vector3(3, 0.85f, 1);
            SpawnEnemy(StartSpawnPosition, 0);
            SpawnEnemy(StartSpawnPosition + new Vector3(2, 0, 2), 1);
            SpawnEnemy(StartSpawnPosition + new Vector3(4, 0, 2), 3);
            SpawnEnemy(StartSpawnPosition + new Vector3(2, 0, -17), 2);


            for (int i = 0; i < 4; i++)
            {
                SpawnEnemy(CloseSpawnPosition(skeletonSpawnPoint.transform.position), 1);
            }
            SpawnEnemy(CloseSpawnPosition(skeletonSpawnPoint1.transform.position), 1);
            SpawnEnemy(CloseSpawnPosition(skeletonSpawnPoint1.transform.position), 1);

            SpawnEnemy(CloseSpawnPosition(skeletonSpawnPoint1A.transform.position), 1);

            SpawnEnemy(CloseSpawnPosition(skeletonSpawnPoint2.transform.position), 1);
            SpawnEnemy(CloseSpawnPosition(skeletonSpawnPoint2.transform.position), 1);

            for (int i=0; i<1; i++)
            {
                SpawnEnemy(CloseSpawnPosition(ballspawnpoint1.transform.position), 0);
            }
            for(int i=0; i<2; i++)
            {
                SpawnEnemy(CloseSpawnPosition(cannonspawnpoint1.transform.position), 4);
            }

            SpawnEnemy(CloseSpawnPosition(multspawnpoint1.transform.position), 1);
            SpawnEnemy(CloseSpawnPosition(multspawnpoint1.transform.position), 4);
            SpawnEnemy(CloseSpawnPosition(multspawnpoint1.transform.position), 4);

            SpawnEnemy(CloseSpawnPosition(multspawnpoint2.transform.position), 3);
            SpawnEnemy(CloseSpawnPosition(multspawnpoint2.transform.position), 4);
            SpawnEnemy(CloseSpawnPosition(multspawnpoint2.transform.position), 4);

            SpawnEnemy(CloseSpawnPosition(multspawnpoint3.transform.position), 0);
            SpawnEnemy(CloseSpawnPosition(multspawnpoint3.transform.position), 0);
        }
    }
    Vector3 CloseSpawnPosition(Vector3 spawnPosition)
    {
        Vector3 CloseSpawnPosition = new Vector3(Random.Range(0, 3.5f), 0f, Random.Range(0, 3.5f));
        return spawnPosition + CloseSpawnPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ismasterclient = true;

            //if(EnemiesPlaying[0]==null)
            //{
            //    enemiesplayingpointer = 0;
            //    Vector3 RandomPositionNear = new Vector3(Random.Range(-15, 5), 4f, Random.Range(-15, 5));
            //    SpawnEnemy(RandomPositionNear, 0);
            //}

            //if (EnemiesPlaying[1] == null)
            //{
            //    enemiesplayingpointer = 1;
            //    Vector3 RandomPositionNear = new Vector3(Random.Range(-15, 5), 4f, Random.Range(-15, 5));
            //    SpawnEnemy(RandomPositionNear, 1);
            //}
            if(EnemiesPlaying[2]== null)
            { 
                enemiesplayingpointer = 2;
                Vector3 RandomPositionNear = new Vector3(Random.Range(-10, 5), 4f, Random.Range(-15, 5));
                SpawnEnemy(RandomPositionNear, 3);
            }
            //if(EnemiesPlaying[3]==null)
            //    enemiesplayingpointer = 3;
            //    Vector3 RandomPositionNear = new Vector3(Random.Range(-10, 5), 4f, Random.Range(-15, 5));
            //    SpawnEnemy(RandomPositionNear, 2);
            //}
        }
        else
        {
            ismasterclient = false;
        }

        
    }
    public void FireBall01HitVFX(Transform place)
    {
        PhotonNetwork.Instantiate("Fireball01Hit", place.position+new Vector3(0, -0.5f, 0), place.rotation* Quaternion.Euler(new Vector3(-1, 1, -1)));
        //PhotonNetwork.Instantiate("HitFireball02", place.position+new Vector3(0, -0.5f, 0), place.rotation* Quaternion.Euler(new Vector3(-1, 1, -1)));
    }
    public void FireBall02HitVFX(Transform place)
    {
        //PhotonNetwork.Instantiate("Fireball01Hit", place.position+new Vector3(0, -0.5f, 0), place.rotation* Quaternion.Euler(new Vector3(-1, 1, -1)));
        PhotonNetwork.Instantiate("HitFireball02", place.position + new Vector3(0, -0.5f, 0), place.rotation * Quaternion.Euler(new Vector3(-1, 1, -1)));
    }
    public void BasicAttackHitVFX(Transform place)
    {
        PhotonNetwork.Instantiate("Fireball01BlueHitBAORANGE", place.position + new Vector3(0, -0.5f, 0), place.rotation * Quaternion.Euler(new Vector3(-1, 1, -1)));
    }
    public void BasicAttack0HitVFX(Transform place)
    {
        PhotonNetwork.Instantiate("Fireball01BlueHitGREYWHITE", place.position + new Vector3(0, -0.5f, 0), place.rotation * Quaternion.Euler(new Vector3(-1, 1, -1)));
    }

    public void BlueHitVFX(Transform place)
    {
        PhotonNetwork.Instantiate("GotHitBlue", place.position + new Vector3(0, 0f, 0), place.rotation * Quaternion.Euler(new Vector3(-1, 1, -1)));
    }
    public void BLueHitVFXLarge(Transform place, Vector3 position)
    {
        PhotonNetwork.Instantiate("GotHitBlue", position + new Vector3(0, 0f, 0f), place.rotation * Quaternion.Euler(new Vector3(-1, 1, -1)));
    }

    public void FireMuzzleVFX(Vector3 place, Quaternion rotation)
    {
        PhotonNetwork.Instantiate("MuzzleBulletOrange", place, rotation);
    }
    public void FireMuzzle1VFX(Vector3 place, Quaternion rotation)
    {
        PhotonNetwork.Instantiate("MuzzleFireball01", place, rotation);
    }
    public void WarningAreaVFX(Vector3 place, Quaternion rotation)
    {
        PhotonNetwork.Instantiate("warning area", place, rotation);
    }
    public void WarningAreaHorVFX(Vector3 place, Quaternion rotation)
    {
        PhotonNetwork.Instantiate("WarningAreaHor", place, rotation);
    }
    public void EnemyLaserHitVFX(Vector3 place, Quaternion rotation)
    {
        PhotonNetwork.Instantiate("EnemyFireBeamImpact0", place, rotation);
    }
    [PunRPC]
    public void Ability01Cast(object[] content)
    {
        Vector3 place1 = (Vector3)content[0];
        Quaternion place2 = (Quaternion)content[1];
        int myplayerID = (int)content[2];
        int mydamage = (int)content[3];
        Debug.Log(myplayerID);
        if(PhotonNetwork.IsMasterClient)
        {
            GameObject prickHitBox;
            PhotonNetwork.Instantiate("Prick (1)", place1, place2);

            prickHitBox = Instantiate(Prick1HitBox, place1, place2 * Quaternion.Euler(new Vector3(90, 0, 0)));
            //prickHitBox = Instantiate(Prick1HitBox, place1, place2);
            prickHitBox.GetComponent<ProjectileBasicLocal>().PlayerIDSet(myplayerID);
            prickHitBox.GetComponent<ProjectileBasicLocal>().DamageSet(mydamage);
        }
    }
    [PunRPC]
    public void Ability02Cast(object[] content)
    {
        Vector3 place1 = (Vector3)content[0];
        Quaternion place2 = (Quaternion)content[1];
        int myplayerID = (int)content[2];
        int mydamage = (int)content[3];
        
        if(PhotonNetwork.IsMasterClient)
        {
            GameObject slashWaveHitBox;
            PhotonNetwork.Instantiate("SlashWaveBlue", place1 + new Vector3(0, -0.6f, 0), place2);

            slashWaveHitBox = Instantiate(SlashWaveHitBox, place1 + new Vector3(0, -0.6f, 0), place2);
            slashWaveHitBox.GetComponent<ProjectileBasicLocal>().PlayerIDSet(myplayerID);
            slashWaveHitBox.GetComponent<ProjectileBasicLocal>().DamageSet(mydamage);
        }
    }
    [PunRPC]
    public void Ability03Cast(object[] content)
    {
        Vector3 place1 = (Vector3)content[0];
        Quaternion place2 = (Quaternion)content[1];
        int myplayerID = (int)content[2];
        int mydamage = (int)content[3];
        if(PhotonNetwork.IsMasterClient)
        {
            GameObject earthShattererHitBox;
            PhotonNetwork.Instantiate("EarthShatterer02", place1, place2);

            earthShattererHitBox = Instantiate(EarthShattererHitBox, place1, place2);
            earthShattererHitBox.GetComponent<ProjectileBasicLocal>().PlayerIDSet(myplayerID);
            earthShattererHitBox.GetComponent<ProjectileBasicLocal>().DamageSet(mydamage);
        }
    }
    [PunRPC]
    public void Ability00Cast(object[] content)
    {
        Vector3 place1 = (Vector3)content[0];
        Quaternion place2 = (Quaternion)content[1];
        int myplayerID = (int)content[2];
        int mydamage = (int)content[3];
        if(PhotonNetwork.IsMasterClient)
        {
            GameObject FireballHitbox;
            GameObject FireballHitbox2;
            //FireballHitbox2 = PhotonNetwork.Instantiate("Fireball01", place1, place2);
            FireballHitbox2 = PhotonNetwork.Instantiate("Fireball02", place1, place2);


            FireballHitbox = Instantiate(fireball01Hitbox, place1, place2);
            FireballHitbox.GetComponent<ProjectileBasicLocal>().PlayerIDSet(myplayerID);
            FireballHitbox.GetComponent<ProjectileBasicLocal>().DamageSet(mydamage);

            FireballHitbox2.GetComponent<sceneVFX>().scriptSetter(FireballHitbox);
        }
    }
    public void EnemyFlame00Cast(object[] content)
    {
        Vector3 place1 = (Vector3)content[0];
        Quaternion place2 = (Quaternion)content[1];
        int myplayerID = (int)content[2];
        int mydamage = (int)content[3];
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject FireballHitbox;
            GameObject FireballHitbox2;
            //FireballHitbox2 = PhotonNetwork.Instantiate("Fireball01", place1, place2);
            FireballHitbox2 = PhotonNetwork.Instantiate("Fireball02", place1, place2);

            FireballHitbox = Instantiate(fireball01Hitbox, place1, place2);
            FireballHitbox.GetComponent<ProjectileBasicLocal>().enemySkillshot = true;
            FireballHitbox.GetComponent<ProjectileBasicLocal>().PlayerIDSet(myplayerID);
            FireballHitbox.GetComponent<ProjectileBasicLocal>().DamageSet(mydamage);

            FireballHitbox2.GetComponent<sceneVFX>().scriptSetter(FireballHitbox);
        }
    }
    public void EnemyCannonBall(object[] content)
    {
        Vector3 place1 = (Vector3)content[0];
        Quaternion place2 = (Quaternion)content[1];
        int myplayerID = (int)content[2];
        int mydamage = (int)content[3];
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject FireballHitbox;
            GameObject FireballHitbox2;
            FireballHitbox2 = PhotonNetwork.Instantiate("CannonBall", place1, place2);

            FireballHitbox = Instantiate(fireball01Hitbox, place1, place2);
            FireballHitbox.GetComponent<ProjectileBasicLocal>().enemySkillshot = true;
            FireballHitbox.GetComponent<ProjectileBasicLocal>().PlayerIDSet(myplayerID);
            FireballHitbox.GetComponent<ProjectileBasicLocal>().DamageSet(mydamage);

            FireballHitbox2.GetComponent<sceneVFX>().scriptSetter(FireballHitbox);
        }

    }
    public void EnemyLaserCast(object[] content)
    {
        Vector3 place1 = (Vector3)content[0];
        Quaternion place2 = (Quaternion)content[1];
        int myplayerID = (int)content[2];
        int mydamage = (int)content[3];
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject EnemyLaserHitBox1;
            GameObject EnemyLaserHitBox2;
            // EnemyLaserHitBox2 = PhotonNetwork.Instantiate

            EnemyLaserHitBox1 = Instantiate(EnemyLaserHitBox, place1, place2);
            EnemyLaserHitBox1.GetComponentInChildren<ProjectileBasicLocal>().enemySkillshot = true;
            EnemyLaserHitBox1.GetComponentInChildren<ProjectileBasicLocal>().PlayerIDSet(myplayerID);
            EnemyLaserHitBox1.GetComponentInChildren<ProjectileBasicLocal>().DamageSet(mydamage);

            //EnemyLaserHitBox2//.GetComponent<sceneVFX>().scriptSetter(FireballHitbox);
        }
    }
    [PunRPC]
    public void RangedBasicAttack1Cast(object[] content)
    {
        Vector3 place1 = (Vector3)content[0];
        Quaternion place2 = (Quaternion)content[1];
        int myplayerID = (int)content[2];
        int mydamage = (int)content[3];

        if(PhotonNetwork.IsMasterClient)
        {
            GameObject RangedMageHit;
            GameObject RangedMageHit2;
            RangedMageHit2 = PhotonNetwork.Instantiate("RangedBasicAttack", place1, place2);
            
            RangedMageHit = Instantiate(RangedMagicHithitBox, place1, place2);
            RangedMageHit.GetComponent<ProjectileBasicLocal>().PlayerIDSet(myplayerID);
            RangedMageHit.GetComponent<ProjectileBasicLocal>().DamageSet(mydamage);

            RangedMageHit2.GetComponent<sceneVFX>().scriptSetter(RangedMageHit);
        }
    }

    [PunRPC]
    public void EnemyDied(int UnitID)
    {
        Debug.Log("Message via server: Number "+UnitID+" Died");
        EnemiesPlaying[UnitID] = null;
        Debug.Log(EnemiesPlaying[UnitID]);
    }
    public void SpawnEnemy(Vector3 spawnPosition, int enemytype)
    {
        //var newEnemy = PhotonNetwork.InstantiateRoomObject("Enemies/EnemyDummy1", spawnPosition, Quaternion.identity);
        GameObject newEnemy;
        if(enemytype ==0)
        {
            newEnemy = PhotonNetwork.InstantiateRoomObject("Enemies/EnemyBall1", spawnPosition, Quaternion.identity);
        }
        else if( enemytype == 1)
        {
            newEnemy = PhotonNetwork.InstantiateRoomObject("Enemies/EnemyDummy1", spawnPosition, Quaternion.Euler(0f, Random.Range(0, 110), 0f));
        }
        else if(enemytype == 3)
        {
            newEnemy = PhotonNetwork.InstantiateRoomObject("Enemies/FlameGuy1", spawnPosition, Quaternion.identity);
        }
        else
        {
            newEnemy = PhotonNetwork.InstantiateRoomObject("Enemies/CannonEnemy1", spawnPosition, Quaternion.identity);
        }
        newEnemy.GetComponent<Enemy3>().UnitID = enemiesplayingpointer;
        EnemiesPlaying[enemiesplayingpointer] = newEnemy;

        Debug.Log("server: "+ EnemiesPlaying[enemiesplayingpointer]);
        if (enemiesplayingpointer < 100)
            enemiesplayingpointer++;
        else
            enemiesplayingpointer = 0;
    }
}
