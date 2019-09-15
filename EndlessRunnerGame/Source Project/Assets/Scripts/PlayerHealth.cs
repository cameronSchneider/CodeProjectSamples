using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour
{
    public float alphaVal;

    public bool isBeingFrozen = false;
    public SpriteRenderer bodyRenderer;
    public GameObject enemyCold;
    public GameObject friendlyCold;
    public Text nameText;

    [SyncVar]
    public string playerName = "";

    void Start()
    {
        gameObject.name = playerName;
        alphaVal = 1;
    }

    void Update()
    {
        TakeDamage(isBeingFrozen);
    }

    [ClientRpc]
   public void RpcBeingFrozen(bool frozen)
    {
        isBeingFrozen = true;
    }

    void TakeDamage(bool beingFrozen)
    {
        if (beingFrozen)
        {
            bodyRenderer.color = Color.Lerp(bodyRenderer.color,
                            new Color(bodyRenderer.color.r, bodyRenderer.color.g, bodyRenderer.color.b, bodyRenderer.color.a - 1), Time.deltaTime / 1.5f);
            alphaVal = bodyRenderer.color.a;

            if (alphaVal <= 0.1f)
            {
                alphaVal = 0f;
                bodyRenderer.color = new Color(bodyRenderer.color.r, bodyRenderer.color.g, bodyRenderer.color.b, alphaVal);
            }
        }
        else
        {
            bodyRenderer.color = Color.LerpUnclamped(bodyRenderer.color,
                             new Color(bodyRenderer.color.r, bodyRenderer.color.g, bodyRenderer.color.b, bodyRenderer.color.a + 1), Time.deltaTime / 1.75f);
            alphaVal = bodyRenderer.color.a;

            if(alphaVal > 1f)
            {
                alphaVal = 1f;
                bodyRenderer.color = new Color(bodyRenderer.color.r, bodyRenderer.color.g, bodyRenderer.color.b, alphaVal);
            }
        }

        //Changes enemy movement speed
        if (beingFrozen && GetComponent<PlayerMovement>().xSpeed > 0)
        {
            GetComponent<PlayerMovement>().xSpeed = (bodyRenderer.color.a * 10);

            if (GetComponent<PlayerMovement>().xSpeed < 0)
                GetComponent<PlayerMovement>().xSpeed = 0;
        }
        else if (!beingFrozen && GetComponent<PlayerMovement>().xSpeed < 10f)
        {
            GetComponent<PlayerMovement>().xSpeed = (bodyRenderer.color.a * 10);

            if (GetComponent<PlayerMovement>().xSpeed > 10)
                GetComponent<PlayerMovement>().xSpeed = 10;
        }

        isBeingFrozen = false;
    }

    public override void OnStartLocalPlayer()
    {
        nameText.color = Color.green;
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
        enemyCold.SetActive(false);
    }

    public override void OnStartClient()
    {
        Names.namesHolder.allNames.Add(playerName);
        nameText.text = playerName;
        bodyRenderer = GetComponentInChildren<SpriteRenderer>();
        GameManager.code.AddPlayerTimer(GetComponent<PlayerTimer>());
        GameManager.code.AddPlayerGameStates(GetComponent<PlayerGameStateScript>());
        GameManager.code.AddPlayerMovement(GetComponent<PlayerMovement>());
        GetComponent<PlayerGameStateScript>().winPanel = GameObject.Find("WinPanel");
    }


    //This block is from trying to use SyncVars to synchronize gamestates. As it turns out
    // they do not work like the documentation says, and the community agrees. It is better to stick with
    //RPC/Command structure.
    /*
    void OnChangeAlpha(float alpha)
    {
        if (!isLocalPlayer)
        {
            alphaVal = alpha;
            bodyRenderer.color = new Color(bodyRenderer.color.r, bodyRenderer.color.g, bodyRenderer.color.b, alphaVal);
        }
        else
        {
            alphaVal = alpha;
            bodyRenderer.color = new Color(255f, 255f, 255f, alphaVal);
        }
    }
    */

    /*
    void CheckForAndFreezeEnemy()
    {
        if (otherPlayerExists)
        {
            //Changes visual effect of freezing and sets up the alpha value
            if (enemyInRange && isFiring)
            {
                Debug.Log("EnemyInRange: " + enemyInRange + "    isFiring: " + isFiring);
                if (otherPlayerBody.color.a > 0)
                {
                    isFreezingEnemy = true;
                    Debug.Log("IsFreezingEnemy");
                    otherPlayerBody.color = Color.LerpUnclamped(otherPlayerBody.color,
                        new Color(otherPlayerBody.color.r, otherPlayerBody.color.g, otherPlayerBody.color.b, otherPlayerBody.color.a - 1), Time.deltaTime / 3);
                }
            }
            else if (!isFiring || !enemyInRange)
            {
                Debug.Log("EnemyInRange: " + enemyInRange + "    isFiring: " + isFiring);
                if (otherPlayerBody.color.a < 1)
                {
                    isFreezingEnemy = false;
                    Debug.Log("Is NOT Freezing enemy");
                    otherPlayerBody.color = Color.LerpUnclamped(otherPlayerBody.color,
                        new Color(otherPlayerBody.color.r, otherPlayerBody.color.g, otherPlayerBody.color.b, otherPlayerBody.color.a + 1), Time.deltaTime);
                }
            }

            //Changes enemy movement speed
            if (isFreezingEnemy && otherPlayerMovement.xSpeed > 0)
            {
                otherPlayerMovement.xSpeed = (otherPlayerBody.color.a * 10);

                if (otherPlayerMovement.xSpeed < 0)
                    otherPlayerMovement.xSpeed = 0;
            }
            else if (!isFreezingEnemy && otherPlayerMovement.xSpeed < 10f)
            {
                otherPlayerMovement.xSpeed = (otherPlayerBody.color.a * 10);

                if (otherPlayerMovement.xSpeed > 10)
                    otherPlayerMovement.xSpeed = 10;
            }
        }
    }
    */
}
