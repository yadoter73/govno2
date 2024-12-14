using UnityEngine;
using UnityEngine.AI;

public class SearchState : EnemyState
{
    private Vector3 _lastKnownPosition;
    private float _searchTimer;
    private float _searchRadius;
    private Vector3 _currentSearchPoint;

    public SearchState(EnemyAI enemy, Vector3 lastKnownPosition) : base(enemy)
    {
        _lastKnownPosition = lastKnownPosition;
        _searchTimer = enemy.SearchDuration;
        _searchRadius = enemy.PatrolRadius / 2f;

        SetNewSearchPoint();
    }

    public override void UpdateState()
    {
        if (_enemy.IsPlayerInFieldOfView())
        {
            _enemy.SwitchState(new ChaseState(_enemy));
            return;
        }

        _enemy.MoveTo(_currentSearchPoint);

        if (Vector3.Distance(_enemy.transform.position, _currentSearchPoint) < 7f)
        {
            SetNewSearchPoint();
        }

        _searchTimer -= Time.deltaTime;
        if (_searchTimer <= 0)
        {
            _enemy.SwitchState(new PatrolState(_enemy));
        }
    }

    private void SetNewSearchPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _searchRadius;
        randomDirection += _lastKnownPosition;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, _searchRadius, NavMesh.AllAreas))
        {
            _currentSearchPoint = hit.position;
        }
    }
}
