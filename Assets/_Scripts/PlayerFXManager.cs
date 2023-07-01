using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerFXManager : MonoBehaviour
{
    [SerializeField] public VisualEffect footStepFX;
    [SerializeField] public VisualEffect fallFX;

    public void Update_FootStep(bool state)
    {
        if (state)
            footStepFX.Play();
        else
            footStepFX.Stop();
    }

    public void Update_Fall(bool state)
    {
        if(state)
            fallFX.Play();
        else
            fallFX.Stop();
    }
}
