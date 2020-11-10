using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Rigidbody rb;

    private Vector2 prevInput;

    private Controls controls;

    public Controls Controls
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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnStartAuthority()
    {
        enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
        Controls.Player.Move.canceled += ctx => ResetMovement();
    }

    [ClientCallback]
    private void OnEnable()
    {
        Controls.Enable();
    }

    [ClientCallback]
    private void OnDisable()
    {
        controls.Disable();
    }

    [ClientCallback]
    private void FixedUpdate()
    {
        Move();
    }

    [Client]
    private void SetMovement(Vector2 input)
    {
        prevInput = input;
    }

    [Client]
    private void ResetMovement()
    {
        prevInput = Vector3.zero;
    }
    [Client]
    private void Move()
    {
        Vector3 right = transform.right;
        Vector3 forward = transform.forward;
        right.y = 0f;
        forward.y = 0f;
        Vector3 movement = (right * prevInput.x + forward * prevInput.y).normalized;
        Vector3 velY = Vector3.up * rb.velocity.y;
        rb.velocity = movement * speed + velY;
    }
}
