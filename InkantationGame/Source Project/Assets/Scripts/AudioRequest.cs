using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RequestType
{
    ENEMY_FRONT_CLIP,
    ENEMY_BEHIND_CLIP,
    MALACODA_CLIP,
    KEK_CLIP,
    JUDAS_HURT_CLIP,
    PLAYER_LOSES_CLIP,
    KEK_EASTER_EGG
}


public class AudioRequest
{
    private AudioSource m_SenderSource;
    private float m_LifeSpan;
    private RequestType m_Type;

    public AudioRequest(AudioSource sender, RequestType type)
    {
        m_SenderSource = sender;
        m_Type = type;
        m_LifeSpan = 0.0f;
    }

    public void Update()
    {
        m_LifeSpan += Time.deltaTime;
    }

    public float getLifeLength()
    {
        return m_LifeSpan;
    }

    public AudioSource getSource()
    {
        return m_SenderSource;
    }

    public RequestType getType()
    {
        return m_Type;
    }
}
