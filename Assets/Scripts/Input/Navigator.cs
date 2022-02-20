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

        float positionX;
        float positionZ;
        Vector3 position;
        

        if (newPosition == Vector3.zero)
        {
            position = new Vector3(_transformZone.position.x, _transform.position.y, _transformZone.position.z);
        }
        else
        {
            positionX = CheakPosition(_zone.MaxDistanceX, _zone.MinDistanceX, _transform.position.x, newPosition.x);
            positionZ = CheakPosition(_zone.MaxDistanceZ, _zone.MinDistanceZ, _transform.position.z, newPosition.z);


            position = new Vector3(positionX, _transform.position.y, positionZ);
        }
        _transform.position = position;

    }

    private void ChangeSpriteRenderer(bool value)
    {
        _spriteRendere.enabled = value;
    }

    private float CheakPosition(float maxDistance, float minDistance, float currentPosition, float newPosition)
    {
        float position;
        //position = currentPosition < limitDistance ? newPosition : currentPosition;

        if (newPosition < maxDistance && newPosition > minDistance)
        {
            position = newPosition;
        }
        else
        {
            position = currentPosition;
        }

        return position;
    }

    private float CalculationLimitationDistance(float newPosition, float centerZon, float limitationDistance)
    {
        float limit;

        //limit *= newPosition > centerZon ? 1 : -1;

        if (newPosition > centerZon)
        {
            limitationDistance *= 1;
        }
        else
        {
            limitationDistance *= -1;
        }

        limit = centerZon + limitationDistance;
        return limit;
    }
}
