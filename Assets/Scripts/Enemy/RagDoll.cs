using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    public Rigidbody[] rbs;

    private void Awake()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
    }

    public void EnableRagdoll()
    {
        foreach (var rb in rbs)
        {
            if (rb.gameObject.layer != 3)
            {
                _animator.enabled = false;
                rb.isKinematic = false;
            }
        }
    }

    public void DisableRagdoll()
    {
        foreach (var rb in rbs)
        {
            if (rb.gameObject.layer != 3)
            {
                _animator.enabled = true;
                rb.isKinematic = true;
            }
        }
    }
}