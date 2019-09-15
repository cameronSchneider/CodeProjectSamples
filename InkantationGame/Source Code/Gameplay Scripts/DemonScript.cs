using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonScript : MonoBehaviour
{
    public enum Demon
    {
        Malacoda,
        Kek
    };

    /*
    [Tooltip("The time Kek is active before disapearing")]
    public float maxDetectDist = 15.0f;
    public Demon demon;
    */
    private GameObject[] enemies;
    private GameObject closestEnemy;
    private float distFromPlayer;

    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEnemyList();

        if (enemies.Length > 0)
        {
            CheckProximity();
            Look();
        }
    }

    private void CheckProximity()
    {
        float tempDist = 0.0f;
        float shortestDist = 25.0f;
        int index = 0;

        // Find distance between cop and player
        for (int i = 0; i < enemies.Length; i++)
        {
            tempDist = Vector3.Distance(enemies[i].transform.position, this.transform.position);
            if (tempDist < shortestDist)
            {
                index = i;
                shortestDist = tempDist;
            }
        }
        closestEnemy = enemies[index];
    }

    private void Look()
    {
        Vector3 lookPt = closestEnemy.transform.position;
        transform.LookAt(lookPt);

        // Correct billboard direction and account for looking down at a target
        transform.eulerAngles = new Vector3(
            0.0f, 
            transform.eulerAngles.y + 180, 
            transform.eulerAngles.z
            );
    }

    private void UpdateEnemyList()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }
}