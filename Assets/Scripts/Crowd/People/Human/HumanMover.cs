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
    private Vector3 _fixPosition;
    private float _speed = 3;

    private void OnEnable()
    {
        _navMeshAgent.enabled = true;
        _animator.Run();
        _fixPosition = Vector3.zero;
    }

    private void OnDisable()
    {
        _navMeshAgent.enabled = false;
    }

    private void Update()
    {
        if (_isBoss == false)
        {
            //_transform.position = Vector3.MoveTowards(_transform.position, _targetPosition, 0.009f);
            _transform.position = Vector3.MoveTowards(_transform.position, _targetPosition, _speed * Time.deltaTime);
        }
        else
        {
            _transform.position = _fixPosition;
        }

    }

    public void SetBossPosition(Vector3 bossPosition)
    {
        _navMeshAgent.enabled = true;
        _animator.Run();
        _targetPosition = bossPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Boss>(out Boss boss))
        {
            _isBoss = true;
            _fixPosition = _transform.position;
        }
    }
}
