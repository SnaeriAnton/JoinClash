using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Navigator _navigator;

    private float _maxDistance = 10000000;
    private bool _zonOfPeople;

    private void Update()
    {
        //var mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 screenMouse = Input.mousePosition;
        Vector3 worldMouse = Camera.main.ScreenToWorldPoint(screenMouse);
        Debug.Log(screenMouse);



        //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //if (Input.GetMouseButtonDown(0))
        //{
        //    _zonOfPeople = Physics.Raycast(Camera.main.transform.position, Camera.main.ScreenPointToRay(Input.mousePosition).direction, _maxDistance, _layerMask);

        //    if (_zonOfPeople == true)
        //    {
        //        _navigator.ChangeSpriteRenderer(true);
        //    }
        //}
        //else
        //{
        //    //_spriteRendererNavigator.enabled = false;
        //}
        _navigator.Track(screenMouse);
                
    }
}
