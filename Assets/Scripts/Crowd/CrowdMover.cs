using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrowdMover : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private AudioSource _audioSource;

    private List<Vector3> _targets = new List<Vector3>();
    private Vector3 _target;
    private float _speed = 8f;
    private int _i = 0;
    private Vector3 _isArrive;
    private bool _inStop = false;

    private bool _firstTarget = true;

    public UnityAction Arrived;
    public UnityAction<Vector3> Seted;

    private void Start()
    {
        _target = _transform.position;
    }

    private void Update()
    {
        _transform.position = Vector3.MoveTowards(_transform.position, _target, _speed * Time.deltaTime);

        if (_targets.Count > 0)
        {
            _isArrive = _targets[_targets.Count - 1];
            _target = _targets[_i];
            if (_firstTarget == true)
            {
                Seted?.Invoke(_target);
                _firstTarget = false;
            }
            if (_transform.position == _targets[_i])
            {
                _i++;
                _target = _targets[_i];
                Seted?.Invoke(_target);
            }
        }


        if (_transform.position == _isArrive && _inStop == true)
        {
            Arrived?.Invoke();
            _inStop = false;
            _firstTarget = true;
        }


        if (_i + 1 == _targets.Count)
        {
            _i = 0;
            _targets.Clear();
            _inStop = true;
        }


    }

    public void AddTarget(Vector3 target)
    {
        _targets.Add(target);
        _audioSource.Play();
    }
}
