using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Mirror;
using UnityEngine;

public class Bullet : NetworkBehaviour
{

    public GameObject source;
    public override void OnStartServer()
    {
        Invoke(nameof(Destroy), 5f);
    }

    [Server]
    private void Destroy()
    {
        NetworkServer.Destroy(gameObject);
    }


    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            other.GetComponent<Health>().DecreaseHealth(20);
        }
        Destroy();
    }

    
}
