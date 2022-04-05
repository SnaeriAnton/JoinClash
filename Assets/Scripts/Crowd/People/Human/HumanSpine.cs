using UnityEngine;
using UnityEngine.Events;

public class HumanSpine : MonoBehaviour
{
    [SerializeField] private HumanDeath _death;
    [SerializeField] private GameObject _humanSkin;
    [SerializeField] private GameObject _humanSkilet;
    [SerializeField] private AudioSource _audioSource;

    public UnityAction<Vector3> Fell;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Arena>(out Arena arena) && _death.IsDeath == true)
        {
            Fell?.Invoke(collision.contacts[0].point);
            _humanSkin.SetActive(false);
            _humanSkilet.SetActive(false);
            float delay = Random.Range(0f, 0.3f);
            _audioSource.PlayDelayed(delay);
        }
    }
}
