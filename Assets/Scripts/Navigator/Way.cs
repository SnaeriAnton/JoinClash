using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Way : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private GameObject _wayLinePrefab;
    [SerializeField] private Transform _containerWays;
    [SerializeField] private CrowdMover _mover;
    [SerializeField] private Navigator _navigator;

    private List<Line> _lines = new List<Line>();
    private List<Zone> _zones = new List<Zone>();
    private int _wayLineCount = 0;
    private int _firstContact = 0;
    private int _invertVector = -1;
    private float _distance = 0;
    private float _offsetCrowd = 0.1f;
    private float _defaulsDistance = 1.95f;
    private bool _isFirst = true;
    private int _endPositionLine = 1;

    public UnityAction<float> MovedAway;

    private void OnEnable()
    {
        _navigator.Disabled += OnSetDistance;
    }

    private void OnDisable()
    {
        _navigator.Disabled -= OnSetDistance;
    }

    public void PaveWay()
    {
        if (_isFirst == true)
        {
            _distance = 0.5f;
            _isFirst = false;
        }

        _wayLineCount = 0;
        _wayLineCount += CalculaterWayLine(_transform.position + _transform.forward * _offsetCrowd, _transform.forward, _wayLineCount);
        RemoveOldLine(_wayLineCount);
    }

    public void ClearLines()
    {
        HandOverPosition();

        foreach (var line in _lines)
        {
            if (line.IsGoesWay == true)
            {
                line.GoesWay();
            }
            Destroy(line.gameObject);
        }
        _lines.Clear();
        _isFirst = true;
    }

    private void HandOverPosition()
    {
        foreach (var line in _lines)
        {
            _mover.AddTarget(line.GetPositionLineRenderer(_endPositionLine));
        }
    }

    private void RemoveOldLine(int CountLine)
    {
        if (CountLine < _lines.Count)
        {
            Destroy(_lines[_lines.Count - 1].gameObject);
            _lines.RemoveAt(_lines.Count - 1);
            RemoveOldLine(CountLine);
        }
    }

    private int CalculaterWayLine(Vector3 startPosition, Vector3 direction, int indexLine, Zone zoneWithPeople = null)
    {
        int addLine = 1;
        RaycastHit[] hits = Physics.RaycastAll(startPosition, direction, _distance, _layerMask);
        Debug.DrawRay(startPosition, direction * _distance, Color.red);
        Vector3 hitPosition;

        if (hits.Length == 0)
        {
            hitPosition = startPosition + direction * _distance;
            if (_zones.Count == 0)
            {
                ChabgeColorLineWay(false);
            }
        }
        else
        {
            hitPosition = hits[_firstContact].point;
            if (_zones.Count == 0)
            {
                ChabgeColorLineWay(false);
            }
        }

        DrawWayLine(startPosition, hitPosition, indexLine, zoneWithPeople);

        if (hits.Length >= 1)
        {
            bool isFinger = false;
            foreach (var hit in hits)
            {
                if (hit.transform.TryGetComponent<Finger>(out Finger finger))
                {
                    _distance = Vector3.Distance(_transform.position, finger.transform.position);
                    MovedAway?.Invoke(_distance);
                    isFinger = true;
                }
            }

            if (hits[_firstContact].transform.TryGetComponent<Wall>(out Wall wall) && isFinger == false)
            {
                _distance = _defaulsDistance;
                MovedAway?.Invoke(_distance);
                addLine += CalculaterWayLine(hits[_firstContact].point, Vector3.Reflect(direction, hits[_firstContact].normal), indexLine + addLine);
            }

            if (hits[_firstContact].transform.TryGetComponent<Zone>(out Zone zone) && isFinger == false)
            {
                _distance = _defaulsDistance;
                MovedAway?.Invoke(_distance);
                if (CheakPeople(zone))
                {
                    addLine += CalculaterWayLine(zone.transform.position, hits[_firstContact].normal * _invertVector, indexLine + addLine, zone);
                }
                else
                {
                    hitPosition = startPosition + direction * _distance;
                    DrawWayLine(startPosition, hitPosition, indexLine);
                }
                ChabgeColorLineWay(true);
                _zones.Clear();
            }
        }
        return addLine;
    }

    private void DrawWayLine(Vector3 startposition, Vector3 finishPosition, int indexLine, Zone zoneWithPeople = null)
    {
        Line line = null;
        if (indexLine < _lines.Count)
        {
            line = _lines[indexLine];
        }
        else
        {
            GameObject newLine = Instantiate(_wayLinePrefab, Vector3.zero, Quaternion.identity, _containerWays);

            line = newLine.GetComponent<Line>();
            if (zoneWithPeople != null)
            {
                line.SetZone(zoneWithPeople);
            }
            _lines.Add(line);
        }
        line.SetPositionLineRenderer(startposition, finishPosition);
    }

    private bool CheakPeople(Zone zone)
    {
        foreach (var peopleTransform in _zones)
        {
            if (peopleTransform == zone)
            {
                return false;
            }
        }
        _zones.Add(zone);
        return true;
    }

    private void ChabgeColorLineWay(bool value)
    {
        for (int i = 0; i < _lines.Count; i++)
        {
            _lines[i].ChangeColorLineRandarer(value);
        }
    }

    private void OnSetDistance()
    {
        _distance = _defaulsDistance;
    }
}
