using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HumanAnimator))]
[RequireComponent(typeof(HumanFighter))]
[RequireComponent(typeof(HumanMover))]
[RequireComponent(typeof(HumanOption))]
public class Human : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private HumanAnimator _animator;
    [SerializeField] private HumanFighter _fighter;
    [SerializeField] private HumanMover _mover;
    [SerializeField] private HumanOption _option;
    [SerializeField] private HumanDeath _death;

    private Crowd _crowd;
    private Transform _lookAt;
    private int _health = 10;
    private bool _isBoss = false;
    private bool _sees = false;
    private Boss _boss;

    public UnityAction Finished;
    public UnityAction Won;

    private void OnDisable()
    {
        if (_crowd != null)
        {
            _crowd.GotReadyToRun -= OnReadyToRun;
            _crowd.LookedAtTarget -= OnLookAtRotation;
            _crowd.Saw -= OnSee;
            _crowd.ReachedFinish -= OnReacheFinish;
            _crowd.Stand -= Stay;
        }
    }

    private void Update()
    {
        if (_sees == true)
        {
            _transform.LookAt(_lookAt);
        }
    }

    public void OnReacheFinish(Transform bossPosition, Boss boss)
    {
        _isBoss = true;
        _fighter.enabled = true;
        _option.enabled = false;
        _mover.enabled = true;
        _mover.SetBossPosition(bossPosition.position);
        OnSee();
        SetLookAt(bossPosition);
        Finished?.Invoke();
        _boss = boss;
        _boss.Died += OnWin;
    }

    public void SetLookAt(Transform lookAt)
    {
        _lookAt = lookAt;
    }

    public void OnLookAtRotation(Vector3 position)
    {
        if (_isBoss == false)
        {
            _sees = false;
            _option.LookAtRotation(position);
        }
    }

    public void SetParent(Transform parent)
    {
        _transform.SetParent(parent);
    }

    public void SetActive(bool isActive)
    {
        _option.SetActive(isActive);
    }

    public void SetPosition(Vector3 position, float radius)
    {
        _option.SetPosition(position);
        _option.SetRadius(radius);
    }

    public void OnReadyToRun(float distance)
    {
        _animator.GetReadyToRun(distance);
    }

    public void StandPoseToRun()
    {
        _animator.StandPoseToRun();
    }

    public void Stay(float distance)
    {
        OnReadyToRun(distance);
        _animator.Stay();
    }

    public void OnSee()
    {
        _sees = true;
    }

    public void TakingDamage(int damage)
    {
        if (damage > 0)
        {
            _health -= damage;
            _death.ChekDeath(_health);
        }
    }

    public void SetCrowd(Crowd crowd)
    {
        _crowd = crowd;
        _crowd.GotReadyToRun += OnReadyToRun;
        _crowd.LookedAtTarget += OnLookAtRotation;
        _crowd.Saw += OnSee;
        _crowd.ReachedFinish += OnReacheFinish;
        _crowd.Stand += Stay;
    }

    public void SetPriority(int priorityNumber)
    {
        _mover.SetPriority(priorityNumber);
    }

    private void OnWin()
    {
        Won?.Invoke();
        _animator.Win();
        _boss.Died -= OnWin;
        _mover.enabled = false;
        _fighter.enabled = false;
    }
}
