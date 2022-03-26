using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private string _run = "Run";
    private string _distance = "Distance";
    private string _stand = "Stand";
    private string _standPoseTwo = "StandPoseTwo";
    private float _defaultValue = 0;

    public void Run()
    {
        _animator.SetTrigger(_run); ;
    }

    public void GetReadyToRun(float value)
    {
        _animator.SetFloat(_distance, value);
    }

    public void Stand()
    {
        _animator.SetTrigger(_stand);
        _animator.SetFloat(_distance, _defaultValue);
    }

    public void StandPose()
    {
        _animator.SetTrigger(_standPoseTwo);
    }
}
