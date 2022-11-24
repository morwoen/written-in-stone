using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCanvasManager : MonoBehaviour
{
    private bool destroying = false;

    private void OnEnable() {
        InputManager.Instance.Player.Move.performed += OnMovement;
    }

    private void OnDisable() {
        InputManager.Instance.Player.Move.performed -= OnMovement;
    }

    private void OnMovement(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        if (destroying) return;

        destroying = true;
        Destroy(gameObject, 2);
    }
}
