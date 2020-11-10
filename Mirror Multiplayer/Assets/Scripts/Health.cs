using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class Health : NetworkBehaviour
{
    [Header("Stuff")] 
    [SyncVar]
    [SerializeField] private int healthAmount = 100;

    public override void OnStartAuthority()
    {
        enabled = true;
    }

    public int HealthAmount
    {
        get => healthAmount;
        set => healthAmount = value;
    }
    public void DecreaseHealth(int value)
    {
        RpcDecreaseHealth(value);
        
    }

    [ClientRpc]
    void RpcDecreaseHealth(int value)
    {
        HealthAmount -= value;
    }

    private void Update()
    {
        if(HealthAmount <= 0)
        {
            HealthAmount = 100;
            StartCoroutine(Respawn(gameObject));
        }
    }

    [Server]
    IEnumerator Respawn(GameObject obj)
    {
        NetworkServer.UnSpawn(obj);
        Transform newPos = NetworkManager.singleton.GetStartPosition();
        obj.transform.position = newPos.position;
        obj.transform.rotation = newPos.rotation;
        yield return new WaitForSeconds(1f);
        NetworkServer.Spawn(obj, NetworkServer.localConnection);
    }

    //[Server]
    //IEnumerator Respawn(GameObject obj)
    //{
    //    NetworkServer.UnSpawn(obj);
    //    Transform newPos = NetworkManager.singleton.GetStartPosition();
    //    obj.transform.position = newPos.position;
    //    obj.transform.rotation = newPos.rotation;
    //    yield return new WaitForSeconds(1f);
    //    NetworkServer.Spawn(obj, obj);
    //}

}
