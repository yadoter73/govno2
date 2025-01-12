using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;
using System;

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

    [SerializeField] private MultiAimConstraint _multiAimConstraint;
    [SerializeField] private Rig _arms;
    [SerializeField] private Transform _head;
    private EnemyState _currentState;
    private float _distanceToPlayer;
    private Vector3 _lastKnownPosition;
    private bool _isMoving;

    public bool IsMoving => _isMoving;
    public float SearchDuration => _searchDuration;

    public event Action OnShoot;
    public event Action OnReload;
    public event Action OnReloadEnd;

    private void Start()
    {
        IsPlayerInFieldOfView();
        _currentState = new PatrolState(this);

    }

    private void Update()
    {
        _distanceToPlayer = Vector3.Distance(transform.position, Player.position);
        _isMoving = _agent.velocity.magnitude > _agent.speed / 4;
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

    public bool IsPlayerInFieldOfView()
    {
        _multiAimConstraint.weight = 0;
        Vector3 directionToPlayer = (Player.position - _head.position).normalized;
        float angle = Vector3.Angle(_head.forward, directionToPlayer);

        if (angle > _viewAngle / 2 || !(_distanceToPlayer <= _detectionRange))
            return false;

        if (Physics.Raycast(_head.position, directionToPlayer, out RaycastHit hit, _detectionRange, _wallsAndPlayerLayer))
        {
            if (hit.transform != Player)
                return false;
        }
        _multiAimConstraint.weight = 1;
        return true;
    }

    public void RotateTowardsPlayer()
    {
        Vector3 directionToPlayer = (Player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        targetRotation.x = transform.rotation.x;
        targetRotation.z = transform.rotation.z;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _agent.angularSpeed / 2);
    }

    public void ShootAtPlayer()
    {

        if (_weapon.CurrentAmmo <= 0)
        {
            _arms.weight = 0;
            OnReload?.Invoke();
            return;
        }
        OnShoot?.Invoke();
        StartCoroutine(AttackRoutine());
    }

    public void ReloadEnd()
    {
        _arms.weight = 1;
        _weapon.Reloaded();
        OnReloadEnd?.Invoke();
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
