using UnityEngine;

public class Looking : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Transform _transfromLookAt;

    private void Update()
    {
        _transform.LookAt(_transfromLookAt);
    }
}
