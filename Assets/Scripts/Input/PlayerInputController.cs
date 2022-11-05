using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Linq;

public class PlayerInputController : MonoBehaviour
{
    private InputManager input;

    [SerializeField] private float interactPersistence = 0.2f;

    public bool Dash { get; private set; }
    public bool LookBack { get; private set; }
    public bool Interact { get; private set; }
    public bool Attack { get; private set; }
    public Vector3 Movement { get; private set; }
    public Vector2 LookDirection { get; private set; }

    private void OnEnable() {
        input = InputManager.Instance;

        input.Player.Move.performed += OnMove;
        input.Player.Attack.performed += OnAttack;
        //input.Player.Look.performed += OnLook;
        //input.controlSchemeChange += OnControlSchemeChange;
    }

    private void OnDisable() {
        input.Player.Move.performed -= OnMove;
        input.Player.Attack.performed -= OnAttack;
        //input.Player.Look.performed -= OnLook;
        //input.Player.OpenPauseMenu.performed -= OpenPauseMenu;
        //input.controlSchemeChange -= OnControlSchemeChange;

        Movement = Vector3.zero;
        Dash = false;
        LookBack = false;
        Attack = false;
    }

    void OnMove(InputAction.CallbackContext ctx) {
        var inp = ctx.ReadValue<Vector2>();
        Movement = new Vector3(inp.x, 0, inp.y);
        if (input.CurrentControlScheme == InputManager.ControlScheme.Keyboard) {
            Movement.Normalize();
        }
    }

    //void OnLook(InputAction.CallbackContext ctx) {
    //    LookDirection = ctx.ReadValue<Vector2>();
    //    if (input.CurrentControlScheme == InputManager.ControlScheme.Keyboard) {
    //        LookDirection = new Vector2(LookDirection.x.Remap(0, Screen.width, -1, 1), LookDirection.y.Remap(0, Screen.height, -1, 1));
    //    }
    //}

    void OnAttack(InputAction.CallbackContext ctx) {
        var isPressed = ctx.ReadValueAsButton();
        Attack = isPressed;
    }
}
