using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private EnemyAI _enemyAI;
    private Animator _animator;
    private Health _health;
    private void Start()
    {
        _enemyAI = GetComponent<EnemyAI>();
        _health = GetComponent<Health>();
        _animator = GetComponent<Animator>();
        _enemyAI.OnShoot += ShootAnimation;
        _enemyAI.OnReload += ReloadAnimationStart;
        _enemyAI.OnReloadEnd += ReloadAnimationEnd;
        _health.OnHit += HitAnimation;
    }

    private void Update()
    {
        _animator.SetBool("IsRunning", _enemyAI.IsMoving);
    }

    private void ShootAnimation()
    {
        _animator.SetTrigger("Shoot");
    }

    private void ReloadAnimationStart()
    {
        _animator.SetBool("Reload", true);
    }
    private void ReloadAnimationEnd()
    {
        _animator.SetBool("Reload", false);
    }
    private void HitAnimation()
    {
        _animator.SetTrigger("Hit");
    }
}
