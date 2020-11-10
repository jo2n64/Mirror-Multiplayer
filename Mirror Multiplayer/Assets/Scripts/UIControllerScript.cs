using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControllerScript : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private bool isPaused;
    [SerializeField] private string menuScene = string.Empty;
    [SerializeField] private PlayerCameraController cameraController;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerJumpScript jumpScript;
    [SerializeField] private PlayerShootScript shootScript;

    [Client]
    public void Pause()
    {
        if (hasAuthority)
        {
            Cursor.lockState = CursorLockMode.None;
            cameraController.enabled = false;
            playerMovement.enabled = false;
            jumpScript.enabled = false;
            shootScript.enabled = false;
        }
    }

    private void Update()
    {
        if (hasAuthority)
        {
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                if (!isPaused)
                {
                    Pause();
                    isPaused = true;
                }
                else if (isPaused)
                {
                    UnPause();
                    isPaused = false;
                }
            }
        }
    }

    [Client]
    public void UnPause()
    {
        if (hasAuthority)
        {
            Cursor.lockState = CursorLockMode.Locked;
            cameraController.enabled = true;
            playerMovement.enabled = true;
            jumpScript.enabled = true;
            shootScript.enabled = true;
        }
    }

    public void LeaveServer()
    {
        if (hasAuthority)
        {
            SceneManager.LoadScene(menuScene);
        }
    }

}
