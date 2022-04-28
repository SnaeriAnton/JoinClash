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
    private float _defaulsDistance = 2.75f;
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
            hitPosition = GetHitPosition(startPosition, direction);
            ChekCountZones();
        }
        else
        {
            hitPosition = hits[_firstContact].point;
            ChekCountZones();
        }

        DrawWayLine(startPosition, hitPosition, indexLine, zoneWithPeople);

        if (hits.Length >= 1 && CheckFinger(hits) == false)
        {
            CheckHits(hits[_firstContact], startPosition, direction, hitPosition, indexLine, ref addLine);
        }
        return addLine;
    }

    private void ChekCountZones()
    {
        if (_zones.Count == 0)
        {
            ChangeColorLineWay(false);
        }
    }

    private Vector3 GetHitPosition(Vector3 startPosition, Vector3 direction)
    {
        Vector3 hitPosition = startPosition + direction * _distance;
        return hitPosition;
    }

    private void SetDistance(float distance)
    {
        _distance = distance;
        MovedAway?.Invoke(_distance);
    }

    private void CheckHits(RaycastHit hit, Vector3 startPosition, Vector3 direction, Vector3 hitPosition, int indexLine, ref int addLine)
    {

        if (hit.transform.TryGetComponent<Wall>(out Wall wall))
        {
            SetDistance(_defaulsDistance);
            addLine += CalculaterWayLine(hit.point, Vector3.Reflect(direction, hit.normal), indexLine + addLine);
        }

        if (hit.transform.TryGetComponent<Zone>(out Zone zone))
        {
            SetDistance(_defaulsDistance);
            if (CheakPeople(zone) == true)
            {
                addLine += CalculaterWayLine(zone.transform.position, hit.normal * _invertVector, indexLine + addLine, zone);
            }
            else
            {
                hitPosition = GetHitPosition(startPosition, direction);
                DrawWayLine(startPosition, hitPosition, indexLine);
            }
            ChangeColorLineWay(true);
            _zones.Clear();
        }
    }

    private bool CheckFinger(RaycastHit[] hits)
    {
        foreach (var hit in hits)
        {
            if (hit.transform.TryGetComponent<Finger>(out Finger finger))
            {
                SetDistance(Vector3.Distance(_transform.position, finger.transform.position));
                return true;
            }
        }
        return false;
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

    private void ChangeColorLineWay(bool value)
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
