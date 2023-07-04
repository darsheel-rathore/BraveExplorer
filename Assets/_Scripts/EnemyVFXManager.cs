using UnityEngine;
using UnityEngine.VFX;

public class EnemyVFXManager : MonoBehaviour
{
    [SerializeField] public VisualEffect footStepFX;
    [SerializeField] public VisualEffect smashFX;

    public void BurstFootStep()
    {
        footStepFX.SendEvent("OnPlay");
    }

    public void PlaySmash() 
    {
        Debug.Log("Snmash");
        smashFX.Play(); 
    }
}
