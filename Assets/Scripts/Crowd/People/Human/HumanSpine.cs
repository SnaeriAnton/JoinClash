using UnityEngine;
using UnityEngine.Events;

public class HumanSpine : MonoBehaviour
{
    [SerializeField] private HumanDeath _death;
    [SerializeField] private GameObject _humanSkin;
    [SerializeField] private GameObject _humanSkilet;

    public UnityAction<Vector3> Fell;

    private void OnTriggerEnter(Collider other)
    {
        //if (other.TryGetComponent<Arena>(out Arena arena) && _death.IsDeath == true)
        //{
        //    Fell?.Invoke(other.transform.position);
        //    _human.SetActive(false);
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Arena>(out Arena arena) && _death.IsDeath == true)
        {
            Fell?.Invoke(collision.contacts[0].point);
            _humanSkin.SetActive(false);
            _humanSkilet.SetActive(false);
        }
    }
}
