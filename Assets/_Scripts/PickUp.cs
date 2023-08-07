using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public enum PickUpType
    {
        HEAL, COIN
    }

    public PickUpType type;
    public int value = 20;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.gameObject.GetComponent<Character>().PickUpItem(this);
            Destroy(gameObject);
        }
    }
}
