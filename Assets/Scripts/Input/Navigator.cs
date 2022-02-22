using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    [SerializeField] private Transform _transformZone;
    [SerializeField] private SpriteRenderer _spriteRendere;
    [SerializeField] private Transform _transform;
    [SerializeField] private Zone _zone;

    public void Track(Vector3 newPosition, bool spriteEnable)
    {
        ChangeSpriteRenderer(spriteEnable);

        Vector3 position;

        if (newPosition == Vector3.zero)
        {
            position = new Vector3(_transformZone.position.x, _transform.position.y, _transformZone.position.z);
        }
        else
        {
            position = _zone.GetVectorMagnitude(newPosition);
        }

        _transform.position = new Vector3(position.x, _transform.position.y, position.z);
    }

    private void ChangeSpriteRenderer(bool value)
    {
        _spriteRendere.enabled = value;
    }
}
