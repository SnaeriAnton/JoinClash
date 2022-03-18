using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    private Zone _zone = null;

    public void SetZone(Zone zone)
    {
        _zone = zone;
        if (zone != null)
        {
            _zone.Select();
        }
    }

    private void OnDestroy()
    {
        if (_zone != null)
        {
            _zone.Deselect();
        }
    }
}
