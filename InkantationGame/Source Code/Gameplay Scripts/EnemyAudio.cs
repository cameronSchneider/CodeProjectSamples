using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    private AudioSource source;
    private Transform camTrans;
    private EnemyMoveScript movement;
    private float timer;
    private float maxTimeBetweenLines;

    void Start()
    {
        camTrans = Camera.main.transform;
        source = GetComponent<AudioSource>();
        movement = GetComponent<EnemyMoveScript>();
        maxTimeBetweenLines = AudioManager.instance.timeBetweenEnemyLines;
        timer = 0.0f;

        AudioManager.instance.registerAudioSource(source);
    }

    
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= maxTimeBetweenLines)
        {
            if (detectBehind() && movement.canChase)
            {
                requestBehindClip();
            }
            else if (!detectBehind() && movement.canChase)
            {
                requestFrontClip();
            }

            timer = 0.0f;
        }
    }

    void requestBehindClip()
    {
        AudioManager.instance.addRequest(new AudioRequest(source, RequestType.ENEMY_BEHIND_CLIP));
    }

    void requestFrontClip()
    {
        AudioManager.instance.addRequest(new AudioRequest(source, RequestType.ENEMY_FRONT_CLIP));
    }

    bool detectBehind()
    {
        Vector3 camRelative = camTrans.InverseTransformPoint(transform.position);

        if(camRelative.z > 0.0)
        {
            //Player in front of enemy
            return false;
        }
        else
        {
            //Player behind enemy
            return true;
        }
    }

}
