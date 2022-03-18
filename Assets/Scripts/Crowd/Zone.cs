using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Zone : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Transform _transform;
    [SerializeField] private GameObject _zone;

    private Human[] _peoples;

    private void Start()
    {
        _peoples = GetComponentsInChildren<Human>();
    }

    public Vector3 Position => _transform.position;

    public void Select()
    {
        _spriteRenderer.color = _selectedColor;
    }

    public void Deselect()
    {
        _spriteRenderer.color = _defaultColor;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Crowd>(out Crowd crowd))
        {
            for (int i = 0; i < _peoples.Length; i++)
            {
                crowd.Add(_peoples[i]);
            }
            _zone.SetActive(false);
        }
    }
}
