using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveScript : MonoBehaviour
{
    public float speed = 10.0f;

    private Rigidbody rb;
   
    private float xMove;
    private float yMove;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
    }

    void GetInput()
    {
        xMove = Input.GetAxis("Horizontal");
        yMove = Input.GetAxis("Vertical");
    }

    void Move()
    {
        Vector3 vel;

        vel.x = speed * xMove;
        vel.y = rb.velocity.y;
        vel.z = speed * yMove;

        rb.velocity = vel;
    }
}
