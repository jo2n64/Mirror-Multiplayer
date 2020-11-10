using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Mirror;
using Mirror.Examples.Additive;
using UnityEngine;

public class PlayerShootScript : NetworkBehaviour
{
    // Start is called before the first frame update
    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private CinemachineVirtualCamera vCam;
    [SerializeField] private AudioClip shootSound;
    
    [Header("Properties")]
    [SerializeField] private float bulSpeed = 20f, shootDelay = 0.05f;
    [SerializeField] private bool isShooting, canShoot;

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
        Controls.Player.Shoot.performed += ctx => SetShoot();
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
    private void Update()
    {
        if (isShooting && canShoot)
        {
            Shoot();
            canShoot = false;
        }
        else if (!canShoot)
        {
            StartCoroutine(ResetShooting());
        }
        isShooting = false;
    }

    [Client]
    private void SetShoot()
    {
        isShooting = true;
    }

    [Client]
    void Shoot()
    {
        CmdShoot(vCam.transform.rotation * Vector3.forward);
    }

    [Command]
    private void CmdShoot(Vector3 rot)
    {
        if (bulletPrefab != null)
        {
            GameObject bul = Instantiate(bulletPrefab, vCam.transform.position + vCam.transform.forward * 2f, Quaternion.Euler(rot));
            bul.GetComponent<Bullet>().source = gameObject;
            NetworkServer.Spawn(bul);
            RpcShoot(bul, rot);
        }
    }


    [ClientRpc]
    void RpcShoot(GameObject instance, Vector3 rot)
    {
        if (vCam != null)
        {
            AudioSource.PlayClipAtPoint(shootSound, transform.position);
            instance.GetComponent<Rigidbody>().velocity = rot * bulSpeed;
            Debug.Log(rot);
        }
    }
    
    [Client]
    private IEnumerator ResetShooting()
    {
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }
}
