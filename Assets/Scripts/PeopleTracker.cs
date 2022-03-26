using UnityEngine;

public class PeopleTracker : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Transform _transformMainZone;


    private void Update()
    {
        _transform.position = new Vector3(_transformMainZone.position.x, _transform.position.y, _transformMainZone.position.z + -2.175f);
    }
}
