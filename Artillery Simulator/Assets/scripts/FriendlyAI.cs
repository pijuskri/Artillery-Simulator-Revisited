using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyAI : MonoBehaviour
{
    public Rigidbody2D rig;
    public Shoot instance;
    public GameObject player;
    public GameObject BloodPile;
    public GameLogic gameLogic;
    public GameObject gameLogicObject;

    float direction = 1;
    float maxSpeed = 5;
    float dangerTime = 3;
    private float baseSpeed = 4;
    public float chanceToIgnoreShot = 1.00f;
    // Use this for initialization
    void Start()
    {
        gameLogicObject = GameObject.FindGameObjectWithTag("GameLogic");
        gameLogic = gameLogicObject.GetComponent<GameLogic>();
        rig = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        instance = player.GetComponent<Shoot>();
    }
    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Barbed wire")
        {
            rig.AddForce(transform.up * rig.velocity.x * baseSpeed * -10);
        }
        else if (coll.gameObject.tag == "EnemyTrench")
        {
            gameLogic.kills++;
        }
    }
    void Update()
    {
        dangerTime += Time.deltaTime;
        chanceToIgnoreShot = 1 - (gameLogic.friendlyKills * 0.2f);
    }
    void FixedUpdate()
    {
        if (transform.position.x > 19.4f)
        {
            Quaternion enemyRotation = new Quaternion(0, 0, 90, 0);
            Vector3 enemyPosition = new Vector3(transform.position.x, transform.position.y, 0);
            GameObject enemyBlood = Instantiate(BloodPile, enemyPosition, enemyRotation);
            gameLogic.kills++;
            gameLogic.killsForGold++;
            Destroy(gameObject);
        }

        rig.AddForce(transform.up * baseSpeed * direction);
        float distance = Mathf.Sqrt(Mathf.Pow(player.transform.position.y - transform.position.y, 2) + Mathf.Pow(player.transform.position.x - transform.position.x, 2));
        if (distance < instance.waveRadius && dangerTime > 3)
        {
            float rand = Random.value;
            if (rand > chanceToIgnoreShot)
            { 
                direction = 1;
                transform.rotation = Quaternion.Euler(0, 0, 90);
                rig.AddForce(transform.up * baseSpeed * 10);
                dangerTime = 0;
            }
            else dangerTime = 0f;
        }
        else if (dangerTime > 3 && dangerTime < 3.5)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
            rig.AddForce(transform.up * baseSpeed * 2);
        }
        if (rig.velocity.x > 20)
        {
            rig.velocity = new Vector3(0, 0, 0);
        }
        else if (rig.velocity.x < -20)
        {
            rig.velocity = new Vector3(0, 0, 0);
        }
        rig.AddForce(transform.up * baseSpeed * Mathf.Pow(rig.velocity.x, 2) * -0.3f);

        GameObject[] enemyList;
        enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemyList.Length; i++)
        {
            float engageDistance = Mathf.Sqrt(Mathf.Pow(enemyList[i].transform.position.y - transform.position.y, 2) + Mathf.Pow(enemyList[i].transform.position.x - transform.position.x, 2));
            if (engageDistance < 2)
            {
                float chance = Random.value;
                if (chance > 0.5f)
                {
                    // enemyList[i].SetActive(false);
                    Quaternion enemyRotation = new Quaternion(0, 0, 90, 0);
                    Vector3 enemyPosition = new Vector3(enemyList[i].transform.position.x, enemyList[i].transform.position.y, 0);
                    GameObject enemyBlood = Instantiate(BloodPile, enemyPosition, enemyRotation);
                    Destroy(enemyList[i]);
                    gameLogic.kills++;
                    gameLogic.killsForGold++;

                }
                else if (chance < 0.5f)
                {
                    //gameObject.SetActive(false);
                    Quaternion enemyRotation = new Quaternion(0, 0, 90, 0);
                    Vector3 enemyPosition = new Vector3(transform.position.x, transform.position.y, 0);
                    GameObject enemyBlood = Instantiate(BloodPile, enemyPosition, enemyRotation);
                    Destroy(gameObject);
                }
            }
        }
    }
}
