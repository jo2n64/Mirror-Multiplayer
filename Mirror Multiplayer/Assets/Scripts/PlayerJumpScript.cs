using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerJumpScript : NetworkBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private bool jumpPressed;
    [SerializeField] private AudioClip jumpSound;
    
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
        enabled = true;

        Controls.Player.Jump.performed += ctx => SetJump();
    }
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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

    [ClientCallback]
    private void FixedUpdate()
    {
        if (jumpPressed)
        {
            Jump();
        }

        jumpPressed = false;
    }
    
    
    [Client]
    private void SetJump()
    {
        jumpPressed = true;
    }

    [Client]
    private void Jump()
    {
        CmdJump();
    }

    [Command]
    private void CmdJump()
    {
        RpcJump();
    }

    [ClientRpc]
    private void RpcJump()
    {
        AudioSource.PlayClipAtPoint(jumpSound, transform.position);
        Vector3 jumpVec = Vector3.up * jumpForce;
        rb.velocity += jumpVec;
    }
    
    
}
