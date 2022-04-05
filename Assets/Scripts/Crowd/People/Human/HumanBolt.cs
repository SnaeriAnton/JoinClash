using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanBolt : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private HumanSpine _humanSpine;
    [SerializeField] private Transform _transform;
    [SerializeField] private ParticleSystem _particleSystem;

    private void OnEnable()
    {
        _humanSpine.Fell += OnDisplayBolt;
    }

    private void OnDisable()
    {
        _humanSpine.Fell -= OnDisplayBolt;
    }

    private void OnDisplayBolt(Vector3 position)
    {
        _transform.position = new Vector3(position.x, _transform.position.y, position.z);
        int random = Random.Range(0, _sprites.Length);
        _spriteRenderer.sprite = _sprites[random];
        _particleSystem.Play();
    }
}
