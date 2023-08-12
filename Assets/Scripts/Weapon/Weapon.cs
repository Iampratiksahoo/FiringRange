using System;
using UnityEngine;

namespace FiringRange
{
    public class Weapon : MonoBehaviour, IInteractable
    {
        [SerializeField] WeaponData WeaponData;
        public bool isEquipped;

        private int _bulletsLeft;
        private int _bulletsShot;

        private bool _reloading;
        private bool _readyToShoot;
        private Camera _playerCamera;

        private void Awake()
        {
            _playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>();
        }

        private void Start()
        {
            _bulletsLeft = WeaponData.MagazineSize;
            _readyToShoot = true;
        }

        private void OnEnable()
        {
            GameEventHandler.OnShoot += OnShoot;
        }

        private void OnDisable()
        {
            GameEventHandler.OnShoot -= OnShoot;
        }

        private void OnShoot()
        {
            if (!isEquipped) return;

            _bulletsShot = WeaponData.BulletPerTap;
            if (!_readyToShoot || _reloading || _bulletsLeft <= 0) return;

            _readyToShoot = false;

            RaycastHit raycastHit;
            if(Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out raycastHit, WeaponData.Range, WeaponData.EnemyLayerMask))
            {
                Instantiate(WeaponData.BulletImpact, raycastHit.point, Quaternion.identity);
            }
            

            _bulletsLeft -= 1;
            _bulletsShot -= 1;

            Invoke(nameof(ResetShot), WeaponData.TimeBetweenShooting);

            if(_bulletsShot > 0 && _bulletsLeft > 0)
            {
                Invoke(nameof(OnShoot), WeaponData.TimeBetweenShots);
            }
        }

        private void ResetShot()
        {
            _readyToShoot = true;
        }

        private void OnReload()
        {
            if (!isEquipped) return;
            if (_bulletsLeft >= WeaponData.MagazineSize || _reloading) return;

            _reloading = true;
            Invoke(nameof(OnReloadFinished), WeaponData.ReloadTime);
        }

        private void OnReloadFinished()
        {
            _bulletsLeft = WeaponData.MagazineSize;
            _reloading = false;
        }

        public void Interact()
        {
            Destroy(gameObject);
            GameEventHandler.OnWeaponPicked?.Invoke();
        }
    }
}