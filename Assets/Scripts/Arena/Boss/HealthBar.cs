using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Boss _boss;
    [SerializeField] private Slider _slider;
    [SerializeField] private GameObject _healthBar;

    private Vector3 _defaultPosition;

    private void OnEnable()
    {
        _defaultPosition = _transform.position;
        _boss.ChangedHealth += OnSetHealtValue;
        _boss.Died += OnActive;
    }

    private void OnDisable()
    {
        _boss.ChangedHealth -= OnSetHealtValue;
        _boss.Died -= OnActive;
    }

    private void Update()
    {
        StayOnPlace();
    }

    private void StayOnPlace()
    {
        _slider.transform.position = _defaultPosition;
    }

    private void OnSetHealtValue(float value, int maxValue)
    {
        _slider.value -= (float)value / maxValue;
    }

    private void OnActive()
    {
        _healthBar.SetActive(false);
    }
}
