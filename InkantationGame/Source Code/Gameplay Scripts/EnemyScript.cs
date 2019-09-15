using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Tooltip("Enemy's health")]
    public float health = 10.0f;
    [Tooltip("Enemy hit range")]
    public float hitDist = 2.0f;
    [Tooltip("Time between hits")]
    public float timePerHit = 5.0f;
    [Tooltip("Damage the enemy deals per hit")]
    public float damage = 5.0f;
    [Tooltip("Force applied on hit")]
    public float hitForce = 15.0f;

    private GameObject player;
    private float distFromPlayer;
        private float hitTimer;
private bool inRange = false;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inRange = false;
        hitTimer = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        CheckProximity();

        if (inRange)
            CountDown();
        else
            hitTimer = timePerHit;
    }

    private void CountDown()
    {
        hitTimer -= Time.deltaTime;

        if (hitTimer <= 0)
        {
            HitPlayer();
            hitTimer = timePerHit;
        }
    }

    private void HitPlayer()
    {
        Vector3 impactVector = player.transform.position - transform.position;
        //player.GetComponent<Rigidbody>().AddForce(direction * hitForce, ForceMode.Impulse);

        player.GetComponent<PlayerScript>().UpdateHealth(damage, impactVector * hitForce);
    }

    private void CheckProximity()
    {
        distFromPlayer = Vector3.Distance(player.transform.position, this.transform.position);

        if (distFromPlayer <= hitDist)
            inRange = true;
        else
            inRange = false;
    }

    // Enemy dies
    private void Kill()
    {
        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(10, 10, 10);
    }

    public float GetProximity()
    {
        return distFromPlayer;
    }

    public Vector3 GetPlayerLocation()
    {
        return player.transform.position;
    }

    // Enemy takes 'd' damage
    public void ApplyDamage(float d)
    {
        health -= d;

        if (health <= 0)
            Kill();
    }

}