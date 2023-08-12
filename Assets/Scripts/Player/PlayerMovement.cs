using UnityEngine;

namespace FiringRange
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] PlayerData PlayerData;

        private Camera _playerCamera;
        private CharacterController _characterController;

        private Vector3 _moveDirection;
        private Vector2 _currentInput;

        private float _currentSpeed;

        private float _rotationX = 0;

        private float _defaultYPos = 0;
        private float _timer;
        private void Awake()
        { 
            // Get the reference of the required components
            _playerCamera = GetComponentInChildren<Camera>();
            _characterController = GetComponent<CharacterController>();
            _defaultYPos = _playerCamera.transform.localPosition.y;

            // Disable the cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Start()
        {
            _currentSpeed = PlayerData.WalkSpeed;
        }

        private void OnEnable()
        {
            GameEventHandler.OnMove += OnMove;
            GameEventHandler.OnMouseLook += OnMouseLook;
            GameEventHandler.OnSprint += OnSprint;
        }

        private void OnDisable()
        {
            GameEventHandler.OnMove -= OnMove;
            GameEventHandler.OnMouseLook -= OnMouseLook;
            GameEventHandler.OnSprint -= OnSprint;
        }

        private void Update()
        {
            if (PlayerData.CanMove)
            {
                HandleHeadbob();
                ApplyFinalMovements();
            }
        }
        private void OnMove(float horizontal, float vertical)
        {
            if (!PlayerData.CanMove) return;

            // Scale the current input according to walk/run speed
            _currentInput = new Vector2(vertical, horizontal) * _currentSpeed;

            // Cache the Y value of the moveDirection from the previous frame, so that we don't by mistakely change the player's Y direction
            float moveDirectionY = _moveDirection.y;

            // Calculate the current move direction according to the prior calculations
            _moveDirection = (transform.TransformDirection(Vector3.forward) * _currentInput.x) + (transform.TransformDirection(Vector3.right) * _currentInput.y);

            // Assign the Y value back to the cached value
            _moveDirection.y = moveDirectionY;
        }

        private void OnSprint(bool isSprinting)
        {
            if (!PlayerData.CanSprint || !PlayerData.CanMove) return;

            PlayerData.IsSprinting = isSprinting;

            _currentSpeed = Mathf.Lerp(
                _currentSpeed,
                isSprinting ? PlayerData.SprintSpeed : PlayerData.WalkSpeed, 
                PlayerData.MovementSpeedBlendTime * Time.deltaTime);
        }

        private void OnMouseLook(float mouseX, float mouseY)
        {
            if (!PlayerData.CanMove) return;

            mouseY *= (PlayerData.MouseInverted ? -1 : 1);

            _rotationX -= mouseY * PlayerData.LookSpeedY;
            _rotationX = Mathf.Clamp(_rotationX, -PlayerData.UpperLookLimit, PlayerData.LowerLookLimit);
            _playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);

            transform.rotation *= Quaternion.Euler(0, mouseX * PlayerData.LookSpeedX, 0);
        }

        private void HandleHeadbob()
        {
            if (!PlayerData.CanHeadbob || !_characterController.isGrounded || Mathf.Abs(_moveDirection.x) <= 0.1 || Mathf.Abs(_moveDirection.z) <= 0.1)
            {
                // Lerp back to the default head position once the player stops movement
                _playerCamera.transform.localPosition = Vector3.Lerp(
                    _playerCamera.transform.localPosition,
                    new Vector3(_playerCamera.transform.localPosition.x, _defaultYPos, _playerCamera.transform.localPosition.z),
                    PlayerData.HeadbobSpeedBlendTime * Time.deltaTime);
                return;
            }

            _timer += Time.deltaTime * (PlayerData.IsSprinting ? PlayerData.SprintBobSpeed : PlayerData.WalkBobSpeed);
            _playerCamera.transform.localPosition = new Vector3(
                _playerCamera.transform.localPosition.x,
                _defaultYPos + Mathf.Sin(_timer) * (PlayerData.IsSprinting ? PlayerData.SprintBobAmount : PlayerData.WalkBobAmount),
                _playerCamera.transform.localPosition.z);
        }

        private void ApplyFinalMovements()
        {
            if (!_characterController.isGrounded)
                _moveDirection.y -= PlayerData.Gravity * Time.deltaTime;

            _characterController.Move(_moveDirection * Time.deltaTime);
        }
    }
}