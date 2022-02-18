using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    [SerializeField] private Transform _transformZone;
    [SerializeField] private SpriteRenderer _spriteRendere;
    [SerializeField] private Transform _transform;


    public void ChangeSpriteRenderer(bool value)
    {
        _spriteRendere.enabled = value;
    }

    public void Track(Vector3 position)
    {
        _transform.position = new Vector3(position.x, _transform.position.y, position.z);
    }
}
