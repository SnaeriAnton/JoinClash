using UnityEngine;

public class HumanSpine : MonoBehaviour
{
    [SerializeField] private HumanDeath _death;
    [SerializeField] private GameObject _human;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Arena>(out Arena arena) && _death.IsDeath == true)
        {
            Destroy(_human);
        }
    }
}
