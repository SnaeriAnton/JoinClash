using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Line : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _transform;
    [SerializeField] private Color _defaultColorLine;
    [SerializeField] private Color _crosedPeopleColorLine;

    private Zone _zone = null;
    private bool _isGoesWay = false;
    private Vector3 _position = new Vector3(0, 0.06f, 0);

    public bool IsGoesWay => _isGoesWay;
    public LineRenderer LineRenderer => _lineRenderer;

    private void OnDestroy()
    {
        if (_zone != null)
        {
            _zone.Deselect();
        }
    }

    private void Start()
    {
        _transform.position = _position;
    }

    public void SetZone(Zone zone)
    {
        _zone = zone;
        if (zone != null)
        {
            _zone.Select();
            _isGoesWay = true;
        }
    }

    public void GoesWay()
    {
        _zone = null;
    }

    public void SetPositionLineRenderer(Vector3 startposition, Vector3 finishPosition)
    {
        _lineRenderer.SetPosition(0, startposition);
        _lineRenderer.SetPosition(1, finishPosition);
    }

    public Vector3 GetPositionLineRenderer(int index)
    {
        return _lineRenderer.GetPosition(index);
    }

    public void ChangeColorLineRandarer(bool value)
    {
        if (value == true)
        {
            SetColorLineRandarer(_crosedPeopleColorLine);
        }
        else
        {
            SetColorLineRandarer(_defaultColorLine);
        }
    }

    private void SetColorLineRandarer(Color color)
    {
        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;
    }
}
