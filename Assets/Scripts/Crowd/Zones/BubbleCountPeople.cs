using System.Collections;
using UnityEngine;
using TMPro;

public class BubbleCountPeople : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Transform _transform;
    [SerializeField] private GameObject _zone;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private char _sign = '+';
    private float _speed = 1f;
    private float _speedAlpha = 2.5f;
    private Vector3 _direction = new Vector3(0, 1, 0);
    private Color _color;

    private void OnEnable()
    {
        _color.a = 0;
    }

    private void Update()
    {
        _transform.position = Vector3.MoveTowards(_transform.position, _transform.position + _direction, _speed * Time.deltaTime);
        _text.color = Color.Lerp(_text.color, _color, _speedAlpha * Time.deltaTime);

        if (_spriteRenderer != false)
        {
            _spriteRenderer.color = Color.Lerp(_spriteRenderer.color, _color, _speedAlpha * Time.deltaTime);
        }

        if (_text.color.a <= 0.001)
        {
            Disable();
        }
    }

    private void Disable()
    {
        if (_zone != null)
        {
            _zone.SetActive(false);
        }
        this.enabled = false;
    }

    public void SetCountPeople(int count)
    {
        _text.text = _sign + count.ToString();
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
