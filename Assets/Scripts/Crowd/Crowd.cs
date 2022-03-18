using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowd : MonoBehaviour
{
    [SerializeField] private SphereCollider _sphereCollider;
    [SerializeField] private List<Human> _peoples = new List<Human>();
    [SerializeField] private Transform _transform;

    public void Add(Human human)
    {
        _peoples.Add(human);
        human.transform.SetParent(_transform);
       //_sphereCollider.radius = CalcalaterRadiusSphere();
    }

    private float CalcalaterRadiusSphere()
    {
        return 1;
    }
}
