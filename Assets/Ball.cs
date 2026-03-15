using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Vector3 _direction;
    private float _speed;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        // Apply my speed 
        transform.Translate(_direction * _speed);
        
        // Gravity? 
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<Bouncer>() is var bouncer && bouncer is not null)
        {
            Vector3 reflect = Vector3.zero;
            foreach (var contact in collision.contacts)
            {
                reflect += Vector3.Reflect(_direction, collision.contacts[0].normal);
            }
            _direction = reflect.normalized;
        }
    }
}
