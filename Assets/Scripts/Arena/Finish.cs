using UnityEngine;

public class Finish : MonoBehaviour 
{
    [SerializeField] private Transform _transformBoss;
    [SerializeField] private Boss _boss;
    [SerializeField] private ParticleSystem[] _particleSystems;
    [SerializeField] private AudioSource _audioSource;

    public Transform PositionBoss => _transformBoss;
    public Boss Boss => _boss;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Crowd>(out Crowd crowd))
        {
            for (int i = 0; i < _particleSystems.Length; i++)
            {
                _particleSystems[i].Play();
            }
            _audioSource.Play();
        }
    }
}
