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

    public bool IsDeath => _isDeath;

    public UnityAction Died;

    private void Awake()
    {
        SetKinematic(true);
    }

    private void Update()
    {
        Debug.DrawRay(_transform.position, new Vector3(_transform.forward.x * -1, 3f, _transform.forward.z * -1) * 0.5f, Color.red);
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
            int forceó = Random.Range(1, 5) * 10;
            _rigidbodies[i].AddForce(new Vector3(_transform.forward.x * -1, 3f, _transform.forward.z * -1) * (50 + forceó), ForceMode.Acceleration);
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
