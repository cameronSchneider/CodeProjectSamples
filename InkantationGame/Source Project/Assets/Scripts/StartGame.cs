using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [Tooltip("The level changer script, located on the BlackFade object under LevelChanger")]
    public LevelChanger levelChanger;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            levelChanger.FadeToLevel(1);
    }
}
