using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FiringRange
{
    public class MainHUD : MonoBehaviour
    {
        [SerializeField] Image Crosshair;
        [SerializeField] GameObject Reload;
        [SerializeField] TextMeshProUGUI BulletCount;
        [SerializeField] GameObject PauseMenu;
        [SerializeField] Button RestartButton;
        [SerializeField] Button ExitButton;
        [SerializeField] Slider Sensitivity;
        [SerializeField] TextMeshProUGUI SensitivityValue;

        private void Awake()
        {
            RestartButton.onClick.AddListener(RestartOnClick);
            ExitButton.onClick.AddListener(ExitOnClick);
            Sensitivity.onValueChanged.AddListener(SensitivityValueChanged);
        }

        private void Start()
        {
            SensitivityValue.text = Sensitivity.value.ToString();
            PauseMenu.SetActive(false);
        }

        private void OnEnable()
        {
            GameEventHandler.OnHoverOverWeapon += OnHoverOverWeapon;
            GameEventHandler.OnReloadStarted += OnReloadStarted;
            GameEventHandler.OnReloadCompleted += OnReloadCompleted;
            GameEventHandler.OnBulletCountChanged += OnBulletCountChanged;
            GameEventHandler.OnWeaponEquipped += OnWeaponEquipped;
            GameEventHandler.OnPausePressed += OnPausePressed;
        }

        private void OnDisable()
        {
            GameEventHandler.OnHoverOverWeapon -= OnHoverOverWeapon;
            GameEventHandler.OnReloadStarted -= OnReloadStarted;
            GameEventHandler.OnReloadCompleted -= OnReloadCompleted;
            GameEventHandler.OnBulletCountChanged -= OnBulletCountChanged;
            GameEventHandler.OnWeaponEquipped -= OnWeaponEquipped;
            GameEventHandler.OnPausePressed -= OnPausePressed;
        }

        private void SensitivityValueChanged(float value)
        {
            SensitivityValue.text = value.ToString();
            GameEventHandler.OnSensitivityChanged?.Invoke(value);
        }

        private void ExitOnClick()
        {
            Application.Quit();
        }

        private void RestartOnClick()
        {
            SceneManager.LoadScene(0);
        }

        private void OnPausePressed(bool isPaused)
        {
            Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isPaused;

            PauseMenu.SetActive(isPaused);
        }

        private void OnWeaponEquipped(Weapon weapon)
        {
            OnBulletCountChanged(weapon.BulletsLeft);
        }

        private void OnBulletCountChanged(int bulletCount)
        {
            BulletCount.text = $"Bullets: {bulletCount}";
        }

        private void OnReloadStarted()
        {
            Reload.SetActive(true); 
        }

        private void OnReloadCompleted()
        {
            Reload.SetActive(false);
        }


        private void OnHoverOverWeapon(bool isHovering)
        {
            Crosshair.color = isHovering ? Color.green : Color.black;
        }
    }
}