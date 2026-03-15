using UnityEngine;

public class Ball : MonoBehaviour
{
    private Vector3 _speed;

    public Vector3 Speed
    {
        get => _speed;
        set => _speed = value;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Apply my speed 
        transform.Translate(_speed);
        
        // Gravity? 
    }
}
