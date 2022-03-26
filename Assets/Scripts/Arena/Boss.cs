using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private Transform _trandform;
    [SerializeField] private BossAnimator _bossAnimator;

    private float _health = 300;

    private Vector3 _defaultPosition;

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
        if (_health < 0)
        {
            _bossAnimator.Diying();
        }
    }

    public void TakingDamage(float damage)
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
