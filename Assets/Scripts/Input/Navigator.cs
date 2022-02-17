using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    [SerializeField] private Transform _transformZone;
    [SerializeField] private SpriteRenderer _spriteRendere;
    [SerializeField] private Transform _transform;

    private Vector3 _position;

    private void Start()
    {
        _position = _transform.position;
    }

    private void Update()
    {
        //if (_spriteRendere.enabled == false)
        //{
        //    _transform.position = _transformZone.position;
        //}
        _transform.position = _position;
    }

    public void ChangeSpriteRenderer(bool value)
    {
        _spriteRendere.enabled = value;
    }

    public void Track(Vector3 position)
    {
        _position = new Vector3(position.x, 0.0001f, position.z);
    }
}
