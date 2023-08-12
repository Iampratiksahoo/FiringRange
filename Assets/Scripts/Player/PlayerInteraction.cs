using System;
using UnityEngine;

namespace FiringRange
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private PlayerData PlayerData;

        private Camera _playerCamera;
        private IInteractable _currentInteractable;

        private void Awake()
        {
            _playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>();    
        }

        private void OnEnable()
        {
            GameEventHandler.OnInteractPressed += OnInteract;
        }

        private void OnDisable()
        {
            GameEventHandler.OnInteractPressed -= OnInteract;

        }


        private void Update()
        {
            TraceForInteractable();
        }

        private void TraceForInteractable()
        {
            Ray ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * PlayerData.InteractionRange);
            RaycastHit hitInfo;

            if(Physics.Raycast(ray, out hitInfo, PlayerData.InteractionRange, PlayerData.InteractionLayer)) 
            {
                hitInfo.collider.gameObject.TryGetComponent(out _currentInteractable);
                if(_currentInteractable != null) 
                {
                    GameEventHandler.OnHoverOverWeapon?.Invoke(true);
                }
                else
                {
                    GameEventHandler.OnHoverOverWeapon?.Invoke(false);
                }
            }
            else
            {
                // Set the value as null if no raycast hit
                _currentInteractable = null;
                GameEventHandler.OnHoverOverWeapon?.Invoke(false);
            }
        }
        private void OnInteract()
        {
            if (_currentInteractable == null) return; 
            
            _currentInteractable.Interact();
        }
    }
}