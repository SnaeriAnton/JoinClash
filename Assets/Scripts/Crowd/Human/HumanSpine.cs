using UnityEngine;
using UnityEngine.Events;

public class HumanSpine : MonoBehaviour
{
    [SerializeField] private HumanDeath _death;
    [SerializeField] private GameObject _humanSkin;
    [SerializeField] private GameObject _humanSkilet;
    [SerializeField] private AudioSource _audioSource;

    public UnityAction<Vector3> Fell;

    private void Touch(Vector3 contact)
    {
        Fell?.Invoke(contact);
        _humanSkin.SetActive(false);
        _humanSkilet.SetActive(false);
        float delay = Random.Range(0f, 0.3f);
        _audioSource.PlayDelayed(delay);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Arena>(out Arena arena) && _death.IsDeath == true)
        {
            Touch(collision.contacts[0].point);
        }
    }
}
