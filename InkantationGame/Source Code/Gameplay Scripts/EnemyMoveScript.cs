using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveScript : MonoBehaviour
{
    [Tooltip("Enemy's speed")]
    public float speed = 5.0f;
    [Tooltip("Maximum distance that the enemy will pursue from")]
    public float maxDetectDist = 15.0f;
    [Tooltip("Maximum distance that the enemy will pursue from")]
    public float stunDuration = 5.0f;

    [HideInInspector] public bool canChase;

    private GameObject player;
    private float distFromPlayer;
    private float stunTimer;
    private bool stunned = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerScript>().AddEnemyToList(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (!stunned)
        {
            Look();
            CheckProximity();

            // Only move towards player if they are within a certain distance
            if (canChase)
                Chase();
        }
        else
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
                stunned = false;
        }
    }

    private void Chase()
    {
        Vector3 newPos = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);
        transform.position = newPos;
    }

    private void Look()
    {
        Vector3 lookPt = player.transform.position;
        transform.LookAt(lookPt);

        // Correct billboard direction and account for looking down at a target
        transform.eulerAngles = new Vector3(
            0.0f,
            transform.eulerAngles.y + 180,
            transform.eulerAngles.z
            );
    }

    private void CheckProximity()
    {
        distFromPlayer = Vector3.Distance(player.transform.position, this.transform.position);

        // Find distance between cop and player
        if (distFromPlayer <= maxDetectDist && distFromPlayer >= 1.5f)
        {
            canChase = true;
        }
        else
        {
            canChase = false;
        }
    }

    private void Kill()
    {
        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(10, 10, 10);
    }
    
    public void BecomeStunned()
    {
        canChase = false;
        stunned = true;
        stunTimer = stunDuration;
    }
}