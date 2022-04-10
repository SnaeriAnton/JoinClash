using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BossAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private string _attack = "Attack";
    private string _diying = "Diying";

    public void Attack()
    {
        _animator.SetTrigger(_attack);
    }
    public void Diying()
    {
        _animator.SetTrigger(_diying);
    }
}
