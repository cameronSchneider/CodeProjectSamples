using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JumpScript : NetworkBehaviour 
{
	[SerializeField] float jumpForce;
	Rigidbody2D rb;

	public LayerMask ground;

	bool isGrounded = true;

	// Use this for initialization
	void Start () 
	{
        if(!isLocalPlayer)
        {
            return;
        }

		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		CheckInput ();
		CheckGround ();
	}

	void CheckInput ()
	{
		if (isGrounded && Input.GetKeyDown (KeyCode.W)) 
		{
			Jump ();
		}
	}

	void Jump()
	{
        if(isLocalPlayer)
		    rb.AddForce (new Vector2 (0f, jumpForce));
	}

	void CheckGround()
	{
		Collider2D col = Physics2D.OverlapCircle (transform.position, 1.1f, ground);

		if (col == null)
			isGrounded = false;
		else
			isGrounded = true;
	}
}
