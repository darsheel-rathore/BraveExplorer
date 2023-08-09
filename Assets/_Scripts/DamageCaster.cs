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
        if (other.tag == TargetTag && !damagedTargetList.Contains(other))
        {
            Character target = other.GetComponent<Character>();
            if (target != null)
            {
                target.ApplyDamage(damage, transform.parent.position);

                PlayerFXManager playerFXManager = transform.parent.GetComponent<PlayerFXManager>();

                if (playerFXManager != null)
                {
                    // Calculate the original position for the boxcast (with adding offset value)
                    Vector3 originalPos = transform.position + (-damageCasterCollider.bounds.extents.z * transform.forward);

                    // Perform a boxcast
                    bool isHit = Physics.BoxCast(
                                                originalPos,
                                                damageCasterCollider.bounds.extents / 2,
                                                transform.forward,
                                                out RaycastHit hit,
                                                transform.rotation,
                                                damageCasterCollider.bounds.extents.z,
                                                1 << 6);

                    if (isHit)
                    {
                        playerFXManager.Slash(hit.point + new Vector3(0, 0.5f, 0));
                    }
                }


            }

            damagedTargetList.Add(other);
        }
    }

    public void EnableDamageCaster()
    {
        damagedTargetList.Clear();
        damageCasterCollider.enabled = true;
        Debug.Log("Enabled Damage Caster");
    }

    public void DisableDamageCaster()
    {
        damagedTargetList.Clear();
        damageCasterCollider.enabled = false;
        Debug.Log("Disable Damage caster");
    }

    //public void OnDrawGizmos()
    //{
    //    if (damageCasterCollider == null)
    //        damageCasterCollider = GetComponent<Collider>();

    //    // Calculate the original position for the boxcast (with adding offset value)
    //    Vector3 originalPos = transform.position + (-damageCasterCollider.bounds.extents.z * transform.forward);

    //    // Perform a boxcast
    //    bool isHit = Physics.BoxCast(
    //                                originalPos,
    //                                damageCasterCollider.bounds.extents / 2,
    //                                transform.forward,
    //                                out RaycastHit hit,
    //                                transform.rotation,
    //                                damageCasterCollider.bounds.extents.z,
    //                                1 << 6);

    //    if (isHit)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawWireSphere(hit.point, 0.3f);
    //    }

    //}
}
