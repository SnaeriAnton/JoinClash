using UnityEngine;

public class CrowdTracker : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Transform _transformPlayZone;
    [SerializeField] private Crowd _crowd;

    private bool _finish = false;
    private float _speed = 1f;
    private float _speedTracker = 5f;
    private float _offsetFromHuman = -2.46f;
    private float _finishRotation = 42.884f;
    private Vector3 _finishPosition = new Vector3(0, 4.61f, 27.43f);

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
        _transform.position = Vector3.Lerp(_transform.position, new Vector3(_transformPlayZone.position.x, _transform.position.y, _transformPlayZone.position.z + _offsetFromHuman), _speedTracker * Time.deltaTime);
    }

    private void SetFinishPosition()
    {
        _transform.rotation = Quaternion.Lerp(_transform.rotation, Quaternion.Euler(_finishRotation, 0, 0), _speed * Time.deltaTime);
        _transform.position = Vector3.Lerp(_transform.position, _finishPosition, _speed * Time.deltaTime);
    }

    private void OnChangePositionCamer()
    {
        _finish = true;
    }
}
