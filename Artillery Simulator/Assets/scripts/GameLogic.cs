using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    #region Editor references
    public GameObject[] friendlyPrefabs;
    public GameObject[] enemyPrefabs;
    public AudioSource music;
    public Canvas shop;
    public MoveShot shotScript;
    public Text waveText;
    public Text killText;
    public Text goldText;
    public Text goldText2;
    public Text ammoText;
    public Text textPriceAmmo;
    public Text textPriceFriendly;
    public Text textPriceSpeed;
    #endregion
    #region Game variables
    [HideInInspector] public float friendlyKills = 0;
    [HideInInspector] public float waveDifficultyPerLevel = 2;
    [HideInInspector] public float currentWave = 0;
    [HideInInspector] public float Basedifficulty = 3;
    [HideInInspector] public float amountToKill = 1;
    [HideInInspector] public float gold = 0;
    [HideInInspector] public float difficulty = 1;
    [HideInInspector] public float kills = 0;
    [HideInInspector] public float ammoPerWave = 10;
    [HideInInspector] public float extraAmmo = 1;
    [HideInInspector] public float currentAmmo = 10;
    [HideInInspector]public float killsForGold = 0;
    float spawnIntervalEnemy = 4f;
    float timeLeftEnemy = 0;
    float timeLeftFriendly = 0;
    float spawnIntervalFriendly = 8f;
    float maxDecals = 40;
    float friendlySpawnRate = 2;
    float priceSpeed = 40, priceFriendly = 40, priceAmmo = 30;
    #endregion

    // Use this for initialization
    void Start()
    {
        shop.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        timeLeftEnemy += Time.deltaTime;
        timeLeftFriendly += Time.deltaTime;


        if (currentAmmo <= 0)
        {
            SceneManager.LoadScene("MainMenu");
            SceneManager.UnloadSceneAsync("Test");
            music.Stop();
        }
        if (kills >= amountToKill) nextWave();

        #region Spawning
        if (timeLeftEnemy > spawnIntervalEnemy)
        {
            Quaternion enemyRotation = new Quaternion(0, 0, 90, 90);
            Vector3 enemyPosition = new Vector3(Random.Range(25, 30), Random.Range(-15, 14), -0.1f);
            int rand = Random.Range(0, enemyPrefabs.Length);
            GameObject enemyClone = Instantiate(enemyPrefabs[rand], enemyPosition, enemyRotation);
            timeLeftEnemy = 0;
        }
        if (timeLeftFriendly > spawnIntervalFriendly)
        {
            Quaternion friendlyRotation = Quaternion.Euler(0, 0, 90);
            Vector3 friendlyPosition = new Vector3(Random.Range(-15, -9), Random.Range(-15, 14), -0.1f);
            int rand = Random.Range(0, friendlyPrefabs.Length);
            GameObject friendlyClone = Instantiate(friendlyPrefabs[rand], friendlyPosition, friendlyRotation);

            timeLeftFriendly = 0;
        }
        #endregion
        #region decals
        GameObject[] decalList;
        decalList = GameObject.FindGameObjectsWithTag("Decal");
        if (decalList.Length > maxDecals)
        {
            Destroy(decalList[0]);
        }
        #endregion

        killText.text = "Kills:" + kills;
        goldText2.text = goldText.text;
        goldText.text = "" + Mathf.Round(gold);
        ammoText.text = "" + currentAmmo;

    }
    
    void nextWave()
    {
        currentWave++;
        gold += currentWave * 10;
        difficulty = currentWave * waveDifficultyPerLevel + Basedifficulty;
        amountToKill = difficulty;
        waveText.text = "Wave:" + currentWave;
        kills = 0;
        spawnIntervalEnemy = 25 / difficulty;
        ammoPerWave += extraAmmo;
        currentAmmo = ammoPerWave;

        if (friendlyKills > 0) friendlyKills--;
    }
    public void addAmmo()
    {
        if (gold >= priceAmmo)
        {
            ammoPerWave += 3;
            gold -= priceAmmo;
            priceAmmo *= 1.2f;
            textPriceAmmo.text = "" + Mathf.Round(priceAmmo);
        }
    }
    public void addFriendly()
    {
        if (gold >= priceFriendly)
        {
            friendlySpawnRate++;
            spawnIntervalFriendly = 16 / friendlySpawnRate;
            gold -= priceFriendly;
            priceFriendly *= 1.3f;
            textPriceFriendly.text = "" + Mathf.Round(priceFriendly);
        }
    }
    public void addSpeed()
    {
        if (gold >= priceSpeed)
        {
            shotScript.baseSpeed += 1.0f;
            gold -= priceSpeed;
            priceSpeed *= 1.3f;

            textPriceSpeed.text = "" + Mathf.Round(priceSpeed);
        }
    }
}
