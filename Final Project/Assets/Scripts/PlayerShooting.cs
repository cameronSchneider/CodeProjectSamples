using UnityEngine;
using UnityEngine.Networking;

public class PlayerShooting : NetworkBehaviour
{
    public ParticleSystem particles;
    public Transform raycastOrigin;
    public LayerMask player;

    [HideInInspector]
    public bool enemyInRange = false;

    bool isFiring = false;

    void Start ()
    {
        if (!isLocalPlayer)
        {
            gameObject.name = "NOT LOCAL";
            GetComponentInChildren<PlayerGunRotation>().enabled = false;
        }
        
	}
	
	void Update ()
    {
        if (isLocalPlayer)
        {
            CheckInput();
            DealDamage();
        }
    }

    void CheckInput()
    {
        if (Input.GetMouseButton(0))
        {
            isFiring = true;
            PlayFireParticles(true);
        }
        else
        {
            isFiring = false;
            PlayFireParticles(false);
        }
    }

    void DealDamage()
    {
        if(enemyInRange && isFiring)
        {
            Vector2 mouseScreen = Input.mousePosition;
            Vector3 mouse = Camera.main.ScreenToWorldPoint(mouseScreen);

            Vector2 dir = mouse - raycastOrigin.position;

            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin.position, dir, 10f, player);
            if(hit)
            {
                CmdFreezeEnemy(hit.collider.gameObject);
            }
        }
    }

    [Command]
    void CmdFreezeEnemy(GameObject hit)
    {
        hit.GetComponent<PlayerHealth>().RpcBeingFrozen(true);
    }

    [Command]
    void CmdPlayFireParticles(bool firingActive)
    {
        RpcPlayFireParticles(firingActive);
    }

    [ClientRpc]
    void RpcPlayFireParticles(bool firingActive)
    {
        if (isLocalPlayer)
            return;
        else
            PlayFireParticles(firingActive);
    }

    void PlayFireParticles(bool firingActive)
    {
        if (isLocalPlayer)
            CmdPlayFireParticles(firingActive);

        if(firingActive)
        {
            particles.Play();
        }
        else
        {
            particles.Stop();
        }
    }
}
