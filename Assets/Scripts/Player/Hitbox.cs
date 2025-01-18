using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public Health Health;

    public void OnRaycastHit(float damage)
    {
        Health.TakeDamage(damage);
    }
}