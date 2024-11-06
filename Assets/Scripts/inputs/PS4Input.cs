using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PS4Input : MonoBehaviour
{
    /// <summary>
    /// References to the actions
    /// </summary>
    InputAction move;
    InputAction look;

    void Start() {
        move = InputSystem.actions.FindAction("Drone/Move");
        look = InputSystem.actions.FindAction("Drone/Look");
    }

    void Update() {
        // obtain the move action value
        Vector2 moveVar = move.ReadValue<Vector2>();
        Vector2 lookVar = look.ReadValue<Vector2>();
        
        
        Debug.Log(moveVar);
    }
}
