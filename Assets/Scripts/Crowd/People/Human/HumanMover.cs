using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(HumanAnimator))]
public class HumanMover : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Transform _transform;
    [SerializeField] private HumanAnimator _animator;

    private Vector3 _targetPosition;
    private bool _isBoss = false;

    private void OnEnable()
    {
        _navMeshAgent.enabled = true;
        _animator.Run();
    }

    private void OnDisable()
    {
        _navMeshAgent.enabled = false;
    }

    private void Update()
    {
        if (_isBoss == false)
        {
            _targetPosition = _transform.position + _transform.forward;
        }

        _transform.position = Vector3.MoveTowards(_transform.position, _targetPosition, 0.009f);
    }

    public void SetBossPosition(Vector3 bossPosition)
    {
        _navMeshAgent.enabled = true;
        _animator.Run();
        _targetPosition = bossPosition;
    }
}
