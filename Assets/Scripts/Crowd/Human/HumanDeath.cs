using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HumanAnimator))]
[RequireComponent(typeof(HumanMover))]
[RequireComponent(typeof(CapsuleCollider))]
public class HumanDeath : MonoBehaviour
{
    [SerializeField] private Rigidbody[] _rigidbodies;
    [SerializeField] private Rigidbody _rigidbodie;
    [SerializeField] private Transform _transform;
    [SerializeField] private HumanAnimator _animator;
    [SerializeField] private HumanMover _mover;
    [SerializeField] private CapsuleCollider _capsileCollider;
    [SerializeField] private HumanFighter _fighter;

    private bool _isDeath = false;
    private float _force = 50;
    private int _minForce = 1;
    private int _maxForce = 5;
    private int _factorForce = 10;
    private float _offsetForward = -1f;
    private float _directionUp = 3f;

    public bool IsDeath => _isDeath;

    public UnityAction Died;

    private void Awake()
    {
        SetKinematic(true);
    }

    public void ChekDeath(int health)
    {
        if (health <= 0)
        {
            Diy();
        }
    }

    private void Fall()
    {
        SetKinematic(false);
        for (int i = 0; i < _rigidbodies.Length; i++)
        {
            int force = Random.Range(_minForce, _maxForce) * _factorForce;
            _rigidbodies[i].AddForce(new Vector3(_transform.forward.x * _offsetForward, _directionUp, _transform.forward.z * _offsetForward) * (_force + force), ForceMode.Acceleration);
        }
    }

    private void SetKinematic(bool value)
    {
        for (int i = 0; i < _rigidbodies.Length; i++)
        {
            _rigidbodies[i].isKinematic = value;
        }
    }

    private void Diy()
    {
        Died?.Invoke();
        _rigidbodie.isKinematic = true;
        _capsileCollider.enabled = false;
        _fighter.enabled = false;
        _animator.enabled = false;
        _mover.enabled = false;
        Fall();
        _isDeath = true;
    }
}