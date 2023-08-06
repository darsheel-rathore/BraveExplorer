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
        footStepFX.SendEvent("OnPlay");
    }

    public void PlaySmash() 
    {
        Debug.Log("Snmash");
        smashFX.Play(); 
    }

    public void BeingHit(Vector3 attackerPos)
    {
        Vector3 forceForward = transform.position - attackerPos;
        forceForward.Normalize();
        forceForward.y = 0;

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
        Destroy(newSplashVFX, 5f);
    }
}
