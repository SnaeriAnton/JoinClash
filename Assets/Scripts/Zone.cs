using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Zone : MonoBehaviour
{
    [SerializeField] private Transform _transform;

    private Vector3 _defaultPosition;
    private float _maxDistance = 1;
    private float _minDistance = -1;
    private float _maxDistanceX;
    private float _minDistanceX;
    private float _maxDistanceZ;
    private float _minDistanceZ;

    public float MaxDistanceX => _maxDistanceX;
    public float MinDistanceX => _minDistanceX;
    public float MaxDistanceZ => _maxDistanceZ;
    public float MinDistanceZ => _minDistanceZ;

    private void Start()
    {
        _defaultPosition = _transform.position;
        _maxDistanceX = CalculationDistance(_transform.position.x, _maxDistance);
        _minDistanceX = CalculationDistance(_transform.position.x, _minDistance);
        _maxDistanceZ = CalculationDistance(_transform.position.z, _maxDistance);
        _minDistanceZ = CalculationDistance(_transform.position.z, _minDistance);
    }

    private void Update()
    {
        if (_transform.position != _defaultPosition)
        {
            _defaultPosition = _transform.position;
            _maxDistanceX = CalculationDistance(_transform.position.x, _maxDistance);
            _minDistanceX = CalculationDistance(_transform.position.x, _minDistance);
            _maxDistanceZ = CalculationDistance(_transform.position.z, _maxDistance);
            _minDistanceZ = CalculationDistance(_transform.position.z, _minDistance);
        }
    }

    private float CalculationDistance(float position, float distance)
    {
        float lomition = position + distance;
        return lomition;
    }


}
