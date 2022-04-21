using UnityEngine;

public class Priority : MonoBehaviour
{
    private int _priorityNumber = 0;

    private void ChangePriorityNumber()
    {
        _priorityNumber++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Human>(out Human human))
        {
            human.SetPriority(_priorityNumber);
            ChangePriorityNumber();
        }
    }
}
