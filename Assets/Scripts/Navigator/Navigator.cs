using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    [SerializeField] private Transform _transformZone;
    [SerializeField] private SpriteRenderer _spriteRendereCircle;
    [SerializeField] private Transform _transform;
    [SerializeField] private SphereCollider _sphereCollider;

    private float _radius = 1.8f;
    private bool _isEnable = false;

    public void Track(Vector3 newPosition, bool spriteEnable)
    {
        Vector3 position;
        ChangeSpriteRenderer(spriteEnable);
        if (newPosition == Vector3.zero)
        {
            position = new Vector3(_transformZone.position.x, _transform.position.y, _transformZone.position.z);
            ChangeSpriteRenderer(spriteEnable);
        }
        else
        {
            position = GetVectorMagnitude(newPosition, _transformZone.position);
        }
        _transform.position = new Vector3(position.x, _transform.position.y, position.z);
    }

    private void ChangeSpriteRenderer(bool value)
    {
        if (_isEnable == false)
        {
            _spriteRendereCircle.enabled = value;
        }
    }

    public Vector3 GetVectorMagnitude(Vector3 position, Vector3 centerCirclePosition)
    {
        Vector3 offset = position - centerCirclePosition;
        offset = new Vector3(offset.x, 0, offset.y);
        Vector3 vector = centerCirclePosition + Vector3.ClampMagnitude(offset, _radius);
        return vector;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Wall>(out Wall wall) == true || collision.gameObject.TryGetComponent<Zone>(out Zone people))
        {
            _sphereCollider.enabled = false;
            ChangeSpriteRenderer(false);
            _isEnable = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        _sphereCollider.enabled = true;
        ChangeSpriteRenderer(true);
        _isEnable = false;
    }
}
