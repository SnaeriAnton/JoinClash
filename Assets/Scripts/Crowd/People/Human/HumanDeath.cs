using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HumanAnimator))]
[RequireComponent(typeof(HumanMover))]
[RequireComponent(typeof(CapsuleCollider))]
public class HumanDeath : MonoBehaviour
{
    [SerializeField] private Rigidbody[] _rigidbodies;
    [SerializeField] private Transform _transform;
    [SerializeField] private HumanAnimator _animator;
    [SerializeField] private GameObject _human;
    [SerializeField] private HumanMover _mover;
    [SerializeField] private CapsuleCollider _capsileCollider;

    private bool _isDeath = false;

    public bool IsDeath => _isDeath;

    public UnityAction Died;

    private void Awake()
    {
        SetKinematic(true);
    }

    private void Update()
    {
        Debug.DrawRay(_transform.position, new Vector3(_transform.forward.x * -1, 5f, _transform.forward.z * -1) * 0.5f, Color.red);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fall();
        }
    }

    public void ChekDeath(int health)
    {
        if (health < 0)
        {
            Diy();
        }
    }

    private void Fall()
    {
        SetKinematic(false);
        for (int i = 0; i < _rigidbodies.Length; i++)
        {
            int forceó = Random.Range(1, 12) * 10;
            _rigidbodies[i].AddForce(new Vector3(_transform.forward.x * -1, 5f, _transform.forward.z * -1) * (10 + forceó), ForceMode.Acceleration);
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
        _capsileCollider.enabled = false;
        _animator.enabled = false;
        _mover.enabled = false;
        Fall();
        _isDeath = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Arena>(out Arena arena) && _isDeath == true)
        {
            Destroy(_human);
        }
    }
}
