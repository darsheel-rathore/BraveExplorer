using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_02_Shoot : MonoBehaviour
{
    public Transform shootingPoint;
    public GameObject damageOrb;
    public Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    private void Update()
    {
        character.RotateToTarget();
    }

    public void ShootTheDamageOrb()
    {
        Instantiate(damageOrb, shootingPoint.position, Quaternion.LookRotation(shootingPoint.forward));
    }
}
