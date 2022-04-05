using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private List<Banner> _banners;
    [SerializeField] private GameObject _gates;
    [SerializeField] private BubbleCountPeople _bubbleCountPeople;
    [SerializeField] private GameObject _bubble;
    [SerializeField] private AudioSource _audioSource;

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

    private void OnDisableBavers(Vector3 position, int count, bool status)
    {
        _bubble.SetActive(true);
        _bubbleCountPeople.SetCountPeople(position, count, status);
        _audioSource.Play();
        _gates.SetActive(false);
    }
}
