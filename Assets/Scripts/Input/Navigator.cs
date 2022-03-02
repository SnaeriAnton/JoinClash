using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    [SerializeField] private Transform _transformZone;
    [SerializeField] private SpriteRenderer _spriteRendereCircle;
    [SerializeField] private Transform _transform;
    [SerializeField] private Transform _transformWay;

    private float _defaultRadius = 1.8f;
    private float _radius;
    private bool _isExit = false;
    private float _offsetRadius = 0.003f;

    [SerializeField] private Transform _way;

    public float Radius => _defaultRadius;

    private void Start()
    {
        _radius = _defaultRadius;
    }

    public void Track(Vector3 newPosition, bool spriteEnable)
    {
        ChangeSpriteRenderer(spriteEnable);

        Vector3 position;

        if (newPosition == Vector3.zero)
        {
            position = new Vector3(_transformZone.position.x, _transform.position.y, _transformZone.position.z);
            _radius = _defaultRadius;
        }
        else
        {
            position = GetVectorMagnitude(newPosition, _transformZone.position);
        }


        if (_isExit == true)
        {
            if (_radius < _defaultRadius)
            {
                _radius += _offsetRadius;
            }
            else
            {
                _radius = _defaultRadius;
            }
        }


        _transform.position = new Vector3(position.x, _transform.position.y, position.z);

        if (_way != null)
        {
            Vector3 positionWay = BuildWay(_transform.position, _way.position);
            _transformWay.position = new Vector3(positionWay.x, _transform.position.y, positionWay.z);
        }
    }


    private void ChangeSpriteRenderer(bool value)
    {
        _spriteRendereCircle.enabled = value;
    }

    public Vector3 GetVectorMagnitude(Vector3 position, Vector3 centerCirclePosition)
    {
        Vector3 offset = position - centerCirclePosition;
        offset = new Vector3(offset.x, 0, offset.y);
        Vector3 vector = centerCirclePosition + Vector3.ClampMagnitude(offset, _radius);
        return vector;
    }

    private Vector3 BuildWay(Vector3 position, Vector3 centerCirclePosition)
    {
        Vector3 newVector = Vector3.Reflect(position, centerCirclePosition.normalized) * 10;
        newVector = GetVectorMagnitude(newVector, centerCirclePosition);
        return newVector;
    }


    //private void OnCollisionStay(Collision collision)
    //{
    //    _isExit = false;
    //    _way = collision.gameObject.GetComponent<Transform>();


    //    if (collision.gameObject.TryGetComponent<Let>(out Let let))
    //    {
    //        _radius = Mathf.Sqrt(Mathf.Pow(_transformZone.position.x - _transform.position.x, 2) + Mathf.Pow((_transformZone.position.z - _transform.position.z), 2));
    //    }

    //    if (collision.gameObject.TryGetComponent<Wall>(out Wall wall))
    //    {
    //        _radius = Mathf.Sqrt(Mathf.Pow(_transformZone.position.x - _transform.position.x, 2) + Mathf.Pow((_transformZone.position.z - _transform.position.z), 2));
    //    }
    //}


    private void OnCollisionEnter(Collision collision)
    {
        _isExit = false;
        _way = collision.gameObject.GetComponent<Transform>();


        if (collision.gameObject.TryGetComponent<Let>(out Let let))
        {
            _radius = Mathf.Sqrt(Mathf.Pow(_transformZone.position.x - _transform.position.x, 2) + Mathf.Pow((_transformZone.position.z - _transform.position.z), 2));
        }

        if (collision.gameObject.TryGetComponent<Wall>(out Wall wall))
        {
            _radius = Mathf.Sqrt(Mathf.Pow(_transformZone.position.x - _transform.position.x, 2) + Mathf.Pow((_transformZone.position.z - _transform.position.z), 2));
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        _isExit = true;
        _way = null;
    }
}
