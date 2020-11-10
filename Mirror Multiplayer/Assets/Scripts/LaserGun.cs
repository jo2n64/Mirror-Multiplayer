using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LaserGun : NetworkBehaviour
{
    public Transform laserTransform, cameraTransform;
    public LineRenderer laser;
    public AudioClip shootSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [ClientCallback]
    // Update is called once per frame
    void Update()
    {
        laserTransform.rotation = cameraTransform.rotation;
        if(isLocalPlayer && Input.GetMouseButtonDown(0))
        {
            CmdShoot();
            AudioSource.PlayClipAtPoint(shootSound, laserTransform.position, 0.5f);
        }
    }


    [Command]
    void CmdShoot()
    {
        Ray ray = new Ray(laserTransform.position, laserTransform.forward);
        if(Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            var player = hit.collider.gameObject.GetComponent<Health>();
            if (player)
            {
                player.DecreaseHealth(100);
            }
            RpcDrawLaser(laserTransform.position, hit.point);
        }
        else
        {
            RpcDrawLaser(laserTransform.position, laserTransform.position + laserTransform.forward * 100f);
        }
    }

    [ClientRpc]
    void RpcDrawLaser(Vector3 start, Vector3 end)
    {
        StartCoroutine(LaserFlash(start, end));
    }

    

   

    IEnumerator LaserFlash(Vector3 start, Vector3 end)
    {
        laser.SetPosition(0, start);
        laser.SetPosition(1, end);
        yield return new WaitForSeconds(0.3f);
        laser.SetPosition(0, Vector3.zero);
        laser.SetPosition(1, Vector3.zero);
    }
}
