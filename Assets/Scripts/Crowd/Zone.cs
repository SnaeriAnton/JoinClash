using UnityEngine;

public class Zone : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform _transform;
    [SerializeField] private GameObject _zone;
    [SerializeField] private Sprite _selectedSprite;
    [SerializeField] private Sprite _deselectSprite;

    private Human[] _people;
    private int _coutnPeopleInZone;
    

    private void Start()
    {
        _people = GetComponentsInChildren<Human>();
        _coutnPeopleInZone = _people.Length;
    }

    public Vector3 Position => _transform.position;

    public void Select()
    {
        _spriteRenderer.sprite = _selectedSprite;
    }

    public void Deselect()
    {
        _spriteRenderer.sprite = _deselectSprite;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Crowd>(out Crowd crowd))
        {
            for (int i = 0; i < _people.Length; i++)
            {
                crowd.AddPeople(_people[i]);
            }
            crowd.AddPeople(_coutnPeopleInZone, true);
            _zone.SetActive(false);
        }
    }
}
