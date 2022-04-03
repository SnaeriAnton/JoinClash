using UnityEngine;
using UnityEngine.Events;

public class Boss : MonoBehaviour
{
    [SerializeField] private Transform _trandform;
    [SerializeField] private BossAnimator _bossAnimator;

    private int _health = 300;
    private Vector3 _defaultPosition;

    public UnityAction Died;

    private void Start()
    {
        _defaultPosition = _trandform.position;
    }

    private void Update()
    {
        _trandform.position = _defaultPosition;
    }

    private void ChekDeath()
    {
        if (_health <= 0)
        {
            _bossAnimator.Diying();
            Died?.Invoke();
        }
    }

    public void TakingDamage(int damage)
    {
        _health -= damage;
        ChekDeath();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Human>(out Human human))
        {
            _bossAnimator.Attack();
        }
    }
}
