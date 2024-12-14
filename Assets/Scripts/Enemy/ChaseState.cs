using UnityEngine;

public class ChaseState : EnemyState
{
    public ChaseState(EnemyAI enemy) : base(enemy) { }

    public override void UpdateState()
    {
        if (_enemy.IsPlayerInAttackRange())
        {
            _enemy.SwitchState(new AttackState(_enemy));
            return;
        }

        if (!_enemy.IsPlayerInFieldOfView())
        {
            _enemy.SwitchState(new SearchState(_enemy, _enemy.Player.position));
            return;
        }

        _enemy.MoveTo(_enemy.Player.position);
    }
}
