using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Vector3 _direction;
    private float _speed;
    public bool released;
    private Rigidbody _rigidbody;

    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }
    
    public Vector3 Direction
    {
        get => _direction;
        set => _direction = value;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private bool bouncedThisUpdate = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!released)
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.useGravity = false;
            return;
        }
        else
        {
            _rigidbody.useGravity = true;
        }
        
        bouncedThisUpdate = false;
        
        // Apply my speed 
        transform.Translate(_direction * _speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (bouncedThisUpdate)
            return;
        
        if (collision.transform.GetComponent<Bouncer>() is var bouncer && bouncer is not null)
        {
            bouncedThisUpdate = true;  
            Vector3 reflect = Vector3.zero;
            foreach (var contact in collision.contacts)
            {
                reflect += Vector3.Reflect(_direction, collision.contacts[0].normal);
            }

            reflect.y = 0;
            _direction = reflect.normalized;
        }
    }
}
