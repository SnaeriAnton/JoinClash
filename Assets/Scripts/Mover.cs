using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private Transform _transform;

    private List<Vector3> _targets = new List<Vector3>();
    private Vector3 _target;
    private float _speed = 0.03f;
    private int _i = 0;

    private void Start()
    {
        _target = _transform.position;
    }

    private void Update()
    {
        _transform.position = Vector3.MoveTowards(_transform.position, _target, _speed);

        if (_targets.Count > 0)
        {
            _target = _targets[_i];
            if (_transform.position == _targets[_i])
            {
                _i++;
                _target = _targets[_i];
            }
        }

        if (_i + 1 == _targets.Count)
        {
            _i = 0;
            _targets.Clear();
        }
    }

    public void AddTarget(Vector3 target)
    {
        _targets.Add(target);
    }
}
