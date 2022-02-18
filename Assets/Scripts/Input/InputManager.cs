using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Navigator _navigator;

    private float _maxDistance = 10000000;
    private bool _zonOfPeople;
    private float _distanceFromCamera = 6.5f;
    Vector3 _screenWorldPosition;

    private void Update()
    {

        _screenWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _distanceFromCamera));
        if (Input.GetMouseButtonDown(0))
        {
            _zonOfPeople = Physics.Raycast(Camera.main.transform.position, Camera.main.ScreenPointToRay(Input.mousePosition).direction, _maxDistance, _layerMask);
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            _zonOfPeople = false;
            _navigator.ChangeSpriteRenderer(false);
        }

        if (_zonOfPeople == true)
        {
            _navigator.ChangeSpriteRenderer(true);
            _navigator.Track(_screenWorldPosition);
        }

    }
}
