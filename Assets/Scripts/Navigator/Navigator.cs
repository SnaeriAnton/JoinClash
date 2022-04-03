using UnityEngine;
using UnityEngine.Events;

public class Navigator : MonoBehaviour
{
    [SerializeField] private Transform _transformZone;
    [SerializeField] private SpriteRenderer _spriteRendereCircle;
    [SerializeField] private Transform _transform;
    [SerializeField] private SphereCollider _sphereCollider;

    private float _radius = 1.8f;

    public UnityAction Enabled;
    public UnityAction Disabled;

    private void OnEnable()
    {
        Enabled?.Invoke();
    }

    private void OnDisable()
    {
        EnableComponents(true);
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
}
