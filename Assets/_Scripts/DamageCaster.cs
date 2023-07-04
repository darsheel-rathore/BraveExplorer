using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCaster : MonoBehaviour
{
    private Collider damageCasterCollider;
    public int damage = 30;
    public string TargetTag;

    private List<Collider> damagedTargetList;

    private void Awake()
    {
        damageCasterCollider = GetComponent<Collider>();
        damageCasterCollider.enabled = false;
        damagedTargetList = new List<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == TargetTag && !damagedTargetList.Contains(other))
        {
            Character target = GetComponent<Character>();
            if(target != null)
            {
                target.ApplyDamage(damage);
            }

            damagedTargetList.Add(other);
        }
    }

    public void EnableDamageCaster()
    {
        damagedTargetList.Clear();
        damageCasterCollider.enabled = true;
    }

    public void DisableDamageCaster()
    {
        damagedTargetList.Clear();
        damageCasterCollider.enabled = false;
    }
}
