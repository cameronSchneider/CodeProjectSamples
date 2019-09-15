using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour 
{
    public float xSpeed = 10f;

    float xMove = 0f;
	Rigidbody2D rb;

    GameObject body;
    GameObject arm;

	// Use this for initialization
	void Start () 
	{
        if (isLocalPlayer)
        {
            rb = GetComponent<Rigidbody2D>();
        }

    }
	
	// Update is called once per frame
	void Update () 
	{
        if(!isLocalPlayer)
        {
            return;
        }

		CheckInput ();
	}

	void FixedUpdate()
	{
        if(!isLocalPlayer)
        {
            return;
        }

		Move ();
	}

    void CheckInput()
	{
		xMove = Input.GetAxis ("Horizontal") * xSpeed;
	}

	void Move()
	{
		rb.velocity = new Vector2 (xMove, rb.velocity.y);
	}

    public override void OnStartClient()
    {
        if (isServer)
        {
            transform.position = new Vector2(-6f, .34f);
        }
        else if (isClient)
        {
            transform.position = new Vector2(21f, .34f);
        }
    }
}
