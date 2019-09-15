using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalacodaScript : MonoBehaviour
{
    public float lifespan = 5.0f;

    private ParticleSystem fire;
    private float time;

    private PlayerAudio playerAudio;

    // Start is called before the first frame update
    void Start()
    {
        playerAudio = FindObjectOfType<PlayerAudio>();
        playerAudio.requestMalacodaClip();

        fire = gameObject.GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

        time += Time.deltaTime;
        if (time % 5.0f <= 0.1f)
            fire.Play();

        if (time >= lifespan)
            gameObject.SetActive(false);
    }
}