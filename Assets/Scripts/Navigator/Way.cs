using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Way : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private GameObject _wayLinePrefab;
    [SerializeField] private Transform _containerWays;
    [SerializeField] private Color _defaultColorLine;
    [SerializeField] private Color _crosedPeopleColorLine;
    [SerializeField] private Mover _mover;

    private List<LineRenderer> _lines = new List<LineRenderer>();
    private List<Zone> _zones = new List<Zone>();
    private int _wayLineCount = 0;
    private int _firstContact = 0;
    private int _invertVector = -1;

    private float _maxDistance = 0;
    private float _defaulsMaxDistance = 1.95f;

    private void Start()
    {
        _maxDistance = _defaulsMaxDistance;
    }

    public void Update()
    {
        //_wayLineCount = 0;
        //_wayLineCount += CalculaterWayLine(_transform.position + _transform.forward * 0.1f, _transform.forward, _wayLineCount);
        //RemoveOldLine(_wayLineCount);
    }

    public void PaveWay()
    {
        _wayLineCount = 0;
        _wayLineCount += CalculaterWayLine(_transform.position + _transform.forward * 0.1f, _transform.forward, _wayLineCount);
        RemoveOldLine(_wayLineCount);
    }

    public void ClearLines()
    {
        HandOverPosition();
        foreach (var line in _lines)
        {
            Destroy(line.gameObject);
        }
        _lines.Clear();
    }

    public void HandOverPosition()
    {
        foreach (var line in _lines)
        {
            _mover.AddTarget(line.GetPosition(1));
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
        RaycastHit[] hits = Physics.RaycastAll(startPosition, direction, _maxDistance, _layerMask);
        Debug.DrawRay(startPosition, direction * _maxDistance, Color.red);
        Vector3 hitPosition;

        if (hits.Length == 0)
        {
            _maxDistance = _defaulsMaxDistance;
            hitPosition = startPosition + direction * _maxDistance;
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
            foreach (var hit in hits)
            {
                if (hit.transform.TryGetComponent<Finger>(out Finger finger))
                {
                    _maxDistance = Vector3.Distance(_transform.position, finger.transform.position);
                }
            }

            if (hits[_firstContact].transform.TryGetComponent<Wall>(out Wall wall))
            {
                _maxDistance = _defaulsMaxDistance;
                addLine += CalculaterWayLine(hits[_firstContact].point, Vector3.Reflect(direction, hits[_firstContact].normal), indexLine + addLine);
            }

            if (hits[_firstContact].transform.TryGetComponent<Zone>(out Zone zone))
            {
                _maxDistance = _defaulsMaxDistance;
                if (CheakPeople(zone))
                {
                    //Через центр? от последнего качания!
                    addLine += CalculaterWayLine(zone.transform.position, hits[_firstContact].normal * _invertVector, indexLine + addLine, zone);
                }
                else
                {
                    hitPosition = startPosition + direction * _maxDistance;
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
        LineRenderer line = null;
        if (indexLine < _lines.Count)
        {
            line = _lines[indexLine];
        }
        else
        {
            GameObject newLine = Instantiate(_wayLinePrefab, Vector3.zero, Quaternion.identity, _containerWays);
            if (zoneWithPeople != null)
            {
                newLine.GetComponent<Line>().SetZone(zoneWithPeople);
            }
            line = newLine.GetComponent<LineRenderer>();
            //line.GetComponent<Transform>().rotation = Quaternion.Euler(0.4f, 0, 0);
            _lines.Add(line);
        }
        line.SetPosition(0, startposition);
        line.SetPosition(1, finishPosition);
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
        if (value == true)
        {
            SetColorLineWay(_crosedPeopleColorLine);
        }
        else
        {
            SetColorLineWay(_defaultColorLine);
        }
    }

    private void SetColorLineWay(Color color)
    {
        for (int i = 0; i < _lines.Count; i++)
        {
            _lines[i].startColor = color;
            _lines[i].endColor = color;
        }
    }
}
