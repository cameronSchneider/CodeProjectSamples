using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GestureRecognizer;

public class PlayerAudio : MonoBehaviour
{
    private AudioSource source;


    void Start()
    {
        source = GetComponent<AudioSource>();
        GetComponentInChildren<AudioListener>().enabled = true;
    }

    public AudioSource GetSource()
    {
        return source;
    }

    public void requestKekClip()
    {
        AudioManager.instance.addRequest(new AudioRequest(source, RequestType.KEK_CLIP));
    }

    public void requestMalacodaClip()
    {
        AudioManager.instance.addRequest(new AudioRequest(source, RequestType.MALACODA_CLIP));
    }

    public void requestHurtClip()
    {
        AudioManager.instance.sendHurtClip(source);
    }

    public void requestLossClip()
    {
        AudioManager.instance.sendLossClip(source);
    }
}
