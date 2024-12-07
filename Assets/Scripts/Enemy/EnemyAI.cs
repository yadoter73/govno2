using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform Player;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Weapon _weapon;

    [Header("Patrol Settings")]
    public Vector3 PatrolCenter;
    public float PatrolRadius = 20f;
    public float PatrolWaitTime = 2f;

    [Header("Detection Settings")]
    [SerializeField] private float _detectionRange = 20f;
    [SerializeField] private float _attackRange = 10f;
    [SerializeField] private float _viewAngle = 90f;
    [SerializeField] private LayerMask _wallsAndPlayerLayer;
    [SerializeField] private float _searchDuration;

    private EnemyState _currentState;
    private float _distanceToPlayer;
    private Vector3 _lastKnownPosition;

    public float SearchDuration => _searchDuration;
    private void Start()
    {
        _currentState = new PatrolState(this);
    }

    private void Update()
    {
        _distanceToPlayer = Vector3.Distance(transform.position, Player.position);
        _currentState?.UpdateState();
        Debug.Log(_currentState.ToString());
    }

    public void SwitchState(EnemyState newState)
    {
        _currentState = newState;
    }

    public void MoveTo(Vector3 destination)
    {
        if (_agent != null && _agent.isActiveAndEnabled)
        {
            _agent.SetDestination(destination);
        }
    }

    public bool IsPlayerInAttackRange() => _distanceToPlayer <= _attackRange;

    public bool IsFacingPlayer()
    {
        Vector3 directionToPlayer = (Player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle <= 1;
    }

    public bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = (Player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (angle > _viewAngle / 2 || !(_distanceToPlayer <= _detectionRange))
            return false;

        if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, _detectionRange, _wallsAndPlayerLayer))
        {
            if (hit.transform != Player)
                return false;
        }

        return true;
    }

    public void RotateTowardsPlayer()
    {
        Vector3 directionToPlayer = (Player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _agent.angularSpeed / 2);
    }

    public void ShootAtPlayer()
    {
        if (_weapon.CurrentAmmo <= 0)
        {
            StartCoroutine(_weapon.Reload());
            return;
        }

        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        _weapon.Shoot(_weapon.FirePoint.position, _weapon.FirePoint.forward);
        yield return new WaitForSeconds(_weapon.FireRate);
    }

    public void UpdateLastKnownPosition(Vector3 position)
    {
        _lastKnownPosition = position;
    }
}
