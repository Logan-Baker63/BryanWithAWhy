using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    GameObject pauseMenu;
    PlayerMovement playerMovement;
    RotateToMouse rotateToMouse;
    PlayerAttack attack;

    // Start is called before the first frame update
    void Awake()
    {
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        pauseMenu.SetActive(false);
        playerMovement = FindObjectOfType<PlayerMovement>();
        rotateToMouse = FindObjectOfType<RotateToMouse>();
        attack = FindObjectOfType<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseToggle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (FindObjectOfType<DevMode>().GetDevType() == DevMode.DevType.None)
            {
                if (pauseMenu.activeInHierarchy)
                {
                    pauseMenu.SetActive(false);
                    Time.timeScale = 1f;
                    playerMovement.canMove = true;
                    rotateToMouse.canRotate = true;
                    if (attack.cooldownRoutine == null)
                    {
                        attack.canAttack = true;
                    }
                }
                else
                {
                    pauseMenu.SetActive(true);
                    Time.timeScale = 0f;
                    playerMovement.canMove = false;
                    rotateToMouse.canRotate = false;
                    attack.canAttack = false;
                }
            }
            else
            {
                FindObjectOfType<DevMode>().ExitDevMode();
            }
        }
    }
}
