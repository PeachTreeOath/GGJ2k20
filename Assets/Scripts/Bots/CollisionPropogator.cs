using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionPropogator : MonoBehaviour
{
    [SerializeField]
    public OnCollisionEnterEvent OnCollisionEnterEvent;

    private void OnCollisionEnter(Collision collision)
    {
        OnCollisionEnterEvent.Invoke(collision);
    }
}

[System.Serializable]
public class OnCollisionEnterEvent : UnityEvent<Collision> { }
