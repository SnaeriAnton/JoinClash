using UnityEngine;

public class CrowdTracker : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Transform _transformMainZone;
    [SerializeField] private Crowd _crowd;

    private bool _finish = false;

    private void OnEnable()
    {
        _crowd.Finished += OnChangePositionCamer;
    }

    private void OnDisable()
    {
        _crowd.Finished -= OnChangePositionCamer;
    }

    private void Update()
    {
        if (_finish == false)
        {
            _transform.position = new Vector3(_transformMainZone.position.x, _transform.position.y, _transformMainZone.position.z + -2.175f);

        }

        if (_finish == true)
        {
            _transform.rotation = Quaternion.Euler(42.876f, 0, 0);
            _transform.position = new Vector3(0, _transform.position.y, _transform.position.z);

        }
    }

    private void OnChangePositionCamer()
    {
        _finish = true;
    }
}
