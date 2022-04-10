using System.Collections;
using UnityEngine;

[RequireComponent(typeof(HumanDeath))]
[RequireComponent(typeof(AudioSource))]
public class HumanFighter : MonoBehaviour
{
    [SerializeField] private HumanDeath _death;
    [SerializeField] private AudioClip[] _audioClips;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Human _human;

    private int _damage = 1;
    private Boss _boss;
    private bool _isWin = false;

    private void Awake()
    {
        _boss = null;
        _human.Won += OnWin;
        _death.Died += OnStopPush;
    }

    private void OnDisable()
    {
        _human.Won -= OnWin;
        _death.Died -= OnStopPush;
        OnStopPush();
    }

    private void OnStopPush()
    {
        StopCoroutine(Push());
        _audioSource.enabled = false;
    }

    private IEnumerator Push()
    {
        while (_isWin == false)
        {
            _boss.TakingDamage(_damage);
            int random = Random.Range(0, _audioClips.Length);
            _audioSource.clip = _audioClips[random];
            _audioSource.Play();
            yield return new WaitForSeconds(1f);
        }
    }

    private void OnWin()
    {
        _isWin = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Boss>(out Boss boss) && _boss == null)
        {
            _boss = boss;
            StartCoroutine(Push());
        }
    }
}
