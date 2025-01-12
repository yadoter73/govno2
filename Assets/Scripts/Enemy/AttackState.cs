using UnityEngine;

public class AttackState : EnemyState
{
    public AttackState(EnemyAI enemy) : base(enemy) { }

    public override void UpdateState()
    {
        if (!_enemy.IsPlayerInAttackRange() || !_enemy.IsPlayerInFieldOfView())
        {
            _enemy.SwitchState(new ChaseState(_enemy));
            return;
        }

        _enemy.RotateTowardsPlayer();
        _enemy.MoveTo(_enemy.transform.position);
        _enemy.ShootAtPlayer();


    }
}
