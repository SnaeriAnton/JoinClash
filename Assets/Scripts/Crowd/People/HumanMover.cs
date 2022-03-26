using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanMover : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Transform _transform;
    [SerializeField] private HumanAnimator _humanAnimator;

    private Vector3 _bossPosition;
    private Vector3 _stopPosition;

    private void Start()
    {
        _bossPosition = Vector3.zero;
        _stopPosition = Vector3.zero;
    }

    private void Update()
    {
        if (_bossPosition != Vector3.zero)
        {
            _navMeshAgent.SetDestination(_bossPosition);
        }

        if (_stopPosition != Vector3.zero)
        {
            //_transform.position = _stopPosition;
        }
    }

    public void SetBossPosition(Vector3 bossPosition)
    {
        _navMeshAgent.enabled = true;
        _bossPosition = bossPosition;
        _humanAnimator.Run();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Boss>(out Boss boss))
        {
            _bossPosition = Vector3.zero;
            _stopPosition = _transform.position;
        }
    }
}
