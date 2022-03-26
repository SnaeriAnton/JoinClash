using UnityEngine;
using System.Collections;

public class Human : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private GameObject _human;
    [SerializeField] private HumanMover _humanMover;
    [SerializeField] private HumanAnimator _humanAnimator;

    private float _health = 10;
    private float _damage = 20;
    private Transform _lookAt;
    private bool _inCrowd = false;
    private Vector3 _defaultPosition;
    private float _radius = 0;
    private float _speed = 0.002f;
    private Vector3 _target;
    private bool _isBoss = false;
    private Vector3 _positionAttack;
    private Boss _boss;

    private void Start()
    {
        _defaultPosition = _transform.localPosition;
        _target = Vector3.zero;
        _positionAttack = Vector3.zero;
    }

    private void Update()
    {
        if (_isBoss == false)
        {
            Action();
        }
    }

    public void SetBossPosition(Vector3 position)
    {
        _humanMover.SetBossPosition(position);
        _isBoss = true;
    }

    public void SetLookAt(Transform lookAt)
    {
        _lookAt = lookAt;
    }

    public void SetParent(Transform parent)
    {
        _transform.SetParent(parent);
    }

    public void SetActive(bool isActive)
    {
        _human.SetActive(isActive);
        _inCrowd = isActive;
        _humanAnimator.enabled = isActive;
    }

    public void SetPosition(Vector3 position)
    {
        _transform.localPosition = position;
    }

    public void SetRadius(float radius)
    {
        _radius = radius;
    }

    public void StartPoseToRun()
    {
        _humanAnimator.StandPose();
    }

    public void StandPoseToRun(float distance)
    {
        _humanAnimator.GetReadyToRun(distance);
    }

    public void Stay()
    {
        _humanAnimator.Stand();
    }

    private void Action()
    {
        if (_inCrowd == false)
        {
            _transform.localPosition = _defaultPosition;
        }
        else
        {
            _transform.LookAt(_lookAt);
            _transform.localPosition = Vector3.ClampMagnitude(_transform.localPosition, _radius);
        }
    }
    public void TakingDamage(float damage)
    {
        _health -= damage;
        ChekDeath();
    }

    private void ChekDeath()
    {
        if (_health < 0)
        {
            Destroy(this.gameObject);
        }
    }

    private IEnumerator Push()
    {
        while (true)
        {
            _boss.TakingDamage(_damage);
            yield return new WaitForSeconds(1f);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Boss>(out Boss boss))
        {
            _boss = boss;
            StartCoroutine(Push());
        }
    }
}
