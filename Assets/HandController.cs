using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    [SerializeField] private int handRaycastHitRange = 100;
    private int groundLayerMask;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        groundLayerMask = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHandPos();
    }

    void UpdateHandPos()
    {
        var cam = Camera.main;

        Vector2 mousePos = cam.ScreenToViewportPoint(Mouse.current.position.ReadValue());
        Ray ray = cam.ViewportPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, handRaycastHitRange, groundLayerMask))
        {
            this.transform.position = hit.point;
        }
    }
}
