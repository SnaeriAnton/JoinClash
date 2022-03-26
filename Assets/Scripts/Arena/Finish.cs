using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private Transform _transformBoss;

    public Vector3 PositionBoss => _transformBoss.position;
}
