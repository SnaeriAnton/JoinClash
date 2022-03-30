using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(HumanAnimator))]
public class HumanMover : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Transform _transform;
    [SerializeField] private HumanAnimator _animator;

    private Vector3 _bossPosition;
    private Vector3 _stopPosition;

    private void OnDisable()
    {
        _navMeshAgent.enabled = false;
    }

    private void Update()
    {
        _navMeshAgent.SetDestination(_bossPosition);
        //_transform.position = Vector3.MoveTowards(_transform.position, _bossPosition, 0.003f);
    }

    public void SetBossPosition(Vector3 bossPosition)
    {
        _navMeshAgent.enabled = true;
        _bossPosition = bossPosition;
        _animator.Run();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Boss>(out Boss boss))
        {
            //_bossPosition = Vector3.zero;
            //_stopPosition = _transform.position;
        }
    }
}
