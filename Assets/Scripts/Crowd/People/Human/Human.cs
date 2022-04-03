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

    private Transform _lookAt;
    private int _health = 10;
    private bool _isBoss = false;
    private bool _sees = false;

    public UnityAction Finished;

    private void OnEnable()
    {
        _fighter.Won += OnWin;
    }

    private void OnDisable()
    {
        _fighter.Won -= OnWin;
    }

    private void Update()
    {
        Debug.DrawRay(_transform.position, _transform.forward * 1, Color.red);
        if (_sees == true)
        {
            _transform.LookAt(_lookAt);
        }
    }

    public void ReacheFinish(Transform bossPosition)
    {
        _isBoss = true;
        _fighter.enabled = true;
        _option.enabled = false;
        _mover.enabled = true;
        _mover.SetBossPosition(bossPosition.position);
        See();
        SetLookAt(bossPosition);
        Finished?.Invoke();
    }

    public void SetLookAt(Transform lookAt)
    {
        _lookAt = lookAt;
    }

    public void LookAtRotation(Vector3 position)
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

    public void SetPosition(Vector3 position)
    {
        _option.SetPosition(position);
    }

    public void SetRadius(float radius)
    {
        _option.SetRadius(radius);
    }

    public void ReadyToRun(float distance)
    {
        _animator.GetReadyToRun(distance);
    }

    public void StandPoseToRun()
    {
        _animator.StandPoseToRun();
    }

    public void Stay()
    {
        _animator.Stay();
    }

    public void See()
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

    private void OnWin()
    {
        _animator.Win();
    }
}
