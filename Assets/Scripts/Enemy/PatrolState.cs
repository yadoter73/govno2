using UnityEngine;
using UnityEngine.AI;

public class PatrolState : EnemyState
{
    private Vector3 _patrolDestination;
    private float _patrolWaitTimer;
    private bool _isWaiting;

    public PatrolState(EnemyAI enemy) : base(enemy)
    {
        SetRandomPatrolDestination();
    }

    public override void UpdateState()
    {
        if (_enemy.IsPlayerInFieldOfView())
        {
            _enemy.SwitchState(new ChaseState(_enemy));
            return;
        }

        if (_isWaiting)
        {
            _patrolWaitTimer -= Time.deltaTime;
            if (_patrolWaitTimer <= 0)
            {
                _isWaiting = false;
                SetRandomPatrolDestination();
            }
        }
        else
        {
            _enemy.MoveTo(_patrolDestination);
            if (Vector3.Distance(_enemy.transform.position, _patrolDestination) < 7)
            {
                _isWaiting = true;
                _patrolWaitTimer = Random.Range(0f, _enemy.PatrolWaitTime);
            }
        }
    }

    private void SetRandomPatrolDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _enemy.PatrolRadius;
        randomDirection += _enemy.PatrolCenter;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, _enemy.PatrolRadius, NavMesh.AllAreas))
        {
            _patrolDestination = hit.position;
            _enemy.MoveTo(_patrolDestination);
        }
    }
}
