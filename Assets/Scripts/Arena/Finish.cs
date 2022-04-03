using UnityEngine;

public class Finish : MonoBehaviour 
{
    [SerializeField] private Transform _transformBoss;
    [SerializeField] private Boss _boss;

    public Transform PositionBoss => _transformBoss;
    public Boss Boss => _boss;
}
