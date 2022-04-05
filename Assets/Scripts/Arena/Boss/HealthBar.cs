using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Boss _boss;
    [SerializeField] private Slider _slider;

    private Vector3 _defaultPosition;

    private void OnEnable()
    {
        _boss.ChangedHealth += OnSetHealtValue;
    }

    private void OnDisable()
    {
        _boss.ChangedHealth -= OnSetHealtValue;
    }

    private void Start()
    {
        _defaultPosition = _transform.position;
    }


    private void Update()
    {
        _slider.transform.position = _defaultPosition;
    }

    private void OnSetHealtValue(float value, int maxValue)
    {
        _slider.value -= (float)value / maxValue;
    }
}
