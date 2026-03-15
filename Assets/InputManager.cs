using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset actionAsset;
    private InputActionMap _playerActionMap;

    private static InputManager instance;

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            return;
        
        actionAsset.Enable();
        _playerActionMap = actionAsset.FindActionMap("Player");
        
    }

    public static InputAction GetAction(string action)
    {
        return instance._playerActionMap.FindAction(action);
    }
}
