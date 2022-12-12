using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    GameObject pauseMenu;
    PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Awake()
    {
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        pauseMenu.SetActive(false);
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseToggle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (pauseMenu.activeInHierarchy)
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1f;
                playerMovement.canMove = true;
            }
            else
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0f;
                playerMovement.canMove = false;
            }
        }
    }
}
