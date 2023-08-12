using UnityEngine;

namespace FiringRange
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {

        #region SerializedPublicVariables

        [Header("Functional Parameters")]
        public bool CanMove = true;
        public bool CanSprint = true;
        public bool CanHeadbob = true;

        #endregion
        #region SerializedPrivateVariables

        [Header("Movement Parameters")]
        [SerializeField] private float walkSpeed = 3.0f;
        [SerializeField] private float sprintSpeed = 6.0f;
        [SerializeField, Range(1f, 5f)] private float movementSpeedBlendTime = 1.0f;
        [Space(10)]
        [SerializeField] private float gravity = 30.0f;

        [Header("Look Parameters")]
        [SerializeField] private bool mouseInverted = false;
        [SerializeField, Range(.1f, 10f)] private float lookSpeedX = 2.0f;
        [SerializeField, Range(.1f, 10f)] private float lookSpeedY = 2.0f;
        [SerializeField, Range(1, 180)] private float upperLookLimit = 80.0f;
        [SerializeField, Range(1, 180)] private float lowerLookLimit = 80.0f;

        [Header("Headbob Parameters")]
        [SerializeField] private float walkBobSpeed = 14.0f;
        [SerializeField] private float walkBobAmount = 0.025f;
        [SerializeField] private float sprintBobSpeed = 18.0f;
        [SerializeField] private float sprintBobAmount = 0.05f;
        [SerializeField, Range(1f, 5f)] private float headbobSpeedBlendTime = 1.0f;


        #endregion
        #region PublicTrackerVariables
        public bool IsSprinting { get; private set; } = false;

        #endregion
        #region PrivateVariables



        private Camera _playerCamera;
        private CharacterController _characterController;

        private Vector3 _moveDirection;
        private Vector2 _currentInput;

        private float _currentSpeed;

        private float _rotationX = 0;

        private float _defaultYPos = 0;
        private float _timer;

        #endregion
        #region UnityFunctions
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
            _currentSpeed = walkSpeed;
        }

        private void OnEnable()
        {
            GameEventHandler.OnMove += Manager_OnMove;
            GameEventHandler.OnMouseLook += Manager_OnMouseLook;
            GameEventHandler.OnSprint += Manager_OnSprint;
        }

        private void OnDisable()
        {
            GameEventHandler.OnMove -= Manager_OnMove;
            GameEventHandler.OnMouseLook -= Manager_OnMouseLook;
            GameEventHandler.OnSprint -= Manager_OnSprint;
        }

        private void Update()
        {
            if (CanMove)
            {
                HandleHeadbob();
                ApplyFinalMovements();
            }
        }

        #endregion
        #region CustomFunctions
        private void Manager_OnMove(float horizontal, float vertical)
        {
            if (!CanMove) return;

            // Scale the current input according to walk/run speed
            _currentInput = new Vector2(vertical, horizontal) * _currentSpeed;

            // Cache the Y value of the moveDirection from the previous frame, so that we don't by mistakely change the player's Y direction
            float moveDirectionY = _moveDirection.y;

            // Calculate the current move direction according to the prior calculations
            _moveDirection = (transform.TransformDirection(Vector3.forward) * _currentInput.x) + (transform.TransformDirection(Vector3.right) * _currentInput.y);

            // Assign the Y value back to the cached value
            _moveDirection.y = moveDirectionY;
        }

        private void Manager_OnSprint(bool isSprinting)
        {
            if (!CanSprint || !CanMove) return;

            if (IsSprinting = isSprinting)
                _currentSpeed = Mathf.Lerp(_currentSpeed, sprintSpeed, movementSpeedBlendTime * Time.deltaTime);
            else
                _currentSpeed = Mathf.Lerp(_currentSpeed, walkSpeed, movementSpeedBlendTime * Time.deltaTime);
        }

        private void Manager_OnMouseLook(float mouseX, float mouseY)
        {
            if (!CanMove) return;

            mouseY *= (mouseInverted ? -1 : 1);

            _rotationX -= mouseY * lookSpeedY;
            _rotationX = Mathf.Clamp(_rotationX, -upperLookLimit, lowerLookLimit);
            _playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);

            transform.rotation *= Quaternion.Euler(0, mouseX * lookSpeedX, 0);
        }

        private void HandleHeadbob()
        {
            if (!CanHeadbob || !_characterController.isGrounded || Mathf.Abs(_moveDirection.x) <= 0.1 || Mathf.Abs(_moveDirection.z) <= 0.1)
            {
                // Lerp back to the default head position once the player stops movement
                _playerCamera.transform.localPosition = Vector3.Lerp(
                    _playerCamera.transform.localPosition,
                    new Vector3(_playerCamera.transform.localPosition.x, _defaultYPos, _playerCamera.transform.localPosition.z),
                    headbobSpeedBlendTime * Time.deltaTime
                    );
                return;
            }

            _timer += Time.deltaTime * (IsSprinting ? sprintBobSpeed : walkBobSpeed);
            _playerCamera.transform.localPosition = new Vector3(
                _playerCamera.transform.localPosition.x,
                _defaultYPos + Mathf.Sin(_timer) * (IsSprinting ? sprintBobAmount : walkBobAmount),
                _playerCamera.transform.localPosition.z
                );
        }

        private void ApplyFinalMovements()
        {
            if (!_characterController.isGrounded)
                _moveDirection.y -= gravity * Time.deltaTime;

            _characterController.Move(_moveDirection * Time.deltaTime);
        }

        #endregion
    }
}