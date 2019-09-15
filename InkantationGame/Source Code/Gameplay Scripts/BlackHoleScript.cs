using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit");
            other.GetComponent<EnemyScript>().ApplyDamage(150.0f);
            transform.parent.GetComponent<KekScript>().ActivateTrapCard();
        }
    }
}