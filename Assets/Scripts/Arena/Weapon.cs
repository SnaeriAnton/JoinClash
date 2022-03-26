using UnityEngine;

public class Weapon : MonoBehaviour
{
    private float _damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Human>(out Human human))
        {
            human.TakingDamage(_damage);
        }
    }
}
