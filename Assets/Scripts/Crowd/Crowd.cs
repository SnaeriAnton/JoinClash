using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class Crowd : MonoBehaviour
{
    [SerializeField] private SphereCollider _sphereCollider;
    [SerializeField] private Transform _transform;
    [SerializeField] private Transform _transformLookAt;
    [SerializeField] private List<Human> _notActivePeople;
    [SerializeField] private Way _way;
    [SerializeField] private CrowdMover _crowdMover;
    [SerializeField] private Navigator _navigator;
    [SerializeField] private GameObject _bubble;
    [SerializeField] private Finger _finger;

    private List<Human> _peopleInCrowd = new List<Human>();
    private int _radiusCircle = 360;
    private float _stepRadiusCircle = 0.3f;
    private int _countPeopleInFullCircles;
    private int _maxPeopleInCircle = 0;
    private int _stepCountPeople = 8;
    private int _minCountPeople = 1;
    private Dictionary<int, int> _dictinaryCircles = new Dictionary<int, int>();

    public UnityAction Finished;
    public UnityAction<int> AddedPeople;
    public UnityAction<float> ChangedCrowd;
    public UnityAction<float> GotReadyToRun;
    public UnityAction<Vector3> LookedAtTarget;
    public UnityAction Saw;
    public UnityAction<Transform, Boss> ReachedFinish;
    public UnityAction<float> Stand;

    private void OnEnable()
    {
        _way.MovedAway += OnSetDistance;
        _crowdMover.Arrived += OnStayStill;
        _crowdMover.Seted += OnSetLookAtTarget;
        _finger.Enabled += OnSee;
    }

    private void OnDisable()
    {
        _way.MovedAway -= OnSetDistance;
        _crowdMover.Arrived -= OnStayStill;
        _crowdMover.Seted -= OnSetLookAtTarget;
        _finger.Enabled -= OnSee;
    }

    private void Start()
    {
        _sphereCollider.radius = _stepRadiusCircle;
        _countPeopleInFullCircles = _minCountPeople;
        AddFirstCircle();

        Human[] people = _transform.GetComponentsInChildren<Human>();
        for (int i = 0; i < people.Length; i++)
        {
            _peopleInCrowd.Add(people[i]);
            _peopleInCrowd[i].SetCrowd(this);
            _peopleInCrowd[i].SetLookAt(_transformLookAt);
            _peopleInCrowd[i].SetActive(true);
        }
        AddedPeople?.Invoke(_peopleInCrowd.Count);
    }

    private void AddFirstCircle()
    {
        _dictinaryCircles.Add(_minCountPeople, _minCountPeople);
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
            int sumPeopleInCrowd = countPeople + _peopleInCrowd.Count;
            if (_countPeopleInFullCircles >= sumPeopleInCrowd)
            {
                int addPeople = countPeople;
                int maxPeopleInCircleKey;
                int minSpaciousnessCrowd = (_dictinaryCircles.Count - 1) * (2 * _dictinaryCircles.Count);

                if (minSpaciousnessCrowd > _peopleInCrowd.Count)
                {
                    maxPeopleInCircleKey = 8;
                    for (int i = 1; i < _dictinaryCircles.Count; i++)
                    {
                        if (countPeople > 0)
                        {
                            _dictinaryCircles[maxPeopleInCircleKey] = ChangePeopleInCrowd(_dictinaryCircles[maxPeopleInCircleKey] <= 3, _dictinaryCircles[maxPeopleInCircleKey], (maxPeopleInCircleKey / 2), ref countPeople);
                        }
                        maxPeopleInCircleKey += _stepCountPeople;
                    }
                }
                AddPeopleInCrowd(addPeople);

                maxPeopleInCircleKey = _maxPeopleInCircle;

                for (int i = 0; i < _dictinaryCircles.Count; i++)
                {
                    if (countPeople > 0)
                    {
                        _dictinaryCircles[maxPeopleInCircleKey] = ChangePeopleInCrowd(_dictinaryCircles[maxPeopleInCircleKey] != maxPeopleInCircleKey, _dictinaryCircles[maxPeopleInCircleKey], maxPeopleInCircleKey, ref countPeople);
                    }
                    else
                    {
                        SortOutPlaceOfPeople();
                        return;
                    }
                    maxPeopleInCircleKey -= _stepCountPeople;
                }
            }
            else
            {
                ChangeValue(true);
                _dictinaryCircles.Add(_maxPeopleInCircle, 0);
                countPeople += SelectPeople(countPeople);
                AddPeopleInCircle(countPeople);
            }
        }
    }

    private void AddPeopleInCrowd(int countPeople)
    {
        for (int i = 0; i < countPeople; i++)
        {
            _peopleInCrowd.Add(_notActivePeople[i]);
            _notActivePeople[i].SetCrowd(this);
            _notActivePeople[i].SetActive(true);
            _notActivePeople[i].SetLookAt(_transformLookAt);
            _notActivePeople[i].StandPoseToRun();
            _notActivePeople.RemoveAt(i); 
        }
        AddedPeople?.Invoke(_peopleInCrowd.Count);
    }

    private int SelectPeople(int countPeople)
    {
        int displacedPeople = 0;
        int extra = 0;
        int sumPeopleInCrowd = countPeople + _peopleInCrowd.Count;

        if (_maxPeopleInCircle < countPeople && sumPeopleInCrowd < _countPeopleInFullCircles)
        {
            extra = countPeople - _maxPeopleInCircle;
            countPeople -= extra;
        }

        if (_maxPeopleInCircle >= countPeople && _peopleInCrowd.Count != 1)
        {
            int difference = _maxPeopleInCircle - countPeople;
            if (countPeople > _peopleInCrowd.Count)
            {
                int people = countPeople + _peopleInCrowd.Count;
                int lacsksPeople = _countPeopleInFullCircles - people - extra;
                displacedPeople += ReductionPeopleInCrowd(lacsksPeople);
            }
            else if (difference != 0)
            {
                displacedPeople += ReductionPeopleInCrowd(difference);
            }
        }
        RemovePeople(displacedPeople);
        return displacedPeople;
    }

    private int ReductionPeopleInCrowd(int lacksPeople)
    {
        int maxPeopleInCircleKey = 8;
        int displacedPeople = 0;
        for (int i = 1; i < _dictinaryCircles.Count; i++)
        {
            if (_dictinaryCircles[maxPeopleInCircleKey] != 0)
            {
                int minpeopleInCircle = maxPeopleInCircleKey / 2;
                if (_dictinaryCircles[maxPeopleInCircleKey] > minpeopleInCircle)
                {
                    int difference = _dictinaryCircles[maxPeopleInCircleKey] - maxPeopleInCircleKey / 2;
                    if (difference < lacksPeople)
                    {
                        displacedPeople += difference;
                        lacksPeople -= displacedPeople;
                        _dictinaryCircles[maxPeopleInCircleKey] -= difference;
                    }
                    else
                    {
                        _dictinaryCircles[maxPeopleInCircleKey] -= lacksPeople;
                        displacedPeople += lacksPeople;
                        lacksPeople = 0;
                    }
                }
            }
            maxPeopleInCircleKey += _stepCountPeople;
        }
        return displacedPeople;
    }

    private void RemovePeopleFromCrowd(int countPeople)
    {
        if (countPeople < 0)
        {
            countPeople *= -1;
        }

        int removePeople = countPeople;
        int maxPeopleInCircleKey = 8;
        int remove = -1;
        int countPeopleInCrowd = (_peopleInCrowd.Count - 1) - countPeople;

        if (countPeople != 0)
        {
            if (countPeopleInCrowd <= 0)
            {
                removePeople = _peopleInCrowd.Count - 1;

                countPeople = 0;
                _countPeopleInFullCircles = _minCountPeople;
                _maxPeopleInCircle = 0;
                _sphereCollider.radius = _stepRadiusCircle;
                _dictinaryCircles.Clear();
                AddFirstCircle();
            }
            else
            {
                for (int i = 1; i < _dictinaryCircles.Count; i++)
                {
                    if (countPeople != 0)
                    {
                        _dictinaryCircles[maxPeopleInCircleKey] = ChangePeopleInCrowd((maxPeopleInCircleKey / 2) < _dictinaryCircles[maxPeopleInCircleKey], _dictinaryCircles[maxPeopleInCircleKey], (maxPeopleInCircleKey / 2), ref countPeople, remove);

                        int minpeopleInCircle = maxPeopleInCircleKey / 2;
                        int countCircle = _dictinaryCircles.Count - 1;
                        if (_dictinaryCircles[maxPeopleInCircleKey] == minpeopleInCircle && countPeople >= minpeopleInCircle && countCircle == i)
                        {
                            countPeople -= _dictinaryCircles[maxPeopleInCircleKey];
                            _dictinaryCircles[maxPeopleInCircleKey] = 0;
                            ChangeValue(false);
                            _dictinaryCircles.Remove(maxPeopleInCircleKey);
                        }
                        maxPeopleInCircleKey += _stepCountPeople;
                    }
                }

                if (countPeople != 0)
                {
                    if (_dictinaryCircles[_maxPeopleInCircle] > countPeople)
                    {
                        _dictinaryCircles[_maxPeopleInCircle] -= countPeople;
                        countPeople = 0;
                    }
                    else
                    {
                        countPeople -= _dictinaryCircles[_maxPeopleInCircle];
                        _dictinaryCircles[_maxPeopleInCircle] = 0;
                        _dictinaryCircles.Remove(_maxPeopleInCircle);
                        ChangeValue(false);
                    }
                }
                removePeople -= countPeople;
            }
        }

        RemovePeople(removePeople);
        if (countPeople != 0)
        {
            RemovePeopleFromCrowd(countPeople);
        }
        DistributePeople();
        SortOutPlaceOfPeople();
    }

    private void RemovePeople(int countPeople)
    {
        int remainderPeople = _peopleInCrowd.Count - countPeople;
        if (_peopleInCrowd.Count != 1)
        {
            if (remainderPeople > 0)
            {
                RemoveHuman(countPeople);
            }
            else
            {
                RemoveHuman(_peopleInCrowd.Count - 1);
            }
        }
    }

    private void RemoveHuman(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _notActivePeople.Add(_peopleInCrowd[_peopleInCrowd.Count - 1]);
            _peopleInCrowd[_peopleInCrowd.Count - 1].SetActive(false);
            _peopleInCrowd.RemoveAt(_peopleInCrowd.Count - 1);
        }
    }

    private int ChangePeopleInCrowd(bool value, int countPeopleInCircle, int peopleInCircleKey, ref int countPeople, int remove = 1)
    {
        if (value == true)
        {
            int lacksPeople = peopleInCircleKey - countPeopleInCircle;

            if (lacksPeople < 0)
            {
                lacksPeople *= -1;
            }

            if (countPeople >= lacksPeople)
            {
                countPeopleInCircle += (lacksPeople * remove);
                countPeople -= lacksPeople;
            }
            else
            {
                countPeopleInCircle += (countPeople * remove);
                countPeople = 0;
            }
        }
        return countPeopleInCircle;
    }

    private void DistributePeople()
    {
        int maxPeopleInCircleKey = _maxPeopleInCircle;
        int freePeople = 0;

        for (int i = 0; i < (_dictinaryCircles.Count - 1); i++)
        {
            _dictinaryCircles[maxPeopleInCircleKey] = ChangePeopleInCrowd(freePeople != 0, _dictinaryCircles[maxPeopleInCircleKey], maxPeopleInCircleKey, ref freePeople);

            int countPeopleInCrowd = _countPeopleInFullCircles - _maxPeopleInCircle;

            if (countPeopleInCrowd >= _peopleInCrowd.Count)
            {
                freePeople = _dictinaryCircles[maxPeopleInCircleKey];
                _dictinaryCircles[maxPeopleInCircleKey] = 0;
                ChangeValue(false);
            }
            maxPeopleInCircleKey -= _stepCountPeople;
        }

        maxPeopleInCircleKey = 8;

        for (int i = 1; i < _dictinaryCircles.Count; i++)
        {
            if (_dictinaryCircles[maxPeopleInCircleKey] == 0)
            {
                _dictinaryCircles.Remove(maxPeopleInCircleKey);
            }
            maxPeopleInCircleKey += _stepCountPeople;
        }
    }

    private void ChangeValue(bool increase)
    {
        if (increase == true)
        {
            _maxPeopleInCircle += _stepCountPeople;
            _countPeopleInFullCircles += _maxPeopleInCircle;
            _sphereCollider.radius += _stepRadiusCircle;
        }
        else
        {
            _countPeopleInFullCircles -= _maxPeopleInCircle;
            _maxPeopleInCircle -= _stepCountPeople;
            _sphereCollider.radius -= _stepRadiusCircle;
        }
    }

    private void SortOutPlaceOfPeople()
    {
        float radius = 0;
        int startValue = 0;
        int countPeople = _peopleInCrowd.Count - 1;

        if (countPeople > 0)
        {
            foreach (var circle in _dictinaryCircles)
            {
                if (circle.Key != 1 && circle.Value != 0)
                {
                    float angleStep = _radiusCircle / circle.Value;
                    for (int i = startValue; i < circle.Value + startValue; i++)
                    {
                        _peopleInCrowd[i].SetPosition(new Vector3(radius * Mathf.Cos(angleStep * (i + 1) * Mathf.Deg2Rad), 0, radius * Mathf.Sin(angleStep * (i + 1) * Mathf.Deg2Rad)), radius);
                    }
                }
                radius += _stepRadiusCircle;
                startValue += circle.Value;
            }
        }
    }

    private void OnSetDistance(float distance)
    {
        GotReadyToRun?.Invoke(distance);
    }

    private void OnStayStill()
    {
        int distance = 0;
        Stand?.Invoke(distance);
    }

    private void OnSetLookAtTarget(Vector3 position)
    {
        LookedAtTarget?.Invoke(position);
    }

    private void OnSee()
    {
        Saw?.Invoke();
    }

    private void ReacheFinish(Finish finish)
    {
        ReachedFinish?.Invoke(finish.PositionBoss, finish.Boss);
        _sphereCollider.enabled = false;
        Finished?.Invoke();
        this.enabled = false;
        _crowdMover.enabled = false;
        _bubble.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Banner>(out Banner banner))
        {
            AddPeople(banner.Number, banner.Addition, banner.Subtraction, banner.Divide, banner.Multiply);
        }

        if (other.TryGetComponent<Finish>(out Finish finish))
        {
            ReacheFinish(finish);
        }
    }
}
