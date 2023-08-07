using UnityEngine;
using UnityEngine.VFX;

public class EnemyVFXManager : MonoBehaviour
{
    [SerializeField] public VisualEffect footStepFX;
    [SerializeField] public VisualEffect smashFX;
    [SerializeField] public ParticleSystem beingHitFX;
    [SerializeField] public VisualEffect beingHitSplashFX;

    public void BurstFootStep()
    {
        if (footStepFX != null)
            footStepFX.SendEvent("OnPlay");
    }

    public void PlaySmash() 
    {
        if (smashFX != null)
            smashFX.Play(); 
    }

    public void BeingHit(Vector3 attackerPos)
    {
        //Debug.Log("Being Hit attackert Pos : " + attackerPos);
        Vector3 forceForward = transform.position - attackerPos;
        forceForward.Normalize();
        forceForward.y = 0;
        //Debug.Log(forceForward);
        beingHitFX.transform.rotation = Quaternion.LookRotation(forceForward);
        beingHitFX.Play();

        BeingHitSplashVFX();
    }

    private void BeingHitSplashVFX()
    {
        Vector3 splashPos = transform.position;
        splashPos.y += 2f;

        VisualEffect newSplashVFX = Instantiate(beingHitSplashFX, splashPos, Quaternion.identity) as VisualEffect;
        newSplashVFX.Play();

        // Destroy the gameobject not the Visual Effect component
        // Otherwise a empty game object will be laying in the hierarchy
        Destroy(newSplashVFX.gameObject, 5f);
    }
}
