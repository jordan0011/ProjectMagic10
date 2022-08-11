using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLibrary : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void TestFuction()
    {
        Debug.Log("enemy library is active");
    }
    [SerializeField]
    public class Enemy
    {
        public int type;
        public int attackspeed;
        public int focusrange;
        public int attackrange;

        public Enemy(int type, int attackspeed, int focusrange, int attackrange)
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
        
        public int getAttackSpeed()
        {
            return attackspeed;
        }

        public int getFocusRange()
        {
            return focusrange;
        }

        public int getAttackrange()
        {
            return attackrange;
        }

        public string toString()
        {
            return "Type: " + this.getType().ToString() + "  attackspeed: " + this.getAttackSpeed().ToString() + "  focusRange: " + this.getFocusRange().ToString() + " attackrange: " + this.getAttackrange().ToString();
        }
    }
    //public Enemy myEnemy = new Enemy();
    

}
