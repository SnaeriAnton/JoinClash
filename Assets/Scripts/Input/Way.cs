using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Way : MonoBehaviour
{
    //[SerializeField] private Transform _transform;
    //[SerializeField] private Transform _transformNavigator;

    //private bool _isContact = false;
    //private Vector3 _direction;

    //private void Update()
    //{
    //    if (_isContact == true)
    //    {
    //        _transform.position = new Vector3(_direction.x, _transformNavigator.position.y, _direction.z);
    //    }
    //    else
    //    {
    //        _transform.position = _transformNavigator.position;
    //    }
    //}

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.TryGetComponent<Transform>(out Transform transform))
    //    {
    //        _isContact = true;
    //        _direction = Vector3.Reflect(transform.position, _transformNavigator.position) * 2;
    //    }

    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    _isContact = false;
    //}
}
