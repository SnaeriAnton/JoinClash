using UnityEngine;

public class CrowdTracker : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Transform _transformPlayZone;
    [SerializeField] private Crowd _crowd;

    private bool _finish = false;
    private float _speed = 1;

    private void OnEnable()
    {
        _crowd.Finished += OnChangePositionCamer;
    }

    private void OnDisable()
    {
        _crowd.Finished -= OnChangePositionCamer;
    }

    private void LateUpdate()
    {
        if (_finish == false)
        {
            LookAt();
        }

        if (_finish == true)
        {
            SetFinishPosition();
        }
    }

    private void LookAt()
    {
        _transform.position = new Vector3(_transformPlayZone.position.x, _transform.position.y, _transformPlayZone.position.z + -1.497f);
    }

    private void SetFinishPosition()
    {
        _transform.rotation = Quaternion.Lerp(_transform.rotation, Quaternion.Euler(42.876f, 0, 0), _speed * Time.deltaTime);
        _transform.position = Vector3.Lerp(_transform.position, new Vector3(0, 5.637f, 16.889f), _speed * Time.deltaTime);
    }

    private void OnChangePositionCamer()
    {
        _finish = true;
    }
}
