using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HumanDeath))]
public class HumanFighter : MonoBehaviour
{
    [SerializeField] private HumanDeath _death;

    private int _damage = 200;
    private Boss _boss;

    public UnityAction Won;

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

    public void OnWin()
    {
        Won?.Invoke();
        _boss.Died -= OnWin;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Boss>(out Boss boss) && _boss == null)
        {
            _boss = boss;
            _boss.Died += OnWin;
            StartCoroutine(Push());
        }
    }
}
