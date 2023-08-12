using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FiringRange
{
    public class InputManager : MonoBehaviour
    {
        private CustomInputAction _input;
        private InputAction _locomotion;
        private InputAction _look;

        public static Vector3 PlayerVelocity;

        private void Awake()
        {
            // Initialize the InputAction for WhiteFrost
            _input = new CustomInputAction();

            // Assign the respective Actions
            _locomotion = _input.Player.Locomotion;
            _look = _input.Player.Look;
        }
        private void OnEnable()
        {
            _input?.Enable();
        }

        private void OnDisable()
        {
            _input?.Disable();
        }

        private void Update()
        {
            HandleMovementInput();
            HandleMouseLook();
            //HandleSprinting();
        }

        private void HandleMovementInput()
        {
            // Read the vertical and horizontal values from the InputActions
            float horizontal = _locomotion.ReadValue<Vector2>().x;
            float vertical = _locomotion.ReadValue<Vector2>().y;

            GameEventHandler.OnMove?.Invoke(horizontal, vertical);
        }

        private void HandleMouseLook()
        {
            float mouseX = _look.ReadValue<Vector2>().x;
            float mouseY = _look.ReadValue<Vector2>().y;

            GameEventHandler.OnMouseLook?.Invoke(mouseX, mouseY);
        }
    }
}