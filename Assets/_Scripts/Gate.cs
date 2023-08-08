using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] public GameObject gateVisual;
    [SerializeField] public float openDuration = 2f;
    [SerializeField] public float openTargetY = -1.5f;
    private Collider collider;

    private void Awake()
    {
        collider = GetComponent<Collider>();
    }

    public void Open()
    {
        StartCoroutine(OpenGateAnimation());
    }

    IEnumerator OpenGateAnimation()
    {
        float currentOpenDuration = 0;
        Vector3 startPos = gateVisual.transform.position;
        Vector3 endPos = gateVisual.transform.position + Vector3.up * openTargetY;

        while(currentOpenDuration < openDuration)
        {
            currentOpenDuration += Time.deltaTime;
            gateVisual.transform.position = Vector3.Lerp(startPos, endPos, currentOpenDuration / openDuration);
            yield return null;
        }

        collider.enabled = false;
    }

}
