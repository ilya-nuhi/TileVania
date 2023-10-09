using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] int coinScore = 100;
    [SerializeField] AudioClip coinPickupSFX;
    bool wasCollected = false;
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag=="Player" && !wasCollected){
            wasCollected = true;
            AudioSource.PlayClipAtPoint(coinPickupSFX,Camera.main.transform.position);
            FindObjectOfType<GameSession>().ProcessPoints(coinScore);
            //gameObject.SetActive(false);
            Destroy(gameObject);
            
        }
    }

    
}
