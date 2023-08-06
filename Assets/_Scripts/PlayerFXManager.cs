using UnityEngine;
using UnityEngine.VFX;

public class PlayerFXManager : MonoBehaviour
{
    [SerializeField] public VisualEffect footStepFX;
    [SerializeField] public VisualEffect fallFX;
    [SerializeField] public ParticleSystem blade01;
    [SerializeField] public VisualEffect slash;

    public void Update_FootStep(bool state)
    {
        if (state)
            footStepFX.Play();
        else
            footStepFX.Stop();
    }

    public void Update_Fall(bool state)
    {
        if (state)
            fallFX.Play();
        else
            fallFX.Stop();
    }

    public void PlayBlade01() => blade01.Play();

    public void Slash(Vector3 pos)
    {
        slash.transform.position = pos;
        slash.Play();
    }
}
