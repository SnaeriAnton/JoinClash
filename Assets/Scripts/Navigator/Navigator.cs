using UnityEngine;
using UnityEngine.Events;

public class Navigator : MonoBehaviour
{
    [SerializeField] private Transform _transformZone;
    [SerializeField] private SpriteRenderer _spriteRendereCircle;
    [SerializeField] private Transform _transform;
    [SerializeField] private SphereCollider _sphereCollider;
    [SerializeField] private Crowd _crowd;
    [SerializeField] private GameObject _finger;

    private float _radius = 0;
    private float _defaultRadius = 2.6f;

    public UnityAction Disabled;

    private void OnEnable()
    {
        _radius = _defaultRadius;
        _crowd.ChangedCrowd += OnSetRadius;
    }

    private void OnDisable()
    {
        _crowd.ChangedCrowd -= OnSetRadius;
    }

    public void Track(Vector3 newPosition, bool spriteEnable)
    {
        Vector3 position;
        if (newPosition == Vector3.zero)
        {
            position = new Vector3(_transformZone.position.x, _transform.position.y, _transformZone.position.z);
        }
        else
        {
            position = GetVectorMagnitude(newPosition, _transformZone.position);
        }
        _transform.position = new Vector3(position.x, _transform.position.y, position.z);
    }

    public void ActiveFinger(bool value)
    {
        _finger.SetActive(value);
    }

    private void EnableComponents(bool value)
    {
        _spriteRendereCircle.enabled = value;
        _sphereCollider.enabled = value;
        if (value == false)
        {
            Disabled?.Invoke();
        }
    }

    public Vector3 GetVectorMagnitude(Vector3 position, Vector3 centerCirclePosition)
    {
        Vector3 offset = position - centerCirclePosition;
        offset = new Vector3(offset.x, 0, offset.y);
        Vector3 vector = centerCirclePosition + Vector3.ClampMagnitude(offset, _radius);
        return vector;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Wall>(out Wall wall) == true || other.TryGetComponent<Zone>(out Zone people) == true)
        {
            EnableComponents(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnableComponents(true);
    }

    private void OnSetRadius(float radius)
    {
        _radius += radius;
        if (_radius <= _defaultRadius)
        {
            _radius = _defaultRadius;
        }
    }
}
