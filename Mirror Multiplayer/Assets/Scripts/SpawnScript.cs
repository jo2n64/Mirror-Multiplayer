using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnScript : NetworkBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        NetworkManager.startPositions.Add(transform);    
    }

}
