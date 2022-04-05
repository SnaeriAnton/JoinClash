using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    private int _damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Human>(out Human human))
        {
            _audioSource.Play();
            human.TakingDamage(_damage);
        }
    }
}
