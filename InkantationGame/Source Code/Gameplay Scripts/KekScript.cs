using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KekScript : MonoBehaviour
{
    [Tooltip("The time Kek is active before disapearing")]
    public float activationTime = 2.0f;

    private float time;
    private bool activated;

    private AudioSource kekSource;
    private PlayerAudio playerAudio;

    // Start is called before the first frame update
    void Start()
    {
        playerAudio = FindObjectOfType<PlayerAudio>();
        playerAudio.requestKekClip();

        kekSource = GetComponent<AudioSource>();

        time = 0.0f;
        activated = false;

        // Set "invisible"
        Color temp = gameObject.GetComponentsInChildren<SpriteRenderer>()[0].color;
        temp.a = 0.2f;
        gameObject.GetComponentsInChildren<SpriteRenderer>()[0].color = temp;
        temp = gameObject.GetComponentsInChildren<SpriteRenderer>()[1].color;
        temp.a = 0.2f;
        gameObject.GetComponentsInChildren<SpriteRenderer>()[1].color = temp;
    }

    // Update is called once per frame
    void Update()
    {
        // Timer to deactivate after it is triggered
        if (time >= activationTime && activated && !kekSource.isPlaying)
            gameObject.SetActive(false);

        // Timer
        time += Time.deltaTime;
    }

    public void ActivateTrapCard()
    {
        // Check if already triggered
        if (!activated)
        {
            // Set visible
            Color temp = gameObject.GetComponentsInChildren<SpriteRenderer>()[0].color;
            temp.a = 1.0f;
            gameObject.GetComponentsInChildren<SpriteRenderer>()[0].color = temp;
            temp = gameObject.GetComponentsInChildren<SpriteRenderer>()[1].color;
            temp.a = 1.0f;
            gameObject.GetComponentsInChildren<SpriteRenderer>()[1].color = temp;

            // Acivate
            activated = true;

            // Reset timer so you dont have to track "activation time" or something
            time = 0.0f;

            AudioManager.instance.CalcEasterEggChance(kekSource);
        }
    }
}