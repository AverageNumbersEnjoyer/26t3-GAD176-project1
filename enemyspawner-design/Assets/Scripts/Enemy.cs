using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event Action<Enemy> Removed;

    private void OnMouseDown()
    {
        // click on enemy simulates defeat in combat
        // for testing convenience
        Destroy(gameObject);
        Debug.Log("destroyed");

    }

    private void OnDestroy()
    {
        // ensure listeners know this enemy no longer exists
        Removed?.Invoke(this);
    }
}
