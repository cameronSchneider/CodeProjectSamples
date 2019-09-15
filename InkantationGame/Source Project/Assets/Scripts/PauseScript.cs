using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    private bool toggle;

    void Awake()
    {
        toggle = false;
        pauseMenu.SetActive(false);
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKeyDown(KeyCode.Mouse1))
        {
            toggle = !toggle;
            pauseMenu.SetActive(toggle);
        }
    }
}
