using UnityEngine;

public class PickUp : MonoBehaviour
{
    public ParticleSystem collectiveVFX;

    public enum PickUpType
    {
        HEAL, COIN
    }

    public PickUpType type;
    public int value = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<Character>().PickUpItem(this);

            if (collectiveVFX != null)
            {
                Instantiate(collectiveVFX, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}
