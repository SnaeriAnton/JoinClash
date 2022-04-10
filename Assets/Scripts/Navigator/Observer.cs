using UnityEngine;

public class Observer : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Transform _transfromLookAt;

    private void Update()
    {
        _transform.LookAt(_transfromLookAt);
    }
}
