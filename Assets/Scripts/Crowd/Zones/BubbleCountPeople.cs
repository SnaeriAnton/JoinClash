using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BubbleCountPeople : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Transform _transform;
    [SerializeField] private GameObject _zone;

    private char _sign = '+';

    private void OnEnable()
    {
        StartCoroutine(ChangeAlpha());
    }

    private void Update()
    {
        _transform.position = Vector3.MoveTowards(_transform.position, _transform.position + new Vector3(0, 1, 0), 0.003f);
    }

    public void SetCountPeople(int count)
    {
        _text.text = _sign + count.ToString();
    }

    private IEnumerator ChangeAlpha()
    {
        var color = _text.color;
        for (int i = 0; i < 255; i++)
        {
            color.a = 1f - (1f / 255f * i);
            _text.color = color;
            yield return null;
        }

        if (_text.color.r <= 1)
        {
            StopCoroutine(ChangeAlpha());
            _zone.SetActive(false);
            yield return null;
        }
    }

    public void SetCountPeople(Vector3 position, int count, bool status)
    {
        _transform.position = new Vector3(position.x, _transform.position.y, position.z);
        if (status == true)
        {
            _sign = '+';
        }
        else
        {
            _sign = '-';
        }
        SetCountPeople(count);
    }
}
