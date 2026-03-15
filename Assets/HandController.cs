using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    [SerializeField] private int handRaycastHitRange = 100;
    private int _groundLayerMask;
    [SerializeField] private InputActionAsset actionAsset;
    private InputActionMap _playerActionMap;

    private Collider _collider;
    private Vector3 _posAtGrabStartDiff;
    private Vector3 _releaseSpeed;

    private Ball _ball;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _groundLayerMask = LayerMask.GetMask("Ground");
        _collider = GetComponent<Collider>();
        actionAsset.Enable();
        _playerActionMap = actionAsset.FindActionMap("Player");
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
        bool grabPressed = _playerActionMap.FindAction("Grab").WasPressedThisFrame();
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
        
        bool grabReleased =  _playerActionMap.FindAction("Grab").WasReleasedThisFrame();
        if (grabReleased)
        {
            // Reset currently grabbed object 
            ReleaseBall();
            _ball = null;
        }

        if (_ball is not null)
        {
            
            Vector3 positionXZ = new Vector3(transform.position.x + _posAtGrabStartDiff.x, _ball.transform.position.y, transform.position.z + _posAtGrabStartDiff.z);
            _releaseSpeed = positionXZ - _ball.transform.position;
            _ball.transform.position = positionXZ;
        }
    }

    void ReleaseBall()
    {
        _ball.Speed = _releaseSpeed;
    }
}
