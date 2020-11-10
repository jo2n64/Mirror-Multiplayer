using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NetworkGameManager : MonoBehaviour
{
    public TMP_Text healthText;
    public Health localHealth;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(localHealth == null) FindLocalPlayer();
        if(localHealth != null && healthText != null)
        {
            healthText.text = localHealth.HealthAmount.ToString();
            
        }
    }

    void FindLocalPlayer()
    {
        if (ClientScene.localPlayer == null) return;

        localHealth = ClientScene.localPlayer.GetComponent<Health>();
    }
}
