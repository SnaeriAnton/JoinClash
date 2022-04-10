using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(AudioSource))]
public class Zone : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform _transform;
    [SerializeField] private Sprite _selectedSprite;
    [SerializeField] private Sprite _deselectSprite;
    [SerializeField] private GameObject _bubble;
    [SerializeField] private BubbleCountPeople _countPeopleOfZone;
    [SerializeField] private SphereCollider _sphereCollider;
    [SerializeField] private AudioSource _audioSource;

    private Human[] _people;
    private int _coutnPeopleInZone;

    private void Start()
    {
        _people = GetComponentsInChildren<Human>();
        _coutnPeopleInZone = _people.Length;
    }

    public void Select()
    {
        _spriteRenderer.sprite = _selectedSprite;
    }

    public void Deselect()
    {
        _spriteRenderer.sprite = _deselectSprite;
    }

    private IEnumerator ChangeAlpha()
    {
        float alphaChannel = 255;
        float unit = 1f;
        var color = _spriteRenderer.color;
        for (int i = 0; i < alphaChannel; i++)
        {
            color.a = unit - (unit / alphaChannel * i);
            _spriteRenderer.color = color;
            yield return null;
        }
    }

    private void ManagerZone(Crowd crowd)
    {
        _sphereCollider.enabled = false;
        for (int i = 0; i < _people.Length; i++)
        {
            crowd.AddPeople(_people[i]);
        }
        crowd.AddPeople(_coutnPeopleInZone, true);
        _bubble.SetActive(true);
        _countPeopleOfZone.SetCountPeople(_people.Length);
        StartCoroutine(ChangeAlpha());
        _audioSource.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Crowd>(out Crowd crowd))
        {
            ManagerZone(crowd);
        }
    }
}
