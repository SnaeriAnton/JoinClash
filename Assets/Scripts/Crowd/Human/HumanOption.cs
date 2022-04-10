using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(HumanMover))]
public class HumanOption : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Animator _animator;
    [SerializeField] private HumanMover _mover;
    [SerializeField] private GameObject _human;
    [SerializeField] private Material _humanInCrowdMaterial;
    [SerializeField] private Material _humanNotInCrowdaterial;
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;

    private float _radius = 0;
    private Vector3 _defaultPosition;
    private bool _inCrowd = false;

    private void Start()
    {
        _defaultPosition = _transform.localPosition;
    }

    private void Update()
    {
        Action();
    }

    public void SetActive(bool isActive)
    {
        _human.SetActive(isActive);
        _inCrowd = isActive;
        _animator.enabled = isActive;
        SetColor(isActive);
    }

    public void SetPosition(Vector3 position)
    {
        _transform.localPosition = position;
    }

    public void SetRadius(float radius)
    {
        _radius = radius;
    }

    public void LookAtRotation(Vector3 position)
    {
        _transform.forward = new Vector3(position.x, 0, position.z) - new Vector3(_transform.position.x, 0, _transform.position.z);
    }

    private void Action()
    {
        if (_inCrowd == false)
        {
            _transform.localPosition = _defaultPosition;
        }
        else
        {
            _transform.localPosition = Vector3.ClampMagnitude(_transform.localPosition, _radius);
        }
    }

    private void SetColor(bool value)
    {
        if (value == true)
        {
            ChangeColor(_humanInCrowdMaterial);
        }
        else
        {
            ChangeColor(_humanNotInCrowdaterial);
        }
    }

    private void ChangeColor(Material material)
    {
        _skinnedMeshRenderer.material = material;
    }
}
