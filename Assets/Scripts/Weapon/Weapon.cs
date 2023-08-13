using UnityEngine;

namespace FiringRange
{
    public class Weapon : MonoBehaviour, IInteractable
    {
        [SerializeField] WeaponData WeaponData;
        [SerializeField] Animator WeaponAnimator;

        private bool _isEquipped;

        private int _bulletsLeft;
        private int _bulletsShot;

        private bool _reloading;
        private bool _readyToShoot;

        private Camera _playerCamera;
        private Transform _firingLocation;
        private LineRenderer _laser;

        private Transform _armoryParent;
        private Vector3 _armoryPosition;
        private Quaternion _armoryRotation;

        private Vector3 _swayPos;
        private Vector3 _swayEulerRot;

        public int BulletsLeft
        {
            get => _bulletsLeft;
            private set
            {
                _bulletsLeft = value;

                if(_isEquipped)
                    GameEventHandler.OnBulletCountChanged?.Invoke(_bulletsLeft);
            }
        }
        public bool Equipped
        {
            get => _isEquipped;
            private set
            {
                _isEquipped = value;

                EnableLaser(_isEquipped);
            }
        }

        private void Awake()
        {
            _playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>();
            _firingLocation = transform.Find("FIRING_LOCATION");
            _laser = transform.Find("LASER_LOCATION").GetComponent<LineRenderer>();

            _armoryParent = transform.parent;
            _armoryPosition = transform.localPosition;
            _armoryRotation = transform.localRotation;
        }

        private void Start()
        {
            BulletsLeft = WeaponData.MagazineSize;
            _readyToShoot = true;

            Equipped = false;
        }
        private void OnEnable()
        {
            GameEventHandler.OnMouseLook += OnMouseLook;
            GameEventHandler.OnReloadPressed += OnReload;

            if (WeaponData.Automatic)
            {
                GameEventHandler.OnShootAutomatic += OnShoot;
            }
            else
            {
                GameEventHandler.OnShootNonAutomatic += OnShoot;
            }
        }

        private void OnDisable()
        {
            GameEventHandler.OnMouseLook -= OnMouseLook;
            GameEventHandler.OnReloadPressed -= OnReload;

            if (WeaponData.Automatic)
            {
                GameEventHandler.OnShootAutomatic -= OnShoot;
            }
            else
            {
                GameEventHandler.OnShootNonAutomatic -= OnShoot;
            }
        }

        private void OnMouseLook(float lookInputX, float lookInputY)
        {
            if (!Equipped) return;

            Sway(new Vector2(lookInputX, lookInputY));
            SwayRotation(new Vector2(lookInputX, lookInputY));
            CompositePositionRotation();

            // Look at the center where the Player is aiming at. 
            Vector3 LookPosition = _playerCamera.transform.position + (_playerCamera.transform.TransformDirection(Vector3.forward) * WeaponData.Range);
            transform.LookAt(LookPosition);
        }

        private void SwayRotation(Vector2 lookInput)
        {
            if (!WeaponData.SwayRotation)
            {
                _swayEulerRot = Vector3.zero; 
                return;
            }

            Vector2 invertLook = lookInput * -WeaponData.SwayRotationStep;
            invertLook.x = Mathf.Clamp(invertLook.x, -WeaponData.MaxRotationStep, WeaponData.MaxRotationStep);
            invertLook.y = Mathf.Clamp(invertLook.y, -WeaponData.MaxRotationStep, WeaponData.MaxRotationStep);

            _swayEulerRot = new Vector3(invertLook.y, invertLook.x, invertLook.x);

        }

        private void Sway(Vector2 lookInput)
        {
            if (!WeaponData.Sway) return;

            Vector2 invertLook = lookInput * -WeaponData.SwayStep;
            invertLook.x = Mathf.Clamp(invertLook.x, -WeaponData.MaxDistanceStep, WeaponData.MaxDistanceStep);
            invertLook.y = Mathf.Clamp(invertLook.y, -WeaponData.MaxDistanceStep, WeaponData.MaxDistanceStep);

            _swayPos = invertLook;
        }

        private void CompositePositionRotation()
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _swayPos, WeaponData.SwaySmooth * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(_swayEulerRot), WeaponData.SwaySmoothRot * Time.deltaTime);
        }

        private void OnShoot()
        {
            if(BulletsLeft <= 0)
            {
                SoundManager.instance.PlaySoundEffect(WeaponData.EmptyFireSound);
                return;
            }
            if (!Equipped || !_readyToShoot || _reloading) return;

            _readyToShoot = false;

            RaycastHit raycastHit;
            if(Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out raycastHit, WeaponData.Range, WeaponData.EnemyLayerMask))
            {
                WeaponAnimator.Play("Fire");
                if (raycastHit.collider.CompareTag("Enemy"))
                {
                    Instantiate(WeaponData.BulletImpact, raycastHit.point, Quaternion.identity); // Impact point showcase
                }
                ParticleSystem MuzzleFlash = Instantiate(WeaponData.MuzzleFlash, _firingLocation);
                Destroy(MuzzleFlash.gameObject, MuzzleFlash.main.duration);
                SoundManager.instance.PlaySoundEffect(WeaponData.BulletFireSound);
            }
            
            BulletsLeft -= 1;
            Invoke(nameof(ResetShooting), WeaponData.FireRate);
        }

        private void ResetShooting()
        {
            _readyToShoot = true;
        }

        private void OnReload()
        {
            if (!Equipped || BulletsLeft >= WeaponData.MagazineSize || _reloading) return;

            _reloading = true;
            GameEventHandler.OnReloadStarted?.Invoke();
            Invoke(nameof(OnReloadFinished), WeaponData.ReloadTime);
        }

        private void OnReloadFinished()
        {
            BulletsLeft = WeaponData.MagazineSize;
            _reloading = false;
            GameEventHandler.OnReloadCompleted?.Invoke();
        }

        private void EnableLaser(bool enable)
        {
            if (!WeaponData.Laser) return;

            _laser.enabled = enable;
            _laser.SetPosition(1, Vector3.forward * WeaponData.Range);
        }

        public void Interact()
        {
            Equipped = true;
            GameEventHandler.OnWeaponEquipped?.Invoke(this);
        }

        public void ReturnToArmory()
        {
            Equipped = false;
            transform.parent = _armoryParent;
            transform.localPosition = _armoryPosition;
            transform.localRotation = _armoryRotation;
        }

    }
}