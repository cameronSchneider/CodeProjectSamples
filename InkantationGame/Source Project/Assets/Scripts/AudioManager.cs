using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioClip[] behindClips;
    [SerializeField] private AudioClip[] frontClips;
    [SerializeField] private AudioClip[] judasMalacoda;
    [SerializeField] private AudioClip[] judasKek;
    [SerializeField] private AudioClip[] judasHurt;
    [SerializeField] private AudioClip[] playerLoses;

    [SerializeField] private AudioClip kekEasterEgg;

    [Range(0, 100)]
    [Tooltip("Chance the easter egg voice line plays, as a percent")]
    [SerializeField] private float easterEggChance;

    [Tooltip("Gap between enemy voice lines")]
    public float timeBetweenEnemyLines = 3.0f;

    //[Tooltip("Gap between player hurt voice lines")]
    //[SerializeField] private float timeBetweenHurtLines = 1.25f;

    [Tooltip("The max time a single clip can exist in the Queue. If a clip lives longer than this time, it will not play")]
    [SerializeField] private float maxRequestLife = 3.0f;

    [Tooltip("do you want to randomize Judas's hurt voice lines?")]
    [SerializeField] private bool randomizeJudasVoicePitch;

    [Tooltip("The min pitch to modify Juda's voice lines (-.2 to .2)")]
    [Range(-.2f, .2f)]
    [SerializeField] private float minPitch;

    [Tooltip("The min pitch to modify Juda's voice lines (.8 to 1.2)")]
    [Range(.8f, 1.2f)]
    [SerializeField] private float maxPitch;
    
    
    private Queue<AudioRequest> requests;
    private List<AudioSource> sources;
    private AudioRequest currentReq;

    private float enemyLineTimer = 0.0f;
    private int numHurtPlayed = 0;

    void Awake()
    {
        instance = this;
        sources = new List<AudioSource>();
        requests = new Queue<AudioRequest>();
    }

    
    void Update()
    {
        enemyLineTimer += Time.deltaTime;

        foreach(AudioRequest r in requests)
        {
            r.Update();
        }

        if(!otherIsPlaying() && requests.Count != 0)
        {
            playNext();
        }
    }

    public void CalcEasterEggChance(AudioSource src)
    {
        float val = Random.Range(0f, 100f);

        if (val <= easterEggChance)
        {
            sendEasterEggClip(src);
        }
    }

    public void addRequest(AudioRequest req)
    {
        requests.Enqueue(req);
    }

    public void registerAudioSource(AudioSource src)
    {
        sources.Add(src);
    }

    public void stopPlayingAndClear()
    {
        requests.Clear();
    }

    void playNext()
    {
        currentReq = requests.Dequeue();

        if(currentReq.getLifeLength() > maxRequestLife)
        {
            print("Request expired");
            return;
        }

        switch(currentReq.getType())
        {
            case RequestType.ENEMY_BEHIND_CLIP:
                sendBehindClip();
                break;

            case RequestType.ENEMY_FRONT_CLIP:
                sendFrontClip();
                break;

            case RequestType.MALACODA_CLIP:
                sendMalacodaClip();
                break;

            case RequestType.KEK_CLIP:
                sendKekClip();
                break;
        }
    }


    bool otherIsPlaying()
    {
        if(requests.Count == 0)
        {
            return false;
        }

        foreach(AudioSource src in sources)
        {
            if(src.isPlaying)
            {
                return true;
            }
        }

        return false;
    }

    bool clipIsRepeat(AudioClip current, AudioClip prev)
    {
        return current == prev;
    }

    void sendBehindClip()
    {
        if(!currentReq.getSource().GetComponent<EnemyMoveScript>().canChase || enemyLineTimer <= timeBetweenEnemyLines)
        {
            return;
        }

        int randClip = Random.Range(0, behindClips.Length);

        currentReq.getSource().clip = behindClips[randClip];
        currentReq.getSource().Play();

        enemyLineTimer = 0.0f;
    }

    void sendFrontClip()
    {
        if (!currentReq.getSource().GetComponent<EnemyMoveScript>().canChase || enemyLineTimer <= timeBetweenEnemyLines)
        {
            return;
        }

        int randClip = Random.Range(0, frontClips.Length);

        currentReq.getSource().clip = frontClips[randClip];
        currentReq.getSource().Play();

        enemyLineTimer = 0.0f;
    }

    void sendMalacodaClip()
    {
        int randClip = Random.Range(0, judasMalacoda.Length);

        currentReq.getSource().clip = judasMalacoda[randClip];
        currentReq.getSource().Play();
    }

    void sendKekClip()
    {
        int randClip = Random.Range(0, judasKek.Length);

        currentReq.getSource().clip = judasKek[randClip];
        currentReq.getSource().Play();
    }

    public void sendHurtClip(AudioSource playerSrc)
    {
        int randClip = Random.Range(0, judasHurt.Length);

        if(randomizeJudasVoicePitch)
        {
            float randPitch;
            randPitch = Random.Range(minPitch, maxPitch);
            playerSrc.pitch = randPitch;
        }

        playerSrc.clip = judasHurt[randClip];
        playerSrc.Play();
    }

    public void sendLossClip(AudioSource playerSrc)
    {
        int randClip = Random.Range(0, playerLoses.Length);

        playerSrc.clip = playerLoses[randClip];
        playerSrc.Play();
    }

    void sendEasterEggClip(AudioSource src)
    {
        src.clip = kekEasterEgg;
        src.Play();
    }
}
