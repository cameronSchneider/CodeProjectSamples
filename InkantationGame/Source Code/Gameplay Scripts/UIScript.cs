using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject controlsMenu;
    [SerializeField] private GameObject glyphMenu;
    [SerializeField] private GameObject healthBar;

    private bool isPaused = false;

    void Awake()
    {
        pauseMenu.SetActive(false);
        controlsMenu.SetActive(false);
        glyphMenu.SetActive(false);
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKeyDown(KeyCode.Mouse1))
        {
            isPaused = !isPaused;

            if (!isPaused)
            {
                DisableAll();
                return;
            }

            TogglePause();
        }
    }

    public void ToggleControls()
    {
        controlsMenu.SetActive(!controlsMenu.activeSelf);
    }

    public void ToggleGlyphs()
    {
        glyphMenu.SetActive(!glyphMenu.activeSelf);
    }

    public void TogglePause()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    private void DisableAll()
    {
        pauseMenu.SetActive(false);
        controlsMenu.SetActive(false);
        glyphMenu.SetActive(false);
    }
}
