using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    [SerializeField] private int handRaycastHitRange = 100;
    private int _groundLayerMask;
    
    private Collider _collider;
    private Vector3 _posAtGrabStartDiff;
    private Vector3 _speed;
    private int _avgSpeedIndex;
    private Vector3[] _lastThreeDistanceDiffs = new Vector3[3];

    private Ball _ball;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _groundLayerMask = LayerMask.GetMask("Ground");
        _collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHandPos();
        UpdateGrab();
    }

    void UpdateHandPos()
    {
        var cam = Camera.main;
        if (cam is null)
            return;

        Vector2 mousePos = cam.ScreenToViewportPoint(Mouse.current.position.ReadValue());
        Ray ray = cam.ViewportPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, handRaycastHitRange, _groundLayerMask))
        {
            this.transform.position = hit.point;
        }
        // Todo - make the hand not snap back and forth when leaving possible raycast area, instead just have it clamp to the edges? 
    }

    void UpdateGrab()
    {
        // Get input
        bool grabPressed = InputManager.GetAction("Grab").WasPressedThisFrame();
        if (grabPressed)
        {
            _ball = null;
            
            // Is my collider overlapping with the pucks? 
            Collider[] colliders = Physics.OverlapBox(this.transform.position, Vector3.one * 0.5f, quaternion.identity);
            foreach (Collider col in colliders)
            {
                if (col.GetComponent<Ball>())
                {
                    // Puck will now follow my hand
                    _ball = col.GetComponent<Ball>();
                    _posAtGrabStartDiff = _ball.transform.position - this.transform.position;
                }
            }
        }
        
        // Speed
        if (_ball is not null)
        {
            Vector3 positionXZ = new Vector3(transform.position.x + _posAtGrabStartDiff.x, _ball.transform.position.y, transform.position.z + _posAtGrabStartDiff.z);
            _lastThreeDistanceDiffs[_avgSpeedIndex++ % _lastThreeDistanceDiffs.Length] = positionXZ - _ball.transform.position;
            _ball.transform.position = positionXZ;
        }
        
        // Release
        bool grabReleased =  InputManager.GetAction("Grab").WasReleasedThisFrame();
        if (grabReleased)
        {
            // Reset currently grabbed object 
            ReleaseBall();
            _ball = null;
        }

        
    }

    void ReleaseBall()
    {
        float averageSpeed = 0;
        Vector3 averageDirection =  Vector3.zero;
        for (int i = 0; i < _lastThreeDistanceDiffs.Length; i++)
        {
            averageDirection += _lastThreeDistanceDiffs[i].normalized;
            averageSpeed += _lastThreeDistanceDiffs[i].magnitude;
        }
        _ball.Speed = averageSpeed / _lastThreeDistanceDiffs.Length;
        _ball.Direction = averageDirection /  _lastThreeDistanceDiffs.Length;
    }
}
