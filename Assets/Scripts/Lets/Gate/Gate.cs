using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private List<Banner> _banners;
    [SerializeField] private GameObject _gates;

    private void OnEnable()
    {
        foreach (var banner in _banners)
        {
            banner.Crossed += OnDisableBavers;
        }
    }

    private void OnDisable()
    {
        foreach (var banner in _banners)
        {
            banner.Crossed -= OnDisableBavers;
        }
    }

    private void OnDisableBavers()
    {
        _gates.SetActive(false);
    }
}
