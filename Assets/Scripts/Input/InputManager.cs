using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Navigator _navigator;
    [SerializeField] private GameObject _navigatorObject;
    [SerializeField] private Way _way;
    [SerializeField] private Crowd _crowd;

    private float _maxDistance = 10000000;
    private bool _zonOfPeople;
    private float _distanceFromCamera = 6.2f;
    private Vector3 _screenWorldPosition;
    private float _minDistanceBeetvenNavigator = 0.26f;

    private void OnEnable()
    {
        _crowd.Finished += OnDisableInput;
    }

    private void OnDisable()
    {
        _crowd.Finished += OnDisableInput;
    }

    private void Update()
    {
        _screenWorldPosition = GetScreenPosition();

        if (Input.GetMouseButtonDown(0))
        {
            Touch();
        }

        if (Input.GetMouseButtonUp(0))
        {
            LetGo();
        }
    }

    private void FixedUpdate()
    {
        SetDirection(_screenWorldPosition);
    }

    private Vector3 GetScreenPosition()
    {
        Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _distanceFromCamera);
        Vector3 screenWorldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        return screenWorldPosition;
    }

    private void Touch()
    {
        _zonOfPeople = Physics.Raycast(Camera.main.transform.position, Camera.main.ScreenPointToRay(Input.mousePosition).direction, _maxDistance, _layerMask);
    }

    private void LetGo()
    {
        _zonOfPeople = false;
        _navigator.DisableFinger(_zonOfPeople);
        _navigator.Track(Vector3.zero, _zonOfPeople);
        _way.ClearLines();
    }

    private void SetDirection(Vector3 screenWorldPosition)
    {
        if (_zonOfPeople == true)
        {
            _navigator.DisableFinger(_zonOfPeople);
            _navigator.Track(screenWorldPosition, _zonOfPeople);
            if (CheakDistanceBetweenZonAndNavigator() == true)
            {
                _way.PaveWay();
            }
        }
    }

    private bool CheakDistanceBetweenZonAndNavigator()
    {
        float disctance = Vector3.Distance(_navigator.transform.position, _way.transform.position);
        if (disctance > _minDistanceBeetvenNavigator)
        {
            return true;
        }
        return false;
    }

    private void OnDisableInput()
    {
        this.enabled = false;
    }
}
