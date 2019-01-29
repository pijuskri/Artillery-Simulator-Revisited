using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveShot : MonoBehaviour
{
    #region Editor references
    public GameObject player;
    public GameObject shot;
    public GameObject flash;
    public Rigidbody2D shotbody;
    public GameObject target;
    public Animator anim;
    public GameObject BloodPile;
    public GameObject Hole;
    public GameLogic gameLogic;
    public AudioSource boom;
    GameObject decoy;
    public AudioClip explosionSound;
    #endregion

    #region Game variables
    public float baseSpeed = 10f;
    public bool targetHit=false;
    float blowPower=1;
    public bool atBase=true;
    float waitAnimation=0;
    private Vector3 currentTarget;
    public bool hasShot;
    private float volLowRange = .5f;
    private float volHighRange = 1.0f;
    #endregion

    // Use this for initialization
    void Start () {
        shotbody = gameObject.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boom = GetComponent<AudioSource>();
        baseSpeed = 5f;
    }
	
	// Update is called once per frame
	void Update () {
        waitAnimation -= Time.deltaTime;
        if (waitAnimation < 0.95) flash.SetActive(false);
        if (waitAnimation < 0.9) anim.SetBool("targetHit", false);
    }
    void FixedUpdate()
    {
        if (targetHit == false && hasShot == true)
        {
            //shotbody.AddForce(transform.up * speedX);
            //shotbody.AddForce(transform.right * speedY * -1);
            transform.position = Vector3.MoveTowards(transform.position, currentTarget, baseSpeed * Time.deltaTime);
            atBase = false;
        }
        if (Vector3.Distance(transform.position, currentTarget)<0.5f)
        {
            targetHit = true;
        }
        if (targetHit == true && atBase == false)
        {
            flash.SetActive(true);
            anim.SetBool("targetHit", true);
            float vol = Random.Range(volLowRange, volHighRange);
            boom.PlayOneShot(explosionSound, vol);
            waitAnimation = 1;
            //shotbody.AddForce(shotbody.velocity * -10);
            hasShot = false;

        }
        if(shotbody.velocity.x==0 && shotbody.velocity.y == 0 && targetHit && !atBase)
        {
            targetHit = false;
            Quaternion shotRotation = new Quaternion(0, 0, 90, 0);
            Vector3 shotPosition = new Vector3(shot.transform.position.x, shot.transform.position.y, 0);
            GameObject ExplosionHole = Instantiate(Hole, shotPosition, shotRotation);
            
            GameObject[] enemyList;
            enemyList = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < enemyList.Length; i++)
            {
                float blowDistance = Vector3.Distance(enemyList[i].transform.position, transform.position);
                
                if (blowDistance < 4)
                {
                    Destroy(enemyList[i]);
                    Quaternion enemyRotation = new Quaternion(0, 0, 90, 0);
                    Vector3 enemyPosition = new Vector3(enemyList[i].transform.position.x, enemyList[i].transform.position.y, 0);
                    GameObject enemyBlood = Instantiate(BloodPile, enemyPosition, enemyRotation);
                    gameLogic.kills++;
                    gameLogic.killsForGold++;
                }
                else if (blowDistance < 5)
                {
                    Rigidbody2D blowRigid = enemyList[i].GetComponent<Rigidbody2D>();
                    //Vector2 toVector = enemyList[i].transform.position - transform.position;
                    //float angleToTarget = Vector2.Angle(transform.up, toVector);
                    blowRigid.AddForce(enemyList[i].transform.up * -1 * blowPower);
                }
            }
            enemyList = GameObject.FindGameObjectsWithTag("Friendly");
            for (int i = 0; i < enemyList.Length; i++)
            {
                float blowDistance = Vector3.Distance(enemyList[i].transform.position, transform.position);
                if (blowDistance < 4)
                {
                    Destroy(enemyList[i]);
                    Quaternion enemyRotation = new Quaternion(0, 0, 90, 0);
                    Vector3 enemyPosition = new Vector3(enemyList[i].transform.position.x, enemyList[i].transform.position.y, 0);
                    GameObject enemyBlood = Instantiate(BloodPile, enemyPosition, enemyRotation);
                    gameLogic.friendlyKills++;
                }
                else if (blowDistance < 5)
                {
                    Rigidbody2D blowRigid = enemyList[i].GetComponent<Rigidbody2D>();
                    //Vector2 toVector = enemyList[i].transform.position - transform.position;
                    //float angleToTarget = Vector2.Angle(transform.up, toVector);
                    blowRigid.AddForce(enemyList[i].transform.up * -1 * blowPower);
                }
            }
            shot.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0.1f);
            atBase = true;
        }
        
    }
    public void targetSet()
    {
        gameLogic.currentAmmo--;
        currentTarget = target.transform.position;
        LookAtTarget();
    }
    public void LookAtTarget()
    {
        var dir = currentTarget - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }
   
}

