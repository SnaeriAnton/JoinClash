using UnityEngine;
using UnityEngine.Events;

public class Finger : MonoBehaviour
{
    public UnityAction Enabled;

    private void OnEnable()
    {
        Enabled?.Invoke();
    }
}
