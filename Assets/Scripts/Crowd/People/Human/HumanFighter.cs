using System.Collections;
using UnityEngine;

[RequireComponent(typeof(HumanDeath))]
public class HumanFighter : MonoBehaviour
{
    [SerializeField] private HumanDeath _death;

    private int _damage = 0;
    private Boss _boss;

    private void Awake()
    {
        _boss = null;
        _death.Died += OnStopPush;
    }

    private void OnDisable()
    {
        _death.Died -= OnStopPush;
    }

    private void OnStopPush()
    {
        StopCoroutine(Push());
    }

    private IEnumerator Push()
    {
        while (true)
        {
            _boss.TakingDamage(_damage);
            yield return new WaitForSeconds(1f);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Boss>(out Boss boss))
        {
            _boss = boss;
            StartCoroutine(Push());
        }
    }
}
