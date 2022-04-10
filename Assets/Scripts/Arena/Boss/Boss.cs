using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BossAnimator))]
[RequireComponent(typeof(AudioSource))]
public class Boss : MonoBehaviour
{
    [SerializeField] private Transform _trandform;
    [SerializeField] private BossAnimator _bossAnimator;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private CapsuleCollider _capsuleCollider;
    [SerializeField] private AudioSource _audioSource;

    private int _health = 300;
    private int _currentHelath = 0;
    private Vector3 _defaultPosition;
    private bool _death = false;

    public UnityAction Died;
    public UnityAction<float, int> ChangedHealth;

    private void Start()
    {
        _currentHelath = _health;
        _defaultPosition = _trandform.position;
    }

    private void Update()
    {
        StayOnPlace();
    }

    private void StayOnPlace()
    {
        _trandform.position = _defaultPosition;
    }

    private void ChekDeath()
    {
        if (_currentHelath <= 0 && _death == false)
        {
            _audioSource.Play();
            _capsuleCollider.enabled = false;
            _particleSystem.Stop();
            _bossAnimator.Diying();
            Died?.Invoke();
            _death = true;
        }
    }

    public void TakingDamage(int damage)
    {
        _currentHelath -= damage;
        ChangedHealth?.Invoke(damage, _health);
        ChekDeath();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Human>(out Human human))
        {
            _bossAnimator.Attack();
            _particleSystem.Play();
        }
    }
}
