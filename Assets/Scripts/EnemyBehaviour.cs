using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    private Transform _patrolRouteTransform;
    [SerializeField] private List<Transform> locations;

    private Transform _playerTransform;

    private NavMeshAgent _navMeshAgent;

    private int _lives = 3;

    private enum Action
    {
        Harassment,
        Patrol
    }

    private Action _currentAction = Action.Patrol;

    private int EnemyLives
    {
        get => _lives;

        set
        {
            _lives = value;
            if (_lives > 0) return;
            Destroy(this.gameObject);
        }
    }


    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _patrolRouteTransform = GameObject.FindGameObjectWithTag("PatrolRoute").GetComponent<Transform>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        InitializePatrolRoute();
        MoveToNextPatrolLocation();
    }

    private void Update()
    {
        if (!(_navMeshAgent.remainingDistance < 0.1f & !_navMeshAgent.pathPending)) return;
        MoveToNextPatrolLocation();
        MoveToPlayer();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Bullet")) return;
        EnemyLives -= 1;
    }

    private void InitializePatrolRoute()
    {
        foreach (Transform child in _patrolRouteTransform)
        {
            locations.Add(child);
        }
    }

    private void MoveToNextPatrolLocation()
    {
        if (locations.Count == 0) return;
        if (_currentAction != Action.Patrol) return;
        _navMeshAgent.destination = locations[Random.Range(0, 4)].position;
    }

    private void MoveToPlayer()
    {
        if (_currentAction == Action.Harassment)
        {
            _navMeshAgent.destination = _playerTransform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _currentAction = Action.Harassment;
        MoveToPlayer();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _currentAction = Action.Patrol;
        MoveToNextPatrolLocation();
    }
}