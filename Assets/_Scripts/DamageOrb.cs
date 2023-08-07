using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOrb : MonoBehaviour
{
    public float speed = 2f;
    public int damage = 10;
    public ParticleSystem hitVFX;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody> ();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            // Reduce the health
            other.gameObject.GetComponent<Character>().ApplyDamage(damage, transform.position);
        }
        // Show VFX
        var VFX = Instantiate(hitVFX, transform.position, Quaternion.identity);

        // Delete the game object
        Destroy (VFX, 2f);

        // Destroy this game object
        Destroy(gameObject);
    }
}
