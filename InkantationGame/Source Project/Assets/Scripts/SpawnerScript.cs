using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public float activationRange;
    public float spawnTimer;
    float timerSet;
    bool activate = false;
    GameObject enemy;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        float timerSet = spawnTimer;

        enemy = Resources.Load<GameObject>("Prefabs/World_Kit/Enemies/Placeholder_Enemy");
        player = GameObject.Find("PlayerController");
    }

    void SpawnEnemies()
    {
        timerSet -= Time.deltaTime;
        if(timerSet <= 0)
        {
            Instantiate(enemy, transform.position, Quaternion.identity);
            timerSet = spawnTimer;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float playerDistance = (transform.position - player.transform.position).sqrMagnitude;

        if (playerDistance < activationRange)
        {
            //Debug.Log("PlayerDetected");
            activate = true;
        }
        else
        {
            //Debug.Log("Undetected");
            activate = false;
        }

        if(activate)
        {
            SpawnEnemies();
        }

        //Debug.Log(activate);
    }
}
