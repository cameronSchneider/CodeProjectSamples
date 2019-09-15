using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TextMeshProUGUI winText;

    [Tooltip("The level changer script, located on the BlackFade object under LevelChanger")]
    public LevelChanger levelChanger;

    private bool ended = false;
    private PlayerAudio playerAudio;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        playerAudio = FindObjectOfType<PlayerAudio>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndGame(char c)
    {
        if (!ended)
        {
            if (c == 'w')
                winText.text = "Freedom!";
            else
                winText.text = "You Lost!";

            Time.timeScale = 0f;
        }
        ended = true;
        StartCoroutine(timerTillMain());
    }

    IEnumerator timerTillMain()
    {
        yield return new WaitUntil(() => !playerAudio.GetSource().isPlaying);
        levelChanger.FadeToLevel(0); //main menu
    }

    public void LoadScene(int index)
    {
        levelChanger.FadeToLevel(index);
    }
}