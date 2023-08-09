using UnityEngine;
using UnityEngine.VFX;

public class PlayerFXManager : MonoBehaviour
{
    [SerializeField] public VisualEffect footStepFX;
    [SerializeField] public VisualEffect fallFX;

    [SerializeField] public ParticleSystem blade01;
    [SerializeField] public ParticleSystem blade02;
    [SerializeField] public ParticleSystem blade03;

    [SerializeField] public VisualEffect slash;
    [SerializeField] public VisualEffect heal;

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
    public void PlayBlade02() => blade02.Play();
    public void PlayBlade03() => blade03.Play();

    public void StopBlade()
    {
        blade01.Simulate(0);
        blade01.Stop();

        blade02.Simulate(0);
        blade02.Stop();

        blade03.Simulate(0);
        blade03.Stop();
    }

    public void Slash(Vector3 pos)
    {
        slash.transform.position = pos;
        slash.Play();
    }

    public void Heal()
    {
        heal.Play();
    }
}
