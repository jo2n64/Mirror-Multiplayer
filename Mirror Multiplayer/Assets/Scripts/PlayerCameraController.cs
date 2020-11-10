using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Mirror;
using UnityEditor;
using UnityEngine;

public class PlayerCameraController : NetworkBehaviour
{
    // Start is called before the first frame update
    [Header("Camera")] 
    [SerializeField] private float sensitivity = 5f;

    [SerializeField] private float yAxisClamp;
    [SerializeField] private Transform player = null;
    [SerializeField] private CinemachineVirtualCamera virtualCam = null;

    private CinemachineTransposer transposer;
    
    private Controls controls;

    private Controls Controls
    {
        get
        {
            if (controls != null)
            {
                return controls;
            }

            return controls = new Controls();
        }
    }
    public override void OnStartAuthority()
    {
        virtualCam.gameObject.SetActive(true);
        Controls.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
        enabled = true;
    }

    [ClientCallback]
    private void OnEnable()
    {
        Controls.Enable();
    }

    [ClientCallback]
    private void OnDisable()
    {
        Controls.Disable();
    }

    [Client]
    private void Look(Vector2 axis)
    {
        float mouseX = axis.x * sensitivity * Time.fixedDeltaTime;
        float mouseY = axis.y * sensitivity * Time.fixedDeltaTime;
        yAxisClamp += mouseY;
        if (yAxisClamp >= 90.0f)
        {
            yAxisClamp = 90.0f;
            mouseY = 0f;
        }
        else if (yAxisClamp <= -90.0f)
        {
            yAxisClamp = -90.0f;
            mouseY = 0f;
        }
        
        player.transform.Rotate(Vector3.up * mouseX);
        virtualCam.transform.Rotate(Vector3.left * mouseY);
    }
    
}
