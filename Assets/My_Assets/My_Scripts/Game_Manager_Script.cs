using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager_Script : MonoBehaviour
{
    public static Game_Manager_Script instance;

    public GameObject healthPickUpGO;
    public GameObject ammoPickUpGO;
    public GameObject dropShipGO;


    public GameObject[] Enemy_GO_Ary;

    public int spawnNumEnemies = 1;
    public int currentNumEnemies = 1;

    void Awake()                                            // First function to run in scene
    {
        instance = this;                                    // This object is the only instance of Game_Controller_Script
    }

    public void SpawnPickUps()
    {
        GameObject[] ammoGO = GameObject.FindGameObjectsWithTag("MaxAmmo") as GameObject[];
        GameObject[] healthGO = GameObject.FindGameObjectsWithTag("MaxHealth") as GameObject[];

        for (var i = 0; i < ammoGO.Length; i++)
        {
            Destroy(ammoGO[i]);
        }
        for (var i = 0; i < healthGO.Length; i++)
        {
            Destroy(healthGO[i]);
        }

        Instantiate(healthPickUpGO, new Vector3(-12, 2.5f, 28), Quaternion.Euler(0, 0, 90));
        Instantiate(ammoPickUpGO, new Vector3(-12, 2.5f, -28), Quaternion.Euler(0, 0, 90));
    }
    public void SpawnDropShip()
    {
        SpawnPickUps();
        Instantiate(dropShipGO, new Vector3(-26, 500, 0), Quaternion.identity);
    }
    public void ChangeEnemyNumber(int num)
    {
        currentNumEnemies -= num;
        if (currentNumEnemies <= 0)
            SpawnEnemy();
    }
    public void SpawnEnemy()
    {
        spawnNumEnemies *= 2;
        for (int i = 0; i < spawnNumEnemies; i++)
        {
            int rndNum = Random.Range(1, Enemy_GO_Ary.Length);
            Instantiate(Enemy_GO_Ary[rndNum]);
            currentNumEnemies++;
        }
    }
}