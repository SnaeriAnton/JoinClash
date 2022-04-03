using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HumanAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private string _run = "Run";
    private string _distance = "Distance";
    private string _startRun = "StartRun";
    private string _idle = "Idle";
    private string _win = "Win";
    private float _defaultDistance = 0;

    private void OnDisable()
    {
        _animator.enabled = false;
    }

    public void Run()
    {
        _animator.SetTrigger(_run); ;
    }

    public void GetReadyToRun(float value)
    {
        _animator.SetFloat(_distance, value);
    }

    public void StandPoseToRun()
    {
        _animator.SetTrigger(_startRun);
    }

    public void Stay()
    {
        _animator.SetTrigger(_idle);
    }

    public void Win()
    {
        _animator.SetTrigger(_win);
        GetReadyToRun(_defaultDistance);
    }
}
