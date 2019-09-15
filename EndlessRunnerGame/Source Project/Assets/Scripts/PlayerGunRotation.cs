using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Functions2DAndUI;

public class PlayerGunRotation : MonoBehaviour
{

    void Update()
    {
        LookAtMouse();
    }

    void LookAtMouse()
    {
        Vector2 mouseScreen = Input.mousePosition;
        Vector2 mouse = Camera.main.ScreenToWorldPoint(mouseScreen);

        CustomFunctions.LookAt2D(transform, transform.position, mouse);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponentInParent<PlayerShooting>().enemyInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponentInParent<PlayerShooting>().enemyInRange = false;
        }
    }
}
