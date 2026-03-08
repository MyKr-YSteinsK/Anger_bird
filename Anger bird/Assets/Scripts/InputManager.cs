using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    public static PlayerInput PlayerInput;
    private InputAction _mousePosisitonAction;
    private InputAction _mouseAction;
    public static Vector2 MousePosition;
    public static bool WasLeftMouseButtonPressed;
    public static bool WasLeftMouseButtonReleased;
    public static bool IsLeftMousePressed;
    private void Awake() 
    {
        PlayerInput = GetComponent<PlayerInput>();
        _mousePosisitonAction = PlayerInput.actions["MousePosition"];
        _mouseAction = PlayerInput.actions["Mouse"];
    }
    private void Update() 
    {
        MousePosition = _mousePosisitonAction.ReadValue<Vector2>();
        //按下左键(准备发射)
        WasLeftMouseButtonPressed = _mouseAction.WasPressedThisFrame();
        //松开左键(发射小鸟)
        WasLeftMouseButtonReleased = _mouseAction.WasReleasedThisFrame();
        IsLeftMousePressed = _mouseAction.IsPressed();
    }
}
