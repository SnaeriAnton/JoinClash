using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Banner : MonoBehaviour
{
    [SerializeField] private int _number;
    [SerializeField] private bool _divide;
    [SerializeField] private bool _multiply;
    [SerializeField] private bool _addition;
    [SerializeField] private bool _subtraction;
    [SerializeField] private TMP_Text _text;

    public int Number => _number;
    public bool Divide => _divide;
    public bool Multiply => _multiply;
    public bool Addition => _addition;
    public bool Subtraction => _subtraction;

    public UnityAction Crossed;

    private void Start()
    {
        SetText();
    }

    private void SetText()
    {
        char sign = '$';
        if (_divide == true)
        {
            sign = '/';
        }
        else if (_multiply == true)
        {
            sign = '*';
        }
        else if (_subtraction == true)
        {
            sign = '-';
        }
        else if (_addition == true)
        {
            sign = '+';
        }


        _text.text = sign + _number.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Crowd>(out Crowd crowd))
        {
            Crossed?.Invoke();
        }
    }
}
