using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Crowd : MonoBehaviour
{
    [SerializeField] private SphereCollider _sphereCollider;
    [SerializeField] private Transform _transform;
    [SerializeField] private Transform _transformLookAt;
    [SerializeField] private List<Human> _notActivePeople;
    [SerializeField] private Way _way;
    [SerializeField] private CrowdMover _crowdMover;
    [SerializeField] private Navigator _navigator;

    private int _countPeople;
    private List<Human> _peopleInCrowd = new List<Human>();
    private float _radius;
    private int _radiusCircle = 360;
    private float _stepRadiusCircle = 0.36f;
    private int _countPeopleInFullCircles;
    private int _maxPeopleInCircle = 8;
    private int _stepCountPeople = 8;
    private int _minCountPeople = 1;

    public Transform TransformParent => _transform;

    public UnityAction Finished;

    private void OnEnable()
    {
        _way.MovedAway += OnSetDistance;
        _crowdMover.Arrived += OnStayStill;
        _crowdMover.Seted += OnSetLookAtTarget;
        _navigator.Enabled += OnSee;
    }

    private void OnDisable()
    {
        _way.MovedAway -= OnSetDistance;
        _crowdMover.Arrived -= OnStayStill;
        _crowdMover.Seted -= OnSetLookAtTarget;
        _navigator.Enabled -= OnSee;
    }

    private void Start()
    {
        _radius = _stepRadiusCircle;
        _sphereCollider.radius = _radius;
        _countPeopleInFullCircles = _minCountPeople;
        Human[] people = _transform.GetComponentsInChildren<Human>();
        for (int i = 0; i < people.Length; i++)
        {
            _peopleInCrowd.Add(people[i]);
            _peopleInCrowd[i].SetLookAt(_transformLookAt);
            _peopleInCrowd[i].SetActive(true);
        }
    }

    public void AddPeople(Human human)
    {
        _notActivePeople.Add(human);
        human.SetParent(_transform);
        human.SetActive(false);
    }

    public void AddPeople(int count, bool addition = false, bool subtraction = false, bool divide = false, bool multiply = false)
    {
        int countPeople;
        if (multiply == true)
        {
            countPeople = _peopleInCrowd.Count * count;
            countPeople = countPeople - _peopleInCrowd.Count;
            AddPeopleInCircle(countPeople);
        }
        else if (divide == true)
        {
            countPeople = _peopleInCrowd.Count / count;
            countPeople = _peopleInCrowd.Count - countPeople;
            RemovePeopleFromCrowd(countPeople);
        }
        else if (subtraction == true)
        {
            RemovePeopleFromCrowd(count);
        }
        else if (addition == true)
        {
            AddPeopleInCircle(count);
        }
    }

    private void AddPeopleInCircle(int countPeople)
    {
        if (countPeople != 0)
        {
            int peopleInCrowd = _peopleInCrowd.Count - _countPeopleInFullCircles;
            if (peopleInCrowd < _maxPeopleInCircle)
            {
                int lacksPeople = _maxPeopleInCircle - peopleInCrowd;
                if (countPeople >= lacksPeople)
                {
                    _countPeople = countPeople - lacksPeople;
                }
                else
                {
                    _countPeople = 0;
                    lacksPeople = countPeople;
                }
                PlaceNewPeople(lacksPeople, _radius, peopleInCrowd);
                AddPeopleInCircle(_countPeople);
            }
            else
            {
                ChangeValue(true);
                AddPeopleInCircle(countPeople);
            }
        }
    }

    private void RemovePeopleFromCrowd(int countPeople)
    {
        if (_peopleInCrowd.Count != 1 && countPeople != 0)
        {
            int remainderPeople = _peopleInCrowd.Count - countPeople;
            if (_peopleInCrowd.Count <= countPeople || remainderPeople == 1)
            {
                int count = _peopleInCrowd.Count - 1;
                RemovePeople(count);
                SetDefaultValue();
            }
            else if (_countPeopleInFullCircles <= remainderPeople)
            {
                RemovePeople(countPeople);
                PlaceOldPeople(_radius);
            }
            else
            {
                ChangeValue(false);
                RemovePeopleFromCrowd(countPeople);
            }
        }
        _countPeople = 0;
    }

    private void ChangeValue(bool _changerValue)
    {
        if (_changerValue == true)
        {
            _countPeopleInFullCircles += _maxPeopleInCircle;
            _maxPeopleInCircle += _stepCountPeople;
            _radius += _stepRadiusCircle;
            _sphereCollider.radius = _radius;
        }
        else
        {
            _maxPeopleInCircle -= _stepCountPeople;
            _countPeopleInFullCircles -= _maxPeopleInCircle;
            if (_countPeopleInFullCircles < 0)
            {
                _countPeopleInFullCircles = _minCountPeople;
            }
            _radius -= _stepRadiusCircle;
            _sphereCollider.radius = _radius;
        }
    }

    private void SetDefaultValue()
    {
        _countPeopleInFullCircles = _minCountPeople;
        _maxPeopleInCircle = _stepCountPeople;
        _radius = _stepRadiusCircle;
        _sphereCollider.radius = _radius;
    }

    private void RemovePeople(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _notActivePeople.Add(_peopleInCrowd[_peopleInCrowd.Count - 1]);
            _peopleInCrowd[_peopleInCrowd.Count - 1].SetActive(false);
            _peopleInCrowd.RemoveAt(_peopleInCrowd.Count - 1);
        }
    }

    private void PlaceNewPeople(int countPeople, float radius, int peopleInCircle)
    {
        int angleStep = _radiusCircle / (peopleInCircle + countPeople);
        for (int i = 0; i < countPeople; i++)
        {
            _peopleInCrowd.Add(_notActivePeople[i]);
            _notActivePeople[i].SetActive(true);
            _notActivePeople[i].SetLookAt(_transformLookAt);
            _notActivePeople[i].SetRadius(radius);
            _notActivePeople[i].StandPoseToRun();
            _notActivePeople.RemoveAt(i);
        }

        int startValue = _peopleInCrowd.Count - (peopleInCircle + countPeople);

        SortOutPeople(startValue, radius, angleStep);
    }

    private void PlaceOldPeople(float radius)
    {
        if (_peopleInCrowd.Count - _countPeopleInFullCircles <= 0)
        {
            return;
        }
        int angleStep = _radiusCircle / (_peopleInCrowd.Count - _countPeopleInFullCircles);
        int startValue = _peopleInCrowd.Count - (_peopleInCrowd.Count - _countPeopleInFullCircles);

        SortOutPeople(startValue, radius, angleStep);
    }

    private void SortOutPeople(int startValue, float radius, float angleStep)
    {
        for (int i = startValue; i < _peopleInCrowd.Count; i++)
        {
            _peopleInCrowd[i].SetPosition(new Vector3(radius * Mathf.Cos(angleStep * (i + 1) * Mathf.Deg2Rad), 0, radius * Mathf.Sin(angleStep * (i + 1) * Mathf.Deg2Rad)));
        }
    }

    private void OnSetDistance(float distance)
    {
        foreach (var human in _peopleInCrowd)
        {
            human.ReadyToRun(distance);
        }
    }

    private void OnStayStill()
    {
        int distance = 0;
        foreach (var human in _peopleInCrowd)
        {
            human.ReadyToRun(distance);
            human.Stay();
        }
    }

    private void OnSetLookAtTarget(Vector3 position)
    {
        foreach (var human in _peopleInCrowd)
        {
            human.LookAtRotation(position);
        }
    }

    private void OnSee()
    {
        foreach (var human in _peopleInCrowd)
        {
            human.See();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Banner>(out Banner banner))
        {
            AddPeople(banner.Number, banner.Addition, banner.Subtraction, banner.Divide, banner.Multiply);
        }

        if (other.TryGetComponent<Finish>(out Finish finish))
        {
            foreach (var human in _peopleInCrowd)
            {
                human.SetBossParameters(finish.PositionBoss);
            }
            _sphereCollider.enabled = false;
            Finished?.Invoke();
            this.enabled = false;
            _crowdMover.enabled = false;
        }
    }
}
