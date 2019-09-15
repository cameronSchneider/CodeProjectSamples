using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public Image healthBar;
    public Image flameVignette;
    public float maxFlameDrawTime = 0.1f;
    public float maxHealth = 100.0f;
    public float boostTime = 10.0f;
    public float fireDamage = 1.5f;
    public PostProcessScript post;

    private List<EnemyMoveScript> enemies;
    private CharacterController controller;
    private Vector3 impact;
    public ParticleSystem[] sprays;
    private int numEnemiesTouching;
    private float health;
    private float boostTimer;
    private bool spraying;
    private bool fireBuff;

    private float flameDrawTimer;

    // Start is called before the first frame update
    void Awake()
    {
        health = maxHealth;
        numEnemiesTouching = 0;

        spraying = false;
        fireBuff = false;

        enemies = new List<EnemyMoveScript>();

        controller = gameObject.GetComponent<CharacterController>();

        impact = Vector3.zero;

        flameVignette.fillAmount = 0;
        flameVignette.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckSurrounded();

        healthBar.fillAmount = health / maxHealth;

        if (Input.GetMouseButton(0))
            SprayEnemy();

        if (Input.GetMouseButtonUp(0))
        {
            StopAllSprays();
        }

        // Get input
        UpdateState();

        /* - Handle Knockback - */
        // Use controller move funciton to handle knockback
        if (impact.magnitude > 0.2)
            controller.Move(impact * Time.deltaTime);
        // Slow impact to 0
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
    }

    private void UpdateState()
    {
        spraying = GetComponent<WiiControlsDPad>().CheckIfSpraying();

        if (boostTimer > 0)
        {
            flameVignette.gameObject.SetActive(true);

            if (flameDrawTimer < maxFlameDrawTime)
                flameDrawTimer += Time.deltaTime;
            else
                flameDrawTimer = maxFlameDrawTime;

            flameVignette.fillAmount = flameDrawTimer / maxFlameDrawTime;

            boostTimer -= Time.deltaTime;

            if (boostTimer <= 0)
            {
                boostTimer = 0.0f;
                fireBuff = false;
                sprays[0].Stop();

                flameVignette.fillAmount = 0.0f;
                flameVignette.gameObject.SetActive(false);
            }
        }
    }

    private void StopAllSprays()
    {
        foreach(ParticleSystem p in sprays)
        {
            p.Stop();
        }
    }

    private void SprayEnemy()
    {
        RaycastHit hit = GetComponent<SymbolScript>().GetRayHit(Input.mousePosition);

        if(fireBuff)
        {
            sprays[0].Play();
        }
        else
        {
            sprays[1].Play();
        }

        if (hit.collider == null)
        {
            return;
        }

        if (hit.collider.tag == "Enemy")
        {
            if (fireBuff)
            {
                hit.collider.gameObject.GetComponent<EnemyScript>().ApplyDamage(fireDamage);
            }
            else
            {
                hit.collider.gameObject.GetComponent<EnemyMoveScript>().BecomeStunned();
            }
        }
    }

    public void GetDamageBuff()
    {
        fireBuff = true;
        boostTimer = boostTime;
    }

    public void UpdateHealth(float d, Vector3 k)
    {
        post.GetHit();

        /* - Add impact - */
        // Direction
        Vector3 direction = k.normalized;
        direction.y = 0.15f; // Add upward velocity
        impact += direction.normalized * k.magnitude;

        /* - Update health - */
        health -= d;
        GetComponent<PlayerAudio>().requestHurtClip();

        /* - Check if dead - */
        if (health <= 0.0f)
        {
            health = 0.0f;
            GetComponent<PlayerAudio>().requestLossClip();
            GameManager.instance.EndGame('l');
        }
    }

    public void AddEnemyToList(EnemyMoveScript enemy)
    {
        enemies.Add(enemy);
    }

    private void CheckSurrounded()
    {
        //numEnemiesTouching = 0;

        float tempDist = 0.0f;

        // Find distance between cop and player
        for (int i = 0; i < enemies.Count; i++)
        {
            tempDist = Vector3.Distance(enemies[i].transform.position, this.transform.position);
            if (tempDist < 3.0f)
            {
                numEnemiesTouching++;
            }
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // gameObject.GetComponent<Rigidbody>().velocity *= 0.35f;
        }
    }
}