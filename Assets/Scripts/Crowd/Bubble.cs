using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bubble : MonoBehaviour
{
    [SerializeField] private TMP_Text _lable;
    [SerializeField] private Crowd _crowd;

    private string _defaultCount = "0";

    private void OnEnable()
    {
        _crowd.AddedPeople += OnChangeText;
    }

    private void OnDisable()
    {
        _crowd.AddedPeople -= OnChangeText;
    }

    private void Start()
    {
        _lable.text = _defaultCount;
    }

    private void OnChangeText(int count)
    {
        _lable.text = count.ToString();
    }
}
